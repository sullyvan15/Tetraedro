using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class createTetra : MonoBehaviour
{
    public bool sharedVertices = false;

    public Vector3 p0 = new Vector3(0, 0, 0);
    public Vector3 p1 = new Vector3(1, 0, 0);
    public Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
    public Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);
    Mesh mesh;

    public Vector3[] getVectors()
    {
        Vector3[] vertex = new Vector3[] { p0, p1, p2, p3 };
        return vertex;
    }

    public void Rebuild()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter not found!");
            return;
        }

        mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            meshFilter.mesh = new Mesh();
            mesh = meshFilter.sharedMesh;
        }
        mesh.Clear();

        if (sharedVertices)
        {
            mesh.vertices = new Vector3[] { p0, p1, p2, p3 };
            mesh.triangles = new int[]{
                0,1,2,
                0,2,3,
                2,1,3,
                0,3,1
            };
            mesh.uv = new Vector2[]{
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(0,1),
                new Vector2(1,1),
            };
        }
        else
        {
            mesh.vertices = new Vector3[]{
                p0,p1,p2,
                p0,p2,p3,
                p2,p1,p3,
                p0,p3,p1
            };
            mesh.triangles = new int[]{
                0,1,2,
                3,4,5,
                6,7,8,
                9,10,11
            };

            Vector2 uv0 = new Vector2(0, 0);
            Vector2 uv1 = new Vector2(1, 0);
            Vector2 uv2 = new Vector2(0.5f, 1);

            mesh.uv = new Vector2[]{
                uv0,uv1,uv2,
                uv1,uv0,uv1,
                uv2,uv1,uv0,
                uv1,uv2,uv0
            };
        }

        Color[] color = new Color[mesh.vertices.Length];
        for (int i = 0; i < color.Length; i++)
        {
            if (i % 3 == 0)
                color[i] = Color.blue;
            else if (i % 3 == 1)
                color[i] = Color.red;
            else
                color[i] = Color.yellow;
        }
        mesh.colors = color;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }

    void Start()
    {
        Rebuild();
    }

    void Update()
    {

    }
}
