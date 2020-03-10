using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hybriona.UnityRemoteLog
{
    public class Globals : MonoBehaviour
    {
        public ScrollManager scrollManager;
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
