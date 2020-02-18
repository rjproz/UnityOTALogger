using System.Net.Sockets;
using UnityEngine;
using System.IO;
using System;

namespace Hybriona.UnityRemoteLog
{
    public class Client : MonoBehaviour
    {
        public string id { get; private set; }
        public TcpClient socket { get; private set; }

        public const int BUFFER_SIZE = 4096;

        private Packet packet;
        private byte[] buffer;
        public void Init(TcpClient socket)
        {
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

            Handle();
        }

        public void Dispose()
        {
            if(socket != null)
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
                packet.Write(buffer, readed);

                stream.BeginRead(buffer, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), null);
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
