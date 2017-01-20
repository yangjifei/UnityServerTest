using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class SocketServer
{

    private Socket socket;//当前套接字
    public Dictionary<string, SocketClient> dictionary = new Dictionary<string, SocketClient>();//string为ip地址

    public void Listen()
    {
        IPEndPoint serverIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(serverIp);
        socket.Listen(100);
        Console.WriteLine("server ready.");
        AsynAccept(socket);
    }

    /// <summary>
    /// 异步连接客户端
    /// </summary>
    public void AsynAccept(Socket serverSocket)
    {
        serverSocket.BeginAccept(asyncResult =>
        {
            Socket client = serverSocket.EndAccept(asyncResult);
            SocketClient socketClient = new SocketClient(client);

            string s = socketClient.GetSocket().RemoteEndPoint.ToString();
            Console.WriteLine("连接的客户端为： " + s);
            dictionary.Add(s, socketClient);

            socketClient.AsynRecive();
            socketClient.AsynSend(new SocketMessage(20, 15, "你好，客户端"));
            socketClient.AsynSend(new SocketMessage(20, 15, "你好，客户端"));
            socketClient.AsynSend(new SocketMessage(20, 15, "你好，客户端"));
            AsynAccept(serverSocket);
        }, null);
    }

    /// <summary>
    /// 解析信息
    /// </summary>
    public static void HandleMessage(SocketClient sc, SocketMessage sm)
    {
        Console.WriteLine(sc.GetSocket().RemoteEndPoint.ToString() + "   " +
            sm.Length + "   " + sm.ModuleType + "   " + sm.MessageType + "   " + sm.Message);
    }
}
