using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Threading;

namespace Hybriona.Logging
{
    public class Connecter : MonoBehaviour
    {
        public System.Action connected;
        public System.Action<long, LogType, string> onLogReceived;
        private EventBasedNetListener listener;
        private NetManager netManager;
        public void Init()
        {
            EventBasedNetListener listener = new EventBasedNetListener();
            netManager = new NetManager(listener);

            listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;
        }

       

        public void Connect(string ip = "127.0.0.1", int port = 11111)
        {
            new Thread(()=>
            {
                if (netManager.IsRunning)
                {
                    netManager.DisconnectAll();
                    netManager.Stop();
                }
                netManager.Start();
                NetPeer peer = netManager.Connect(ip, port, "HybOTALogger");
                if(peer == null)
                {
                    Connect(ip, port);
                }
                else
                {
                    if(connected != null)
                    {
                        connected();
                    }
                    while(true)
                    {
                        netManager.PollEvents();
                        Thread.Sleep(15);
                    }
                }
            }).Start();
        }


        private void Listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            long time = reader.GetLong();
            LogType logType = (LogType)reader.GetInt();
            string message = reader.GetString();
            //Debug.LogFormat("[{0}] {1} {2}", time, logType.ToString(),message);

            if(onLogReceived != null)
            {
                onLogReceived(time,logType,message);
            }
            reader.Recycle();
        }

        private void Awake()
        {
            Init();
           
        }
    }
}
