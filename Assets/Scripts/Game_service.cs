using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Game_service : changeScene
{

    [Header("Create Game")]
    public TMP_InputField user;
    public TMP_InputField password;
    [Header("Login Game")]
    public TMP_InputField user2;
    public TMP_InputField password2;
    [Header("Data Server")]
    public string url;
    public string token;
    public int nextLevel;
    [Header("Alert Error")]
    public GameObject errorGameObj;
    public TextMeshProUGUI textError;

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
                if (rs.idGame != 0)
                {
                    SetGlobalData(rs.idGame, rs.player);
                    ViewLoadScene(nextLevel);
                }
            }
            else
            {
                textError.text = rs.message;
                ShowErrorMesage();
            }
        }));
    }

    public void ShowErrorMesage()
    {
        errorGameObj.GetComponent<Animator>().Play("showNotificationAlert", 0);
    }

    public void SubmitLogin()
    {
        StartCoroutine(LoginGameRequest(user2.text, password2.text, json =>
        {
            ResponseQueryGame rs = JsonUtility.FromJson<ResponseQueryGame>(json);
            Debug.Log(json);
            if (!rs.error)
            {
                if (rs.idGame != 0)
                {
                    SetGlobalData(rs.idGame, rs.player);
                    ViewLoadScene(nextLevel);
                }
            }
            else
            {
                textError.text = rs.message;
                ShowErrorMesage();
            }
        }));
    }


    public void SetGlobalData(int idGame, int playerGame)
    {
        GameObject globalData = GameObject.Find("globalData");
        if (GameObject.Find("globalData") != null)
        {
            globalData.GetComponent<GlobalData>().IdGame = idGame;
            globalData.GetComponent<GlobalData>().playerNum = playerGame;
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
            textError.text = www.error;
            ShowErrorMesage();
        }
        else
        {
            result(www.downloadHandler.text);
        }
    }

    public IEnumerator LoginGameRequest(string email, string password, Action<string> result)
    {

        WWWForm form = new WWWForm();
        form.AddField("name", email);
        form.AddField("password", password);


        Debug.Log(url);

        UnityWebRequest www = UnityWebRequest.Post(url + "/game/login", form);

        www.SetRequestHeader("Authorization", "Bearer " + token);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            textError.text = www.error;
            ShowErrorMesage();
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
    public int player;
    public int idGame;
    public bool error;
}

