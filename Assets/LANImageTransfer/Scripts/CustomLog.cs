using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLog : MonoBehaviour
{
    public Text displayText;

    static string logText;
    static bool updateText;

    public static void Log(string log)
    {
        logText += log + '\n';
        updateText = true;
    }

    void Update()
    {
        if (updateText)
        {
            displayText.text = logText;
            updateText = false;
        }
    }

}
