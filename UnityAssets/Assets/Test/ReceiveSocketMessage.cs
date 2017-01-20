using UnityEngine;
using System.Collections;

public class ReceiveSocketMessage : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SocketSingletion.Instance.sendEvent += PrintInfo;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PrintInfo(SocketMessage sm)
    {
        print("   " + sm.Length + "   " +
            sm.ModuleType + "   " + sm.MessageType + "   " + sm.Message);
    }
}
