using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System;
namespace Hybriona.UnityRemoteLog
{
    public class UnityRemoteLogger : MonoBehaviour
    {
        private TcpClient socket;
        private NetworkStream networkStream;
        private string appID;
        private bool isConnected;

        private byte[] sizeData;
        private byte[] binaryData;

        private static UnityRemoteLogger instance;
        public static UnityRemoteLogger Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject o = new GameObject("UnityRemoteLogger");
                    instance = o.AddComponent<UnityRemoteLogger>();
                    DontDestroyOnLoad(o);
                }
                return instance;
            }
        }


        public void StartLogging(string appID, string ip, int port)
        {
            this.appID = appID;
            if(sizeData == null)
            {
                sizeData = new byte[2];
            }
            if (socket == null)
            {
                socket = new TcpClient();
            }
            socket.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), null);
        }

        public void ConnectCallback(IAsyncResult result)
        {
            try
            {
                socket.EndConnect(result);
                socket.SendBufferSize = socket.ReceiveBufferSize = 4096;
                networkStream = socket.GetStream();
                isConnected = true;
            }
            catch(System.Exception ex)
            {

            }
        }

        public void Log(string message,string tag = null)
        {
            if(isConnected)
            {
                LogData logData = new LogData();
                logData.appId = appID;
                logData.tag = tag;
                logData.message = message;
                logData.type = LogData.Type.Info;

                SendData(logData);
            }
        }

        public void LogWarning(string message, string tag = null)
        {
            if (isConnected)
            {
                LogData logData = new LogData();
                logData.appId = appID;
                logData.tag = tag;
                logData.message = message;
                logData.type = LogData.Type.Warning;

                SendData(logData);
            }
        }

        public void LogError(string message, string tag = null)
        {
            if (isConnected)
            {
                LogData logData = new LogData();
                logData.appId = appID;
                logData.tag = tag;
                logData.message = message;
                logData.type = LogData.Type.Error;

                SendData(logData);
            }
        }


        private void SendData(LogData logData)
        {
            logData.timestamp = System.DateTime.Now.ToFileTime();
            string data = JsonUtility.ToJson(logData);
            binaryData = null;
            binaryData = System.Text.Encoding.UTF8.GetBytes(data);

            if(binaryData.Length > 65535)
            {
                return;
            }
            sizeData[0] = (byte)((int)(binaryData.Length / 256f));
            sizeData[1] = (byte)((int)(binaryData.Length % 256f));

            networkStream.Write(sizeData, 0, sizeData.Length);
            networkStream.Write(binaryData, 0, binaryData.Length);
            //networkStream.Write(binaryData,0, binaryData.Length);
            binaryData = null;
        }



        private void OnDestroy()
        {
            if(socket != null)
            {
                socket.Close();
                socket.Dispose();
            }
        }
    }
}
