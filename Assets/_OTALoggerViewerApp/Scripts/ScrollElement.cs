using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hybriona.Logging
{
    public class ScrollElement : MonoBehaviour
    {
        public TMP_InputField inputField;
        public TMP_Text text;
        public void Assign(string date,Color color, string message)
        {
            text.color = color;
            inputField.text = string.Format("[{0}] {1}", date, message);
        }
    }
}
