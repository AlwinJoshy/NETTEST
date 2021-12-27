using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IClient
{
    void Init(int port, System.Net.IPEndPoint endPoint);

    void SendBytes(DataPacket packet);

    void RegisterListener(System.Action<byte[]> listenerFunc);

    void Close();

    int GetPort();

}