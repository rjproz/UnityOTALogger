using System;
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
            listener.Start();
            
            UnityEngine.Debug.Log("Server started "+ (listener.Server.RemoteEndPoint.ToString()));
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
            Client client = new Client();
            client.Init(socket);
            client.packet.onCompletePacketReceived = OnLogReceived;
            clients.Add(client);
            BeginAcceptClient();
        }

        private void OnLogReceived(string rawData)
        {
            Debug.Log(rawData);
            try
            {
                Globals.Instance.EnqueueToMainThread(() =>
                {
                    Globals.Instance.scrollManager.EnqueueLog(JsonUtility.FromJson<LogData>(rawData));
                });
            }
            catch(System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        
    }
}
