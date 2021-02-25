using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePoints : MonoBehaviour
{
    //Variables
    public GameObject table;
    public int x, y;
    public List<GameObject> listPoints;
    // Start is called before the first frame update
    void Start()
    {
        //table = this.gameObject;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; i < y; j++)
            {
                /* GameObject point = new GameObject(x + "/" + y);
                point.transform.parent = table.transform;
                point.transform.position = new Vector3(i, 0, j); */
                Debug.Log(x + "/" + y);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
