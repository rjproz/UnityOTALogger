using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Hybriona.Logging
{
    public class UIController : MonoBehaviour
    {
        public InputField ipInputField;
        public InputField portInputField;

        public Text autoScrollBtnLbl;
        public Connecter connecter;
        public GameObject scrollElementPrefab;
        public ScrollRect scrollRect;
       
        public Color logTextColor;
        public Color errorTextColor;
        public Color warningTextColor;

        private bool isAutoScrollOn = true;
        private Queue<System.Action> mainthreadQueue = new Queue<System.Action>();
        private static object thelock = new object();

        public void OnAutoScrollBtnClicked()
        {
            isAutoScrollOn = !isAutoScrollOn;
            autoScrollBtnLbl.text = (isAutoScrollOn ? "Auto Scroll On":"Auto Scroll Off");
        }
        public void OnConnectClicked()
        {
            connecter.Connect(ipInputField.text, int.Parse(portInputField.text));
        }

        public void OnLogReceived(long time,LogType logType, string message)
        {
            lock(thelock)
            {
                mainthreadQueue.Enqueue(() =>
                {
                    GameObject o = Instantiate(scrollElementPrefab);
                    o.transform.SetParent(scrollElementPrefab.transform.transform.parent);
                    o.transform.localScale = Vector3.one;
                    ScrollElement scrollElement = o.GetComponent<ScrollElement>();

                    Color currentTxtColor = Color.white;
                    if(logType == LogType.Log)
                    {
                        currentTxtColor = logTextColor;
                    }
                    else if(logType == LogType.Error)
                    {
                        currentTxtColor = errorTextColor;
                    }
                    else if(logType == LogType.Warning)
                    {
                        currentTxtColor = warningTextColor;
                    }

                    scrollElement.Assign(System.DateTime.FromFileTimeUtc(time).ToString("HH:mm:ss"), currentTxtColor, message);
                   
                    o.SetActive(true);

                    StopAllCoroutines();
                    
                    StartCoroutine(DelayScroll());
                });
            }
        }

        private void Update()
        {
            if (mainthreadQueue.Count > 0)
            {
                lock (thelock)
                {
                    mainthreadQueue.Dequeue()();
                }
                
            }
            
        }

        private void Start()
        {
            scrollElementPrefab.SetActive(false);
            connecter.onLogReceived = OnLogReceived;

            connecter.connected = () =>
            {
                lock (thelock)
                {
                    mainthreadQueue.Enqueue(() =>
                    {
                        PlayerPrefs.SetString("ip", ipInputField.text);
                        PlayerPrefs.SetString("port", portInputField.text);
                        PlayerPrefs.Save();
                    });
                }
            };

            ipInputField.text = PlayerPrefs.GetString("ip","");
            portInputField.text = PlayerPrefs.GetString("port", "");
        }

        IEnumerator DelayScroll()
        {
            yield return null;
            yield return null;
            if(isAutoScrollOn)
            {
                scrollRect.verticalNormalizedPosition = 0;
            }
        }
    }
}
