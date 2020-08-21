﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Hybriona.UnityRemoteLog
{ 
    public class Server 
    {

        public TcpListener listener;
        private List<Client> clients = new List<Client>();
        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            try
            {
                listener.Start();
                Globals.Instance.errorText.text = "Connected ";
            }
            catch(System.Exception ex)
            {
                Globals.Instance.errorText.text = ex.Message;
            }
            //UnityEngine.Debug.Log("Server started "+ (listener.Server.RemoteEndPoint.ToString()));
            BeginAcceptClient();
        }

        public void Dispose()
        {
            listener.Stop();

            for(int i=0;i< clients.Count;i++)
            {
                clients[i].Dispose();
            }
            clients.Clear();
        }



        public void BeginAcceptClient()
        {
            listener.BeginAcceptTcpClient(new AsyncCallback(TcpBeginAcceptCallback), null);
        }

        private void TcpBeginAcceptCallback(IAsyncResult result)
        {
            TcpClient socket = listener.EndAcceptTcpClient(result);
            Debug.Log("new connection from " + socket.Client.RemoteEndPoint.ToString());
            Client client = null;
            for (int i=0;i<clients.Count;i++)
            {
                if(clients[i].isFree)
                {
                    client = clients[i];
                    Debug.Log("Found slot for "+ socket.Client.RemoteEndPoint.ToString());
                    //Debug.Log("Is client null: "+(client == null));
                    break;
                }
            }
            if(client == null)
            {
                Debug.Log("create new slot "+ socket.Client.RemoteEndPoint.ToString());
                client = new Client();
                clients.Add(client);
            }
            client.Init(socket);
            client.packet.onCompletePacketReceived = OnLogReceived;

            Debug.Log("Client count " + clients.Count);
            BeginAcceptClient();
            
        }

        private void OnLogReceived(string rawData)
        {
            Debug.Log(rawData);
            try
            {
                Globals.Instance.EnqueueToMainThread(() =>
                {
                    Globals.Instance.remoteLogView.scrollManager.EnqueueLog(JsonUtility.FromJson<LogData>(rawData));
                });
            }
            catch(System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        
    }
}
