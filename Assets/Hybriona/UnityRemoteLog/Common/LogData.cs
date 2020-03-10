using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hybriona.UnityRemoteLog
{
    [System.Serializable]
    public struct LogData 
    {
        public string appId;
        public Type type;
        public string message;
        public long timestamp;
       
        public enum Type { Info = 0 , Warning = 1,Error = 2 };
    }
}
