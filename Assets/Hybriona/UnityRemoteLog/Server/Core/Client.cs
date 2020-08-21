using System.Net.Sockets;
using UnityEngine;
using System.IO;
using System;

namespace Hybriona.UnityRemoteLog
{
    public class Client 
    {
        public string id { get; private set; }
        public TcpClient socket { get; private set; }
        public Packet packet { get; private set; }
        public bool isFree { get; private set; } = true;

        public const int BUFFER_SIZE = 4096;
       
       
        private byte[] buffer;
        public void Init(TcpClient socket)
        {
            isFree = false;
            this.socket = socket;
            this.socket.ReceiveBufferSize = BUFFER_SIZE;
            this.socket.SendBufferSize = BUFFER_SIZE;

            if(buffer == null)
            {
                buffer = new byte[BUFFER_SIZE];
            }

            if(packet == null)
            {
                packet = new Packet();
            }
            packet.Reset();
            //packet.onCompletePacketReceived = 
            Handle();
        }

        public void Dispose()
        {
            isFree = true;
            Debug.Log("disconnected");
            if (socket != null)
            {
                socket.Close();
                socket.Dispose();
                
            }
        }

        NetworkStream stream;
        public void Handle()
        {
            stream = socket.GetStream();
            stream.BeginRead(buffer, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), null);
        }

        public void ReadCallback(IAsyncResult result)
        {
            try
            {  
                int readed = stream.EndRead(result);
                if(readed > 0)
                {
                    //Debug.Log("readed: " + readed);
                    packet.Write(buffer, readed);
                    stream.BeginRead(buffer, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), null);
                }
                else
                {
                    Dispose();
                }
                
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message + " - " + ex.StackTrace);
            }
        }
    }
}
