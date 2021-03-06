using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Game_service : changeScene
{

    public TMP_InputField user;
    public TMP_InputField password;
    public string url;
    public string token;
    public int nextLevel;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GameObject globalData = GameObject.Find("globalData");
        if (GameObject.Find("globalData") != null)
        {
            url = globalData.GetComponent<GlobalData>().url;
            token = globalData.GetComponent<GlobalData>().Token;
        }
    }

    public void Submit()
    {
        StartCoroutine(LoginRequest(user.text, password.text, json =>
        {
            ResponseQueryGame rs = JsonUtility.FromJson<ResponseQueryGame>(json);
            Debug.Log(json);
            if (!rs.error)
            {
                if (rs.idGame != null && rs.idGame != 0)
                {
                    SetGlobalData(rs.idGame);
                    ViewLoadScene(nextLevel);
                }
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

    public IEnumerator LoginRequest(string email, string password, Action<string> result)
    {

        WWWForm form = new WWWForm();
        form.AddField("name", email);
        form.AddField("password", password);


        Debug.Log(url);

        UnityWebRequest www = UnityWebRequest.Post(url + "/game/create", form);

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
public class ResponseQueryGame
{
    public string message;
    public int idGame;
    public bool error;
}

