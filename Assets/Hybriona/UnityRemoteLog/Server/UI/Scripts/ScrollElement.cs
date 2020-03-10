using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hybriona.UnityRemoteLog
{
    public class ScrollElement : MonoBehaviour
    {
        public TextFormat titleTextTarget;
        public Text logTextTarget;
        public RectTransform rectTransform;
        public LogData logData { get; private set; }
        public float elementHeight { get; private set; }


        public IEnumerator Assign(LogData _logData)
        {
            logData = _logData;
            titleTextTarget.Set(System.DateTime.FromFileTime(logData.timestamp).ToString("MM/dd/yyyy HH:mm:ss"), logData.appId);
            logTextTarget.text = logData.message;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            yield return null;
            elementHeight = rectTransform.rect.height;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public ScrollElement Duplicate()
        {
            GameObject o = Instantiate(gameObject);
            o.transform.SetParent(transform.parent);
            o.transform.localScale = Vector3.one;
            return o.GetComponent<ScrollElement>();
        }

    }
}
