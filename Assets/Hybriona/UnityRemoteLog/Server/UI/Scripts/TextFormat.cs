using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hybriona.UnityRemoteLog
{
    public class TextFormat : MonoBehaviour
    {
        public string format = "";
        public Text text;

        public void Set(params object [] parameters)
        {
            text.text = string.Format(format, parameters);
        }
        public void ClearText()
        {
            text.text = "";
        }
    }
}
