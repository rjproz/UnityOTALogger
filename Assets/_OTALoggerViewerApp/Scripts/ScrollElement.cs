using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hybriona.Logging
{
    public class ScrollElement : MonoBehaviour
    {
        public TMP_InputField inputField;
        public void Assign(string date,string color, string message)
        {
            inputField.text = string.Format("<color=#6096FF>[{0}]</color> <color={1}>{2}</color>", date, color, message);
        }
    }
}
