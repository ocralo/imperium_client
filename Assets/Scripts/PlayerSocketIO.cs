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
    public string dataToSend;

    public Socket socket;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        socket = Socket.Connect(url);

        if (GameObject.Find("globalData") != null)
        {
            url = globalData.GetComponent<GlobalData>().url;
        }
        Debug.Log("start");

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

    public GameObject GivePointMesh()
    {
        return pointsMeshSecondPlayer;
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
                child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }
        //GameObject pointsMeshSecondPlayerAux = GameObject.Find("Plane Game player 2");
    }

    public void SendPointFig(string namePoint, int player)
    {

        DataSendPoint dataSendPoint = new DataSendPoint();
        dataSendPoint.player = player;
        dataSendPoint.namePoint = namePoint;

        string json = JsonUtility.ToJson(dataSendPoint);

        dataToSend = json;

        if (socket.IsConnected)
        {
            socket.EmitJson("pointChange", json);
        }


        //socket.Emit("pointChange", json);
    }

    private void OnDestroy()
    {
        //socket.Disconnect();
    }

}

[Serializable]
public class DataSendPoint
{
    public int player;
    public string namePoint;
    public string token;
}
