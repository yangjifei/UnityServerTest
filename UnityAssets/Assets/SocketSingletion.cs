using UnityEngine;
using System.Collections;

public class SocketSingletion : MonoSingletion<SocketSingletion>
{

    public SocketClient socketClient;
    public delegate void SendDelegate(SocketMessage sm);
    public event SendDelegate sendEvent = null;

    // Use this for initialization
    void Start()
    {
        socketClient = new SocketClient();
        socketClient.AsynConnect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Send(SocketMessage sm)
    {
        sendEvent(sm);
    }

    void OnDestroy()
    {
        print("Destroy socketClient");
        socketClient.Destroy();
    }
}
