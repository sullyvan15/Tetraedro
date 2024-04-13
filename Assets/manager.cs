using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
    public GameObject tetrahedron;
    public GameObject[] vetGameObj = new GameObject[24];
    GameObject pai;
    Vector3 m_Center;
    int i = -1;

    public GameObject[] vertices = new GameObject[4];
    public GameObject[] centers = new GameObject[4];

    Vector3 Posit(int x, int a)
    {
        return vetGameObj[x].gameObject.transform.GetChild(a - 1).transform.position;
    }

    void NovoTetra(Vector3 posicao)
    {
        i++;
        GameObject _tetra = Instantiate(tetrahedron, posicao, Quaternion.identity);

        for (int j = 0; j < 4; j++)
        {
            Vector3 _p = _tetra.GetComponent<createTetra>().GetPoint(j) + posicao;
            GameObject p = new GameObject();
            p.transform.position = _p;
            p.transform.parent = _tetra.transform;
        }

        vetGameObj[i] = _tetra;
    }

    void NovoTetraInvertido(Vector3 posicao, int faceIndex, Vector3 rotationEuler)
    {
        NovoTetra(posicao);
        Vector3 p1 = Posit(i, 1);
        Vector3 p2 = Posit(i, faceIndex);
        Vector3 _p = (p1 + p2) / 2;

        GameObject p = new GameObject();
        p.transform.position = _p;

        Vector3 d = p2 - p1;
        p.transform.rotation = Quaternion.LookRotation(d);

        vetGameObj[i].transform.parent = p.transform;
        p.transform.rotation = Quaternion.Euler(rotationEuler);

        vetGameObj[i].transform.parent = null;
        Destroy(p);
    }

    void TetraMagico()
    {
        NovoTetra(new Vector3(0, 0, 0));

        GameObject[] v = new GameObject[4];
        GameObject[] c = new GameObject[4];

        for (int j = 0; j < 4; j++)
        {
            v[j] = new GameObject();
            v[j].transform.position = Posit(0, j + 1);
        }

        for (int j = 0; j < 4; j++)
        {
            NovoTetra(Posit(j, 2));
        }

        for (int j = 0; j < 4; j++)
        {
            c[j] = new GameObject();
            c[j].transform.position = (v[(j + 1) % 4].transform.position + v[(j + 2) % 4].transform.position + v[(j + 3) % 4].transform.position) / 3;
            Vector3 r = c[j].transform.position - v[j].transform.position;
            v[j].transform.rotation = Quaternion.LookRotation(r);
        }

        for (int j = 0; j < 4; j++)
        {
            vertices[j] = v[j];
            centers[j] = c[j];
        }
    }

    void Start()
    {
        TetraMagico();
    }

    int eixo, nivel, direcao;
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            eixo = 0;
        }
        if (Input.GetKeyDown("w"))
        {
            eixo = 1;
        }
        if (Input.GetKeyDown("e"))
        {
            eixo = 2;
        }
        if (Input.GetKeyDown("r"))
        {
            eixo = 3;
        }

        if (Input.GetKeyDown("a"))
        {
            nivel = 0;
        }
        if (Input.GetKeyDown("s"))
        {
            nivel = 1;
        }
        if (Input.GetKeyDown("d"))
        {
            nivel = 2;
        }

        if (Input.GetKeyDown("z"))
        {
            direcao = 1;
            RotateTetraMagico(eixo, nivel, direcao);
        }
        if (Input.GetKeyDown("x"))
        {
            direcao = -1;
            RotateTetraMagico(eixo, nivel, direcao);
        }
    }

    void RotateTetraMagico(int eixo, int nivel, int direcao)
    {
        Vector3 posicao;
        switch (nivel)
        {
            default:
                posicao = vertices[eixo].transform.position;
                break;
            case 0:
                posicao = vertices[eixo].transform.position;
                break;
            case 1:
                posicao = (vertices[eixo].transform.position + centers[eixo].transform.position) / 2;
                break;
            case 2:
                posicao = centers[eixo].transform.position;
                break;
        }

        Collider[] hitColliders = Physics.OverlapBox(posicao, new Vector3(5, 5, 0.1f), vertices[eixo].transform.rotation);
        foreach (Collider collider in hitColliders)
        {
            collider.transform.parent = vertices[eixo].transform;
        }

        vertices[eixo].transform.Rotate(Vector3.forward * 120 * direcao);

        foreach (Collider collider in hitColliders)
        {
            collider.transform.parent = null;
        }
    }
}