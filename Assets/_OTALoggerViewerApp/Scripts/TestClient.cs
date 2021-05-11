using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hybriona.Logging
{
    public class TestClient : MonoBehaviour
    {
        private IEnumerator Start()
        {
            HybOTALogger.Init(idleCheckWait: 10);
            yield return new WaitForSeconds(5);

            while (true)
            {
                int num = Random.Range(0, 100) % 3;

                if (num == 0)
                {
                    HybOTALogger.Log("hello at " + Time.time);
                }
                else if (num == 1)
                {
                    HybOTALogger.LogWarning("hello at " + Time.time + "\nsome warning extra text");
                }
                else if (num == 2)
                {
                    HybOTALogger.LogError("hello at " + Time.time + "\nsome more extra text with error\nEven more!");
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}
