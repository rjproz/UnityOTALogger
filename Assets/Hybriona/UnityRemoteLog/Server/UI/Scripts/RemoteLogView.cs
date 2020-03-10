using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hybriona.UnityRemoteLog
{
    public class RemoteLogView : MonoBehaviour
    {
        public TextFormat autoScrollModeLblText;
        public ScrollManager scrollManager;

        public bool isAutoScrollEnabled;

        public void OnClickedToggleAutoScroll()
        {
            isAutoScrollEnabled = !isAutoScrollEnabled;
            scrollManager.isAutoScrollEnabled = isAutoScrollEnabled;
            if (isAutoScrollEnabled)
            {
                autoScrollModeLblText.Set("On");
            }
            else
            {
                autoScrollModeLblText.Set("Off");
            }
        }

        private void Awake()
        {
            isAutoScrollEnabled = true;
            scrollManager.isAutoScrollEnabled = isAutoScrollEnabled;
        }
    }
}