using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Hybriona.UnityRemoteLog
{
	public class Packet 
	{
        public System.Action<string> onCompletePacketReceived;
        List<byte> buffer = new List<byte>();
        private int headerPos = 0;
        private int readPos = 0;
        private int contentLength = 0;
        public void Reset()
		{
            headerPos = 0;
            readPos = 0;
            contentLength = 0;
            buffer.Clear();

        }

        public void Write(byte [] bufferReference,int size,int offset = 0)
        {           
            int i = 0;
            for(i=offset; i<size; i++)
            {
                //UnityEngine.Debug.Log(System.Convert.ToChar( bufferReference[i] ).ToString());
                if (headerPos == 0)
                {
                    contentLength = bufferReference[i] * 256;
                    headerPos++;
                }
                else if (headerPos == 1)
                {
                    contentLength += bufferReference[i];
                    headerPos++;
                }
                else if(readPos < contentLength)
                {
                    buffer.Add(bufferReference[i]);
                    readPos++;

                    if(readPos == contentLength)
                    {
                        //Debug.Log(Encoding.ASCII.GetString(buffer.ToArray()));
                        onCompletePacketReceived(Encoding.ASCII.GetString(buffer.ToArray()));
                        Reset();
                    }
                }
                else if(i < size)
                {
                    //read header again
                    Write(bufferReference, size, i);
                }
            }
             
        }
	}


}
