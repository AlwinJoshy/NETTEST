using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class UDPClientTest : IClient
{
    public UdpClient client;

    int port = 0;
    IPEndPoint endPoint;

    event Action<byte[]> OnMessageRecieve;

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void Init(int port, IPEndPoint endPoint)
    {
        client = new UdpClient(port);
        client.DontFragment = false;
        this.endPoint = endPoint;

        Listen();
    }

    async void Listen()
    {
        while (client != null)
        {
            UdpReceiveResult result = await client.ReceiveAsync();
            OnMessageRecieve(result.Buffer);
        }
    }

    public void RegisterListener(Action<byte[]> listenerFunc)
    {
        OnMessageRecieve += listenerFunc;
    }

    public async void SendBytes(DataPacket packet)
    {
        byte[] dataList = packet.GetData();
        int result = await client.SendAsync(dataList, dataList.Length, endPoint);
        packet.Discard();
    }

    public int GetPort()
    {
        return port;
    }
}
