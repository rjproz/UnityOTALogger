using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace Hybriona.UnityRemoteLog
{
    public class TestClient : MonoBehaviour
    {
        public string ip;
        public int port;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(2);

            UnityRemoteLogger.Instance.StartLogging("com.hybriona.test", ip, port);
            yield return new WaitForSeconds(5);

            UnityRemoteLogger.Log("Start uB4FXSHQeWtlA8kOlYz9yGScVneP1dCqCJb6SWfRBqyOs5SMup86QiPBi6DVdMdJvaLHYzT95RLMsrAoqWQnITtiBysoS4vswaUEZ66wOInJcYpXODOiavzdYLlI2wl4cWEYsYtDk1FLI8GHc8cxzJ5Iq9dPJ2cgjEbW8Jywh9224vepObiCx1qbgTVeldF39z4Q4utgntJVJmtZuexyWzLROOzaYpPDqjjTv1BEBskkinKQDKWADnXHQBOTK1qlRd6lA2ctiGWuLNJ7dZD7N5HqfjzIiozfvctBjdwuf071QoeURAwgzYF07AioEytvKgbDbEAtkkDQRxQXq0gHQg6p2oH33ymFPyalgcwwst5TYICc6iohTgf90sfnPvQ4gMSQR5gddfdrfkMFA29DzpRdrYYrWm8QsS56ujsjWIVyGLzidxkAaMLUftRrpS1hkpRqxw7yyQlNCPDSs3SINLI89rkqCAAomhPXj3mk3jGfCRH81ab8 End");
            while(true)
            {
                yield return new WaitForSeconds(5);

                UnityRemoteLogger.LogError("Current time is "+Time.time);
            }
        }


      
    }
}
