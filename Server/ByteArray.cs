using System.Collections;
using System.IO;
using System;
using System.Text;

//对SocketMessage的读写
public class ByteArray
{

    //为了节省传输的流量，所以传输的是二进制
    //读与写操作都是对一个流来进行的，这里使用MemoryStream
    private MemoryStream memoryStream;
    private BinaryReader binaryReader;
    private BinaryWriter binaryWriter;

    private int readIndex = 0;
    private int writeIndex = 0;

    public ByteArray()
    {
        memoryStream = new MemoryStream();
        binaryReader = new BinaryReader(memoryStream);
        binaryWriter = new BinaryWriter(memoryStream);
    }

    public void Destroy()
    {
        binaryReader.Close();
        binaryWriter.Close();
        memoryStream.Close();
        memoryStream.Dispose();
    }

    public int GetReadIndex()
    {
        return readIndex;
    }

    public int GetLength()
    {
        return (int)memoryStream.Length;
    }

    public int GetPosition()
    {
        //position是从0开始的
        return (int)memoryStream.Position;
    }

    public byte[] GetByteArray()
    {
        return memoryStream.ToArray();
    }

    public void Seek(int offset, SeekOrigin seekOrigin)
    {
        //offset:相对于 SeekOrigin 所指定的位置的偏移量参数
        memoryStream.Seek(offset, seekOrigin);
    }


    #region read
    public bool ReadBoolean()
    {
        Seek(readIndex, SeekOrigin.Begin);
        bool a = binaryReader.ReadBoolean();
        readIndex += 1;
        return a;
    }

    public short ReadInt16()
    {
        Seek(readIndex, SeekOrigin.Begin);
        short a = binaryReader.ReadInt16();
        readIndex += 2;
        return a;
    }

    public int ReadInt32()
    {
        Seek(readIndex, SeekOrigin.Begin);
        Console.WriteLine("ReadInt32:" + binaryReader.BaseStream.Length + " @" + binaryReader.BaseStream.Position);
        int a = binaryReader.ReadInt32();//可能直接读到结尾然后报错?
        readIndex += 4;
        return a;
    }

    public float ReadSingle()
    {
        Seek(readIndex, SeekOrigin.Begin);
        float a = binaryReader.ReadSingle();
        readIndex += 4;
        return a;
    }

    public double ReadDouble()
    {
        Seek(readIndex, SeekOrigin.Begin);
        double a = binaryReader.ReadDouble();
        readIndex += 8;
        return a;
    }

    public string ReadString()
    {
        Seek(readIndex, SeekOrigin.Begin);
        Console.WriteLine("ReadString:" + binaryReader.BaseStream.Length + " @" + binaryReader.BaseStream.Position);
        string a = binaryReader.ReadString();
        //因为binaryWriter写字符串时会在字符串前面加一字节，存储字符串的长度
        readIndex += Encoding.UTF8.GetBytes(a).Length + 1;
        return a;
    }
    #endregion

    #region write
    public void Write(bool value)
    {
        Seek(writeIndex, SeekOrigin.Begin);
        binaryWriter.Write(value);
        writeIndex += 1;
    }

    public void Write(short value)
    {
        Seek(writeIndex, SeekOrigin.Begin);
        binaryWriter.Write(value);
        writeIndex += 2;
    }

    public void Write(int value)
    {
        Seek(writeIndex, SeekOrigin.Begin);
        binaryWriter.Write(value);
        writeIndex += 4;
    }

    public void Write(float value)
    {
        Seek(writeIndex, SeekOrigin.Begin);
        binaryWriter.Write(value);
        writeIndex += 4;
    }

    public void Write(double value)
    {
        Seek(writeIndex, SeekOrigin.Begin);
        binaryWriter.Write(value);
        writeIndex += 8;
    }

    public void Write(string value)
    {
        Seek(writeIndex, SeekOrigin.Begin);
        binaryWriter.Write(value);
        //因为binaryWriter写字符串时会在字符串前面加一字节，存储字符串的长度
        writeIndex += Encoding.UTF8.GetBytes(value).Length + 1;
    }

    public void Write(byte[] value)
    {
        Seek(writeIndex, SeekOrigin.Begin);
        binaryWriter.Write(value);
        writeIndex += value.Length;
    }
    #endregion
}
