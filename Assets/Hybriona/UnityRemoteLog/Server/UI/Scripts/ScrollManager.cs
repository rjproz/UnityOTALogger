using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hybriona.UnityRemoteLog
{
    public class ScrollManager : MonoBehaviour
    {
        public ScrollElement elementPrefab;
        public ScrollRect scrollRect;
        public RectTransform scrollContentParent;

        public bool isAutoScrollEnabled { get; set; }
        private Queue<LogData> logsPending = new Queue<LogData>();

        private float scrollPoint = 0;
        private float targetVerticalScroll;
       

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

                    bool shouldLerpScroll = false;
                    if (scrollRect.verticalNormalizedPosition <= 0.01f)
                    {
                        shouldLerpScroll = true;
                    }

                    yield return StartCoroutine(o.Assign(logData));
                    o.rectTransform.anchoredPosition = new Vector2(0, scrollPoint);
                    scrollPoint = scrollPoint - o.elementHeight - 5;

                    Vector2 size = scrollContentParent.sizeDelta;
                    size.y = Mathf.Abs(scrollPoint);
                    scrollContentParent.sizeDelta = size;

                    if(shouldLerpScroll)
                    {
                        targetVerticalScroll = 0;
                    }
                }
            }
        }

        private IEnumerator AutoScroll()
        {
            while (true)
            {
                yield return null;
                if (isAutoScrollEnabled)
                {
                    scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, targetVerticalScroll, Time.deltaTime * 20f);

                }
            }
        }

        private void Start()
        {
            Globals.Instance.Cache();
            elementPrefab.Deactivate();
            StartCoroutine(Loop());
            StartCoroutine(AutoScroll());
        }
    }
}
