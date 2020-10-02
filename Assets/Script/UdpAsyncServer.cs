using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class UdpAsyncServer : MonoBehaviour
{
    


    void Start()
    {
        UdpManager server = new UdpManager(SC.SERVER);
        server.Receive();
    }

    
    void Update()
    {
        
    }

    
}
