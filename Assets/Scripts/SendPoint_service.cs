using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class SendPoint_service : changeScene
{
    [Header("Point to send Server")]
    public int point;
    [Header("Data necesary to send Server")]
    public string url;
    public string token;
    public int gameId;
    public int nextLevel;
    public GameObject cameraVr;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        GameObject globalData = GameObject.Find("globalData");
        if (GameObject.Find("globalData") != null)
        {
            url = globalData.GetComponent<GlobalData>().url;
            token = globalData.GetComponent<GlobalData>().Token;
            gameId = globalData.GetComponent<GlobalData>().IdGame;
        }
    }

    public void Submit()
    {
        StartCoroutine(SendPiontRequest(point, gameId, json =>
        {
            ResponseQueryGame rs = JsonUtility.FromJson<ResponseQueryGame>(json);
            Debug.Log(json);
            if (!rs.error)
            {
                //SetGlobalData(rs.idGame);
                cameraVr.GetComponent<VrModeController>().ExitVR();
                ViewLoadScene(nextLevel);

            }
            else
            {

            }
        }));
    }


    public void SetGlobalData(int idGame)
    {
        GameObject globalData = GameObject.Find("globalData");
        if (GameObject.Find("globalData") != null)
        {
            globalData.GetComponent<GlobalData>().IdGame = idGame;
        }
        else
        {
            GameObject globalDataNew = new GameObject();
            globalDataNew.name = "globalData";
            globalDataNew.AddComponent<GlobalData>();
            globalData.GetComponent<GlobalData>().IdGame = idGame;
            globalData.GetComponent<GlobalData>().url = url;
            globalData.GetComponent<GlobalData>().Token = token;
            DontDestroyOnLoad(globalDataNew);
        }
    }

    public IEnumerator SendPiontRequest(int point, int gameId, Action<string> result)
    {

        WWWForm form = new WWWForm();
        form.AddField("point", point);
        form.AddField("gameId", gameId);


        Debug.Log(url);

        UnityWebRequest www = UnityWebRequest.Post(url + "/game/points", form);

        www.SetRequestHeader("Authorization", "Bearer " + token);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            result(www.downloadHandler.text);
        }
    }
}

[Serializable]
public class ResponseQueryPoint
{
    public string message;
    public bool error;
}
