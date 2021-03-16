// unity c# code
using System;
using System.Collections;
using System.Collections.Generic;
using socket.io;
using UnityEngine;

public class PlayerSocketIO : MonoBehaviour
{
    public static PlayerSocketIO instance;
    //private QSocket socket;
    public GameObject globalData;
    public GameObject pointsMeshSecondPlayer;
    public string url;
    public string token;

    public Socket socket;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
        socket = Socket.Connect(url);
    }

    void Start()
    {
        globalData = GameObject.Find("globalData");

        if (globalData != null)
        {
            url = globalData.GetComponent<GlobalData>().url;
            token = globalData.GetComponent<GlobalData>().Token;
            socket.EmitJson("join_room", globalData.GetComponent<GlobalData>().IdGame.ToString());
        }
        Debug.Log("start");
    }

    public void JoinRoom()
    {
        Debug.Log(socket.IsConnected);

        if (socket.IsConnected)
        {
            socket.Emit("join_room", "9");
        }
    }

    void LateUpdate()
    {
        if (socket.IsConnected)
        {
            socket.On("point", (string data) =>
                {
                    Debug.Log("entre");
                    //Debug.Log(data);
                    string dataJson = data.Replace("\"", "").Replace("'", "\"");
                    //Debug.Log(dataJson);
                    GiveData(dataJson);
                });
        }
    }

    public void GiveData(string data)
    {
        DataSendPoint requestData = JsonUtility.FromJson<DataSendPoint>(data);
        //Debug.Log("holaf : " + requestData.namePoint);
        Debug.Log(pointsMeshSecondPlayer.name + " - " + requestData.namePoint);
        foreach (Transform child in pointsMeshSecondPlayer.transform)
        {
            if (child.gameObject.name == requestData.namePoint)
            {
                child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }

    public void SendPointFig(string namePoint, int rope)
    {

        DataSendPoint dataSendPoint = new DataSendPoint();
        dataSendPoint.player = globalData != null ? globalData.GetComponent<GlobalData>().playerNum : 0;
        dataSendPoint.namePoint = namePoint;
        dataSendPoint.rope = rope;
        dataSendPoint.token = token;
        dataSendPoint.idGame = globalData != null ? globalData.GetComponent<GlobalData>().IdGame : 9;


        string json = JsonUtility.ToJson(dataSendPoint);

        if (socket.IsConnected)
        {
            socket.EmitJson("pointChange", json);
        }


        //socket.Emit("pointChange", json);
    }
}

[Serializable]
public class DataSendPoint
{
    public int player;
    public int point;
    public int idGame;
    public int rope;
    public string namePoint;
    public string token;
}
