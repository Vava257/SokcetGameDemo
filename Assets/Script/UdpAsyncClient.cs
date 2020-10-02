using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class UdpAsyncClient : MonoBehaviour
{
    public EndPoint m_RemotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 61100);
    void Start()
    {
        UdpManager client = new UdpManager(SC.CLIENT);
        for(int i = 0; i <20; i++)
        {
            client.Send(System.Text.Encoding.Unicode.GetBytes("客户端发来Test----" + i+"----"), m_RemotePoint);
        }
        
        client.Receive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
