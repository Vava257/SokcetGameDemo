using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;


public enum SC
{
    SERVER = 0,
    CLIENT = 1
}
public class UdpManager 
{
    public static string m_Ip = "127.0.0.1";
    public static int m_Port = 61100;
    public static IPEndPoint m_EndPort;
    
    public SC m_SorC;
    public byte[] m_revBuffer = new byte[1024];
    public Socket m_Udp;

    public EndPoint m_listenPoint = new IPEndPoint(IPAddress.Any, 0);
    public EndPoint m_ServerPoint = new IPEndPoint(IPAddress.Parse(m_Ip), m_Port);

    public UdpManager(SC platform)
    {
        m_SorC = platform;
        m_Udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        if (platform == SC.SERVER)
        {
            m_EndPort = new IPEndPoint(IPAddress.Parse(m_Ip), m_Port);
            m_Udp.Bind(m_EndPort);
        }
    }

    public void Receive()
    {
        if (m_SorC == SC.SERVER)
        {
            m_Udp.BeginReceiveFrom(m_revBuffer, 0, m_revBuffer.Length, SocketFlags.None, ref m_listenPoint, AsyncEndReceive, m_Udp);
        }
        else
        {
            m_Udp.BeginReceiveFrom(m_revBuffer, 0, m_revBuffer.Length, SocketFlags.None, ref m_ServerPoint, AsyncEndReceive, m_Udp);
        }
        
    }

    public void Send(byte[] bt,EndPoint point)
    {
        m_Udp.BeginSendTo(bt, 0, bt.Length, SocketFlags.None, point, AsyncEndSend, m_Udp);
    }

    void AsyncEndReceive(IAsyncResult ar)
    {
        Socket socket = ar.AsyncState as Socket;
        if (socket == null) return;

        EndPoint point = new IPEndPoint(IPAddress.Any, 0);
        int length = socket.EndReceiveFrom(ar, ref point);
        if (length < 0)
        {
            OnDisConnect();
        }
        else
        {
            byte[] revs = new byte[length];
            Buffer.BlockCopy(m_revBuffer, 0, revs, 0, length);
            Debug.Log("【 "+m_SorC+"】----收到：" + System.Text.Encoding.Unicode.GetString(revs));
            
            Receive();
            if(m_SorC == SC.SERVER)
            {
                Send(System.Text.Encoding.Unicode.GetBytes("【 " + m_SorC + "】" + "收到远程："), point);
            }
        }
    }


    void AsyncEndSend(IAsyncResult ar)
    {
        Socket socket = ar.AsyncState as Socket;
        if (socket == null) return;

        int length = socket.EndSendTo(ar);
        if(length <= 0)
        {
            OnDisConnect();
        }
        else
        {
            Debug.Log("---发送成功：[" + m_SorC+"]");
        }
    }

    void OnDisConnect()
    {
        if (m_Udp != null)
            m_Udp.Close();
        Debug.Log("---断开链接");
    }

}
