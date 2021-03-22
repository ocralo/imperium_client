using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public string Token;
    public int IdGame;
    public int playerNum;
    public string url = "http://localhost:7001";
    public string nameGameText = "";
    public int point = 0;
    public float time = 0.0f;

    /// <summary>
    /// Start is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        GameObject globalData = GameObject.Find("globalData");
        if (GameObject.Find("globalData") != null)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
