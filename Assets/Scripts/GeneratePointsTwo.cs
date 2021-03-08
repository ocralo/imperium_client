using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePointsTwo : MonoBehaviour
{

    public int x, y;

    public GameObject spherePointPrefab;

    void Awake()
    {
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
                //point.transform.localPosition = new Vector3(i, j, this.gameObject.transform.localPosition.z);
                point.transform.localPosition = new Vector3(i, 0, -j);
                //point.AddComponent<PointsPlain>();
                //  xlist.Add(point);
            }
            //listPointsX.Add(xlist);
        }
    }
}
