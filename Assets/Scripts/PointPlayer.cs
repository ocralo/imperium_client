using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointPlayer : MonoBehaviour
{
    public static PointPlayer instance;
    public int pointUser = 0;
    public TextMeshProUGUI textPoint;
    public GameObject gameObjPointService;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    public void AddPointsUser(int point)
    {
        pointUser += point;
        textPoint.text = "Puntuación " + pointUser;
        gameObjPointService.GetComponent<SendPoint_service>().point = pointUser;
        GameObject.Find("globalData").GetComponent<GlobalData>().point = pointUser;
        GameObject.Find("globalData").GetComponent<GlobalData>().time = GeneratePoints.instance.timeRemaining;
    }

}
