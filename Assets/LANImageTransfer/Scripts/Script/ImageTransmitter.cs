using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.Events;

public class ImageTransmitter : MonoBehaviour
{
    IClient clientObj;
    IPEndPoint endPoint;

    public string dataDisplay = null;

    public ByteArrayUnityEvent OnByteRecieved;

    bool showError;

    void Start()
    {
        clientObj = new UDPClientTest();
        endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4545);
        clientObj.Init(4545, endPoint);
        clientObj.RegisterListener(RecieveData);
        dataDisplay = "";
    }

    void OnDistroy()
    {
        clientObj.Close();
    }

    void RecieveData(byte[] data)
    {

        OnByteRecieved.Invoke(data);

        /*
        int a = 0;
        int recivedChar = BitwiseRead.ReadInt(data, ref a);
        dataDisplay += recivedChar + "|";
        CustomLog.Log(recivedChar.ToString() + "|");
        CheckPattern(recivedChar);
        
        int a = 0;
        BitwiseRead.ReadInt(data, ref a);
        BitwiseRead.ReadInt(data, ref a);
        BitwiseRead.ReadInt(data, ref a);
        int readLength = BitwiseRead.ReadInt(data, ref a);
        CustomLog.Log(readLength.ToString() + "|");

        */
    }

    int nextNumber;
    void CheckPattern(int value)
    {
        if (value != nextNumber)
        {
            showError = true;
        }
        nextNumber += 1;
    }

    string dataString = null;
    int sendIndex;

    public void OnImageDataRecieve(byte[] data)
    {
        int readIndex = 0;
        int arrayLength = 64542;
        int readLength = arrayLength - (4 * 4);

        int totalSendCount = Mathf.CeilToInt((float)data.Length / readLength);

        for (int i = 0; i < totalSendCount; i++)
        {
            DataPacket dataPacket = DataPacket.NewPacket(arrayLength);
            dataPacket.Write(10);
            dataPacket.Write(readIndex);
            dataPacket.Write(totalSendCount);
            int remainingDataLength = data.Length - readIndex;
            readLength = readLength <= remainingDataLength ? readLength : remainingDataLength;
            dataPacket.Write(readLength);
            dataPacket.Write(data, readIndex, readLength);
            readIndex += readLength;
            clientObj.SendBytes(dataPacket);
        }
    }

}
