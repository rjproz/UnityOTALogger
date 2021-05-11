using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Threading;
namespace Hybriona.Logging
{
    public class HybOTALogger : MonoBehaviour
    {
        private NetManager netManager;
        private int port;
        private NetDataWriter dataWriter;
        private string connectedMessage;
        private bool serverStarted = false;
        #region Static Methods
        public static void Init(int port = 11111,float wait = 60)
        {
            Instance._Init(port);
        }

        public static void Log(string message)
        {
            instance.m_Log(message, LogType.Log);
        }

        public static void LogError(string message)
        {
            instance.m_Log(message, LogType.Error);
        }

        public static void LogWarning(string message)
        {
            instance.m_Log(message, LogType.Warning);
        }

        public static void LogFormat(string format,params object [] parameters)
        {
            Log(string.Format(format, parameters));
        }

        public static void LogErrorFormat(string format, params object[] parameters)
        {
            LogError(string.Format(format, parameters));
        }
      

        public static void LogWarningFormat(string format, params object[] parameters)
        {
            LogWarning(string.Format(format, parameters));
        }

        public static void Log(string message,LogType logType)
        {
            instance.m_Log(message, logType);
        }

        #endregion

        #region Methods
        private void OnDisable()
        {
            netManager.Stop();
        }

        private void _Init(int port = 11111)
        {
            this.port = port;
            this.connectedMessage = string.Format("Connected to {0} (v{3})\n{1}\n{2}", Application.productName, Application.identifier, Application.platform, Application.version);
            new Thread(Process).Start();
        }

        private void m_Log(string message,LogType type)
        {
            if (netManager != null)
            {
                dataWriter.Reset();
                dataWriter.Put(System.DateTime.Now.ToFileTimeUtc());
                dataWriter.Put((int)type);
                dataWriter.Put(message);
                netManager.SendToAll(dataWriter, DeliveryMethod.ReliableOrdered);
            }
        }

        private void Process()
        {

            EventBasedNetListener listener = new EventBasedNetListener();
            netManager = new NetManager(listener);
            dataWriter = new NetDataWriter();


            listener.ConnectionRequestEvent += (ConnectionRequest request) =>
            {
                NetPeer netPeer = request.AcceptIfKey("HybOTALogger");

                dataWriter.Reset();
                dataWriter.Put(System.DateTime.Now.ToFileTimeUtc());
                dataWriter.Put((int)LogType.Log);
                dataWriter.Put(connectedMessage);
                netPeer.Send(dataWriter, DeliveryMethod.ReliableOrdered);
            };

            serverStarted = netManager.Start(port);
            while (true)
            {
                netManager.PollEvents();
                Thread.Sleep(15);
            }
        }



        #endregion


        private static HybOTALogger instance;
        protected static HybOTALogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<HybOTALogger>();
                    if (instance == null)
                    {
                        GameObject o = new GameObject("HybOTALogger");
                        DontDestroyOnLoad(o);
                        instance = o.AddComponent<HybOTALogger>();
                    }
                }
                return instance;
            }
        }
    }
}
