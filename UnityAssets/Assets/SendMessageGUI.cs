using UnityEngine;
using System.Collections;

public class SendMessageGUI : MonoBehaviour
{
    string text = "来自客户端的消息";
    string count = "1";
    public void OnGUI()
    {
        text = GUILayout.TextField(text);
        if (GUILayout.Button("发送")||Input.GetKeyUp(KeyCode.KeypadEnter)||Input.GetKeyDown(KeyCode.S))
        {
            int c=1;
            int.TryParse(count,out c);
            for (int i = 0; i < c; i++)
            {
                SocketSingletion.Instance.socketClient.AsynSend(new SocketMessage(19, 89, text));
            }
        }
        count = GUILayout.TextField(count);
    }

}
