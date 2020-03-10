using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hybriona.UnityRemoteLog
{
    public class Globals : MonoBehaviour
    {
        public RemoteLogView remoteLogView;
        public Text errorText;

        [Header("## CONFIG ##")]
        public Color textColorNormal;
        public Color textColorWarning;
        public Color textColorError;

        private Queue<System.Action> mainThreadQueue = new Queue<System.Action>();

        private static Globals instance;
        public static Globals Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<Globals>();
                }
                return instance;
            }
        }

        public void Cache()
        {

        }

        public void EnqueueToMainThread(System.Action action)
        {
            mainThreadQueue.Enqueue(action);
        }

        private void Update()
        {
            while(mainThreadQueue.Count > 0)
            {
                mainThreadQueue.Dequeue()();
            }
        }
    }
}
