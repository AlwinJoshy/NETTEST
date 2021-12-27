using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class UDPClientTest : IClient
{
    public UdpClient client;

    int port = 0;
    IPEndPoint endPoint;

    Queue<DataPacket> dataPackets = new Queue<DataPacket>();

    event Action<byte[]> OnMessageRecieve;

    bool transmitData = false;

    public void Close()
    {
        transmitData = false;
        client.Close();
    }

    public void Init(int port, IPEndPoint endPoint)
    {
        client = new UdpClient(port);
        client.DontFragment = false;
        this.endPoint = endPoint;

        Listen();


        transmitData = true;
        TransmitionEngine();
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

    public void SendBytes(DataPacket packet)
    {
        dataPackets.Enqueue(packet);
    }

    async Task TransmitionEngine()
    {
        while (transmitData)
        {
            if (dataPackets.Count > 0)
            {
                DataPacket packet = dataPackets.Dequeue();
                await SendPacket(packet);
            }
            else await Task.Delay(1);
        }
    }


    public async Task SendPacket(DataPacket packet)
    {
        byte[] dataList = packet.GetData();
        int result = await client.SendAsync(dataList, dataList.Length, endPoint);
        packet.Discard();
    }


    // public async void SendBytes(DataPacket packet)
    // {
    //     byte[] dataList = packet.GetData();
    //     int result = await client.SendAsync(dataList, dataList.Length, endPoint);
    //     packet.Discard();
    // }

    public int GetPort()
    {
        return port;
    }
}
