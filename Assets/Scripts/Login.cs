using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Login : changeScene
{
    [Header("Inputs User Data")]
    public TMP_InputField user;
    public TMP_InputField password;
    [Header("Data Server")]
    public string url;
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
        }
    }

    public void Submit()
    {
        StartCoroutine(LoginRequest(user.text, password.text, json =>
        {
            ResponseQuery rs = JsonUtility.FromJson<ResponseQuery>(json);
            Debug.Log(json);
            if (rs.auth)
            {
                CreateGlobalToken(rs.token);
                ViewLoadScene(nextLevel);
            }
            else
            {
                textError.text = rs.message;
                ShowErrorMesage();
            }
            Debug.Log(rs.auth);
        }));
    }

    public void ShowErrorMesage()
    {
        errorGameObj.GetComponent<Animator>().Play("showNotificationAlert", 0);
    }

    public void CreateGlobalToken(string token)
    {
        GameObject globalData = GameObject.Find("globalData");
        if (GameObject.Find("globalData") != null)
        {
            globalData.GetComponent<GlobalData>().Token = token;
        }
        else
        {
            GameObject globalDataNew = new GameObject();
            globalDataNew.name = "globalData";
            globalDataNew.AddComponent<GlobalData>();
            globalDataNew.GetComponent<GlobalData>().Token = token;
            DontDestroyOnLoad(globalDataNew);
        }
    }

    /*  */
    public IEnumerator LoginRequest(string email, string password, Action<string> result)
    {

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);


        Debug.Log(url);

        UnityWebRequest www = UnityWebRequest.Post(url + "/user/login", form);

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

    public IEnumerator getDataDB(string user, string password, Action<bool> result)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {

            //webRequest.SetRequestHeader("Authorization", "Bearer " + token);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
                textError.text = webRequest.error;
                ShowErrorMesage();
            }
            else
            {
                string json = webRequest.downloadHandler.text;
                //ResponseBasket responseBasket = JsonUtility.FromJson<ResponseBasket>(json);
                Debug.Log(json);
                //result(responseBasket);
            }
        }
    }
}

[Serializable]
public class ResponseQuery
{
    public string message;
    public bool auth;
    public string error;
    public string token;
}
