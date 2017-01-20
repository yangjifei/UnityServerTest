using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketClient
{

    private Socket socket;//当前套接字
    private ByteArray byteArray = new ByteArray();//字节数组缓存
    private Thread handleMessage;//处理消息的线程

    public SocketClient()
    {
        handleMessage = new Thread(HandleMessage);
        handleMessage.Start();
    }

    public SocketClient(Socket socket)
    {
        this.socket = socket;
        handleMessage = new Thread(HandleMessage);
        handleMessage.Start();
    }

    public Socket GetSocket()
    {
        return socket;
    }

    public void Destroy()
    {
        handleMessage.Abort();
        socket.Close();
        byteArray.Destroy();
    }

    /// <summary>
    /// 异步连接服务器
    /// </summary>
    public void AsynConnect()
    {
        IPEndPoint serverIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect(serverIp, asyncResult =>
        {
            socket.EndConnect(asyncResult);
            Debug.Log("connect success!");

            AsynRecive();
        }, null);
    }

    /// <summary>
    /// 异步接受信息
    /// </summary>
    public void AsynRecive()
    {
        byte[] data = new byte[1024];
        socket.BeginReceive(data, 0, data.Length, SocketFlags.None,
        asyncResult =>
        {
            int length = socket.EndReceive(asyncResult);
            byte[] temp = new byte[length];
            Debug.Log("接受到的字节数为" + length);
            Array.Copy(data, 0, temp, 0, length);

            byteArray.Write(temp);

            AsynRecive();
        }, null);
    }

    /// <summary>
    /// 异步发送信息
    /// </summary>
    public void AsynSend(SocketMessage sm)
    {
        ByteArray ba = new ByteArray();
        ba.Write(sm.Length);
        ba.Write(sm.ModuleType);
        ba.Write(sm.MessageType);
        ba.Write(sm.Message);

        byte[] data = ba.GetByteArray();
        ba.Destroy();
        socket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
        {
            int length = socket.EndSend(asyncResult);
        }, null);
    }

    /// <summary>
    /// 解析信息
    /// </summary>
    public void HandleMessage()
    {
        int tempLength = 0;//用来暂存信息的长度
        bool hasGetMessageLength = false;//是否得到了消息的长度

        while (true)
        {
            if (!hasGetMessageLength)
            {
                if (byteArray.GetLength() - byteArray.GetReadIndex() > 4)//消息的长度为int，占四个字节
                {
                    tempLength = byteArray.ReadInt32();//读取消息的长度
                    hasGetMessageLength = true;
                }
            }
            else
            {
                //根据长度就可以判断消息是否完整
                //GetReadIndex()可以得到已读的字节
                //注意上面的ReadInt32读取后，读的索引会加上4，要把多余的减去
                if ((tempLength + byteArray.GetReadIndex() - 4) <= byteArray.GetLength())
                {
                    SocketMessage sm = new SocketMessage(byteArray.ReadInt32(), byteArray.ReadInt32(), byteArray.ReadString());
                    //SocketServer.HandleMessage(this, sm);
                    SocketSingletion.Instance.Send(sm);
                    hasGetMessageLength = false;
                }
            }
        }
    }

}
