using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMessage : MonoBehaviour {

    public GameObject textObj;
    Text messageText;
    Animator textAnimation;
    string currentText;
    static List<string> logQueue = new List<string>(); // the log queue
    float timer;
    float logTime = 0.5f;
    static bool logged = false;
    private void Start()
    {
        textAnimation = textObj.GetComponent<Animator>();
        messageText = textObj.GetComponent<Text>();
    }
    private void Update()
    {
        if (logged)
        {
            timer = logTime;
            logged = false;
            currentText = logQueue[0];
            logQueue.RemoveAt(0);
            textAnimation.Play("messageAnimation", -1, 0f);
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            messageText.text = currentText;
        }
        else
        {
            if (logQueue.Count != 0)
            {

                logged = true;
            }
            else
                messageText.text = "";
        }
    }
    static public void Log(string message)
    {
        logQueue.Add(message);
        logged = true;
    }


}
