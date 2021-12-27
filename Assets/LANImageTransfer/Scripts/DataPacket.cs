using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPacket
{
    int index = 0;
    byte[] dataList;
    static Queue<DataPacket> dataPackets = new Queue<DataPacket>();

    public static DataPacket NewPacket(int size)
    {

        DataPacket packet = null;

        System.Action MakeNewPacket = () =>
        {
            packet = new DataPacket(size);
        };

        if (dataPackets.Count > 0)
        {
            packet = dataPackets.Dequeue();
            if (packet.dataList.Length != size)
            {
                MakeNewPacket();
            }
        }
        else
        {
            MakeNewPacket();
        }
        packet.index = 0;
        return packet;
    }

    public void Discard()
    {
        dataPackets.Enqueue(this);
    }

    DataPacket(int dataSize)
    {
        dataList = new byte[dataSize];
    }

    public byte[] GetData()
    {
        return dataList;
    }

    public void Write(int value)
    {
        dataList[index] = (byte)(value >> 24);
        dataList[index + 1] = (byte)(value >> 16);
        dataList[index + 2] = (byte)(value >> 8);
        dataList[index + 3] = (byte)value;
        index += 4;
    }

    public void Write(char value)
    {
        dataList[index] = (byte)value;
        index += 2;
    }

}
