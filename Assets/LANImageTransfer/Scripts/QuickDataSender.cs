using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class QuickDataSender : MonoBehaviour
{
    IClient clientObj;
    IPEndPoint endPoint;

    public string dataDisplay = null;

    bool showError;

    void Start()
    {
        clientObj = new UDPClientTest();
        endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4545);
        clientObj.Init(4545, endPoint);
        clientObj.RegisterListener(RecieveData);
        dataDisplay = "";
    }

    void RecieveData(byte[] data)
    {
        int a = 0;
        dataDisplay += BitwiseRead.ReadInt(data, ref a) + "|";
        // CheckPattern(int.Parse(BitwiseRead.ReadChar(data, ref a).ToString()));
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DirectSend(0, 100);
            //StartCoroutine(CountSend());
        }

        if (showError)
        {
            Debug.Log("Chain Broken...");
            showError = false;
        }

    }

    void DirectSend(int start, int length)
    {
        for (int i = start; i < start + length; i++)
        {
            DataPacket dataPacket = DataPacket.NewPacket(1024);
            dataPacket.Write(i);
            clientObj.SendBytes(dataPacket);
        }
    }

    IEnumerator CountSend()
    {
        DirectSend(0, 64);
        yield return new WaitForSeconds(0.2f);
        DirectSend(64, 64);
    }

}
