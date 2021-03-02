using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsPlain : MonoBehaviour
{
    //vars
    public bool statePoint = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetStatePoint(bool state)
    {
        statePoint = state;
    }
    public bool GetStatePoint()
    {
        return statePoint;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnterClick()
    {
        ChangeColor();
    }
    public void ChangeColor()
    {
        //this.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
}
