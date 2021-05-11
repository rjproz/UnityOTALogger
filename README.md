# hybUOTALogger: Logging via RUDP (Wireless)

The server is being build using [LiteNetLib](https://github.com/RevenantX/LiteNetLib). 

## Features
* Unity Library can be implemented in Standalone, Android and iOS platforms. 
* The Viewer (Windows & Mac OSX) can connect to a Unity build via RUDP using ip address and port.



## Unity Sample
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hybriona.Logging
{
    public class TestClient : MonoBehaviour
    {
        private IEnumerator Start()
        {
            HybOTALogger.Init(idleCheckWait:120);
            yield return new WaitForSeconds(2);

            while(true)
            {
                int num = Random.Range(0, 100) % 3;

                if (num == 0)
                {
                    HybOTALogger.Log("hello at " + Time.time);
                }
                else if(num == 1)
                {
                    HybOTALogger.LogWarning("hello at " + Time.time+"\nsome warning extra text");
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

```