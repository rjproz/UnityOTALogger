using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hybriona.UnityRemoteLog
{
    public class ScrollManager : MonoBehaviour
    {
        public ScrollElement elementPrefab;
        public RectTransform scrollContentParent;
        private Queue<LogData> logsPending = new Queue<LogData>();
        private float scrollPoint = 0;
        public void EnqueueLog(LogData logData)
        {
            logsPending.Enqueue(logData);
        }
        private IEnumerator Loop()
        {
            while(true)
            {
                while(logsPending.Count == 0)
                {
                    yield return null;
                }

                while(logsPending.Count > 0)
                {
                    LogData logData = logsPending.Dequeue();
                    ScrollElement o = elementPrefab.Duplicate();
                    o.Activate();
                    yield return StartCoroutine(o.Assign(logData));
                    o.rectTransform.anchoredPosition = new Vector2(0, scrollPoint);
                    scrollPoint = scrollPoint - o.elementHeight - 5;

                    Vector2 size = scrollContentParent.sizeDelta;
                    size.y = Mathf.Abs(scrollPoint);
                    scrollContentParent.sizeDelta = size;
                }
            }
        }

        private void Start()
        {
            Globals.Instance.Cache();
            elementPrefab.Deactivate();
            StartCoroutine(Loop());
        }
    }
}
