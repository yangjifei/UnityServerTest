using System.Collections;
using System.Text;

public class SocketMessage
{

    //大模块，例如登录注册模块，角色模块(行走、释放技能)，购买模块
    public int ModuleType { get; set; }
    //进一步分类，例如登录注册模块中含有登录和注册两种类型
    public int MessageType { get; set; }
    //SocketMessage的核心，包含各种内容
    public string Message { get; set; }
    //信息的总字节数
    public int Length { get; set; }

    public SocketMessage(int moduleType, int messageType, string message)
    {
        ModuleType = moduleType;
        MessageType = messageType;
        Message = message;
        //Length的字节数，ModuleType的字节数，MessageType的字节数，系统自动添加的存储字符串长度的字节，Message的字节数
        Length = 4 + 4 + 4 + 1 + Encoding.UTF8.GetBytes(message).Length;
    }

}
