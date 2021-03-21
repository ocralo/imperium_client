using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetGlobalDataFinished : MonoBehaviour
{
    public GameObject globalDataObj;
    public TextMeshProUGUI textTimer;
    public TextMeshProUGUI textPoint;
    // Start is called before the first frame update
    void Start()
    {
        globalDataObj = GameObject.Find("globalData");
        textTimer.text = globalDataObj.GetComponent<GlobalData>().time +"";
        textPoint.text = globalDataObj.GetComponent<GlobalData>().point +"";
    }
}
