// unity c# code
using Socket.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;

public class PlayerSocketIO : MonoBehaviour
{
    private QSocket socket;

    void Start()
    {
        Debug.Log("start");
        socket = IO.Socket("http://192.168.39.164:7001");

        socket.On(QSocket.EVENT_CONNECT, () =>
        {
            Debug.Log("Connected");
            socket.Emit("chat", "test mobile");
        });

        socket.On("chat", data =>
        {
            Debug.Log("data : " + data);
        });
    }

    private void OnDestroy()
    {
        socket.Disconnect();
    }
}
