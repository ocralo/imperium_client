using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsPlain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnterClick()
    {
        ChangeColor();
    }
    /* public void OnPointerEnter()
    {
        ChangeColor();
    }
    public void OnPointerExit()
    {
        ChangeColor();
    } */
    public void ChangeColor()
    {
        this.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
}
