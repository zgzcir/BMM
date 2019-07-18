using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Common;
using UnityEngine;

public class ClientManager : BaseManager
{
    public ClientManager(Facade facade) : base(facade)
    {
    }

    private const string IP = "47.106.254.223"; //47.106.254.223//127.0.0.1
    private const int PORT = 7788;
    private Socket clientSocket;
    private Message msg = new Message();

    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }

        catch (Exception e)
        {
            Debug.Log("Wrong:无法连接到服务器端，请检查您的网络.." + e);
        }
    }
    private void Start()
    {
      
        if (clientSocket == null || clientSocket.Connected == false)
        { Debug.Log("dwa");
            return;
        }

        clientSocket.BeginReceive(msg.Data, msg.dynamicLength, msg.RemainSize, SocketFlags.None, ReciveCallBack, null);
    }

    private void ReciveCallBack(IAsyncResult ar)
    {
        
        try
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }
            int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count, OnProcessDataCallBack);
            Start();
            
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
    }
    private void OnProcessDataCallBack(RequestCode requestCode, string data)
    {
        facade.HandleResponse(requestCode, data);
    }

    public void SendRequest(ControllerCode controllerCode, RequestCode requestCode, string data)
    {
        byte[] bytes = Message.PackData(controllerCode, requestCode, data);
        clientSocket.Send(bytes);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.Log("无法关闭与服务器的连接" + e);
        }
    }
}