using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hybriona.UnityRemoteLog
{
    public class ServerInstance : MonoBehaviour
    {
        private Server server;
        public void StartServer()
        {
            server = new Server();
            server.Start(10000);
        }

        public void StopServer()
        {
           if(server != null)
            {
                server.Dispose();
            }
        }

        private void Start()
        {
            StartServer();
        }

        private void OnDisable()
        {
            StopServer();
        }
    }
}
