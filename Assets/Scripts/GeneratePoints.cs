﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePoints : MonoBehaviour
{
    //Variables
    public static GeneratePoints instance;
    public int lineCount;
    public bool lineComplete;
    public float initPosX, initPosY;
    public float finalPosX, finalPosY;
    public LineRenderer lineR;
    public List<Vector3> listPointEneable = new List<Vector3>();
    public int x, y;
    public string pointNameInit;
    public GameObject spherePointPrefab;

    public bool isInit = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
        for (int i = 0; i <= x; i++)
        {
            //List<GameObject> xlist = new List<GameObject>();
            for (int j = 0; j <= y; j++)
            {
                //  List<GameObject> ylist = new List<GameObject>();
                //GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GameObject point = Instantiate(spherePointPrefab, new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z), Quaternion.identity);
                point.name = i + "-" + j;
                point.tag = "pointTable";
                point.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                point.transform.parent = this.gameObject.transform;
                point.transform.localPosition = new Vector3(i, 0, j);
                //point.AddComponent<PointsPlain>();
                //  xlist.Add(point);
            }
            //listPointsX.Add(xlist);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineR.positionCount = 1;
    }

    public float CalculateSurfaceArea(Mesh mesh)
    {
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;

        double sum = 0.0;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;

            sum += Vector3.Cross(a, b).magnitude;
        }

        Debug.Log((float)(sum / 2.0) + 4);
        return (float)(sum / 2.0) + 4;
    }

    public float SuperficieIrregularPolygon()
    {
        float temp = 0;
        int i = 0;
        for (; i < listPointEneable.Count; i++)
        {
            if (i != listPointEneable.Count - 1)
            {
                float mulA = listPointEneable[i].x * listPointEneable[i + 1].z;
                float mulB = listPointEneable[i + 1].x * listPointEneable[i].z;
                temp = temp + (mulA - mulB);
            }
            else
            {
                float mulA = listPointEneable[i].x * listPointEneable[0].z;
                float mulB = listPointEneable[0].x * listPointEneable[i].z;
                temp = temp + (mulA - mulB);
            }
        }
        temp *= 0.5f;
        return Mathf.Abs(temp);
    }

    public void CreateQuad()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        //Mesh mesh = GetComponent<MeshFilter>().mesh;
        Mesh mesh = new Mesh();

        mesh.Clear();
        mesh.vertices = listPointEneable.ToArray();
        Vector2[] vector2List = new Vector2[listPointEneable.ToArray().Length];

        for (int i = 0; i < listPointEneable.ToArray().Length; i++)
        {
            vector2List[i] = new Vector2(listPointEneable[i].x, listPointEneable[i].z);
        }

        /* int[] tris = new int[12]
                {
            // lower left triangle
            5, 0, 1,
            // upper right triangle
            5, 1, 4,
            1, 2, 3,
            3, 4, 1
                }; */

        //UVs
        var uvs = new Vector2[listPointEneable.Count];

        for (x = 0; x < listPointEneable.Count; x++)
        {
            if ((x % 2) == 0)
            {
                uvs[x] = new Vector2(0, 0);
            }
            else
            {
                uvs[x] = new Vector2(1, 1);
            }
        }

        //Triangles
        int num = 0;

        int[] tris = new int[3 * (listPointEneable.Count - 2)];
        //3 verts per triangle * num triangles
        int C1;
        int C2;
        int C3;

        if (num == 0)
        {
            C1 = 0;
            C2 = 1;
            C3 = 2;


            for (x = 0; x < tris.Length; x += 3)
            {
                tris[x] = C1;
                tris[x + 1] = C2;
                tris[x + 2] = C3;

                C2++;
                C3++;
            }
        }
        else
        {
            C1 = 0;
            C2 = listPointEneable.Count - 1;
            C3 = listPointEneable.Count - 2;

            for (x = 0; x < tris.Length; x += 3)
            {
                tris[x] = C1;
                tris[x + 1] = C2;
                tris[x + 2] = C3;

                C2--;
                C3--;
            }
        }
        mesh.triangles = tris;


        mesh.uv = uvs;
        meshFilter.mesh = mesh;

        //CalculateSurfaceArea(mesh);
        Debug.Log(SuperficieIrregularPolygon());
        //mesh.triangles = lineCount;
    }



    // Update is called once per frame
    void Update()
    {

    }
}
