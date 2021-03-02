using System.Collections;
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



    // Update is called once per frame
    void Update()
    {

    }
}
