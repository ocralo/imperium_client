// unity c# code
using System;
using System.Collections;
using System.Collections.Generic;
using Socket.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;

public class PlayerSocketIO : MonoBehaviour
{
    public static PlayerSocketIO instance;
    private QSocket socket;
    public GameObject globalData;
    public GameObject pointsMeshSecondPlayer;
    public string url;
    public string dataToSend;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (GameObject.Find("globalData") != null)
        {
            url = globalData.GetComponent<GlobalData>().url;
        }
        Debug.Log("start");
        socket = IO.Socket(url);

        socket.On(QSocket.EVENT_CONNECT, () =>
        {
            Debug.Log("Connected");
            //socket.Emit("chat", "test mobile");
        });

        /* socket.On("onPointChange", data =>
        {
            DataSendPoint  requestData = JsonUtility.FromJson<DataSendPoint>(data.ToString());
                Debug.Log("hola1 : " );
            foreach (Transform item in pointsMeshSecondPlayer.transform)
            {
                Debug.Log("holaf : " +item.transform.name);
                if(item.transform.name == requestData.namePoint){
                    item.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }else{
                    item.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;

                }
            }
            Debug.Log("data : " + data.ToString());
        }); */
        socket.On("point", data =>
        {
            string dataJson = data.ToString().Replace("'","\"").Replace("\\","").Replace("}\"","}").Substring(1);
            GiveData(dataJson);
        });

        /* socket.On("chat", data =>
        {
            Debug.Log("data : " + data);
        }); */
    }

    public void GiveData(string data)
    {
            DataSendPoint  requestData= JsonUtility.FromJson<DataSendPoint>(data);
            Debug.Log("holaf : " + requestData.namePoint);             
    }

    public void SendPointFig(string namePoint,int player){

        DataSendPoint dataSendPoint = new DataSendPoint();
        dataSendPoint.player = player;
        dataSendPoint.namePoint = namePoint;

        string json = JsonUtility.ToJson(dataSendPoint);

        dataToSend = json;

        socket.Emit("pointChange", json);
    }

    private void OnDestroy()
    {
        socket.Disconnect();
    }
    
}

[Serializable]
public class DataSendPoint
{
    public int player;
    public string namePoint;
    public string token;
}
