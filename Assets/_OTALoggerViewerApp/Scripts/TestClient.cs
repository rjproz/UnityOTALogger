using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hybriona.Logging
{
    public class TestClient : MonoBehaviour
    {
        private IEnumerator Start()
        {
            HybOTALogger.Init();
            yield return new WaitForSeconds(5);

            while(true)
            {
                int num = Random.Range(0, 100) % 3;
                LogType logType = (LogType) ( Random.Range(0, 100) % 3);
                if (num == 0)
                {
                    HybOTALogger.Log("hello at " + Time.time, logType);
                }
                else if(num == 1)
                {
                    HybOTALogger.Log("hello at " + Time.time+"\nsome more extra text", logType);
                }
                else if (num == 2)
                {
                    HybOTALogger.Log("hello at " + Time.time + "\nsome more extra text\nEven more!", logType);
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}
