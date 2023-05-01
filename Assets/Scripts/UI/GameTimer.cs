using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TimeFormat format;

    bool gameLoaded = false;

    private void Awake()
    {
        Bootstraper.instance.gameLoaded += () => gameLoaded = true;
        Bootstraper.instance.gameUnloaded += () => gameLoaded = false;
    }

    void Update()
    {
        if(gameLoaded)
        {
            timerText.text = GetTimeString(GameManager.instance.Timer, format);
        }
    }

    public static string GetTimeString(float timeInSecond, TimeFormat format)
    {
        int hour= (int) Mathf.Floor(timeInSecond / 3600);
        int min = (int) Mathf.Floor((timeInSecond - 3600 * hour) / 60);
        int sec = (int) Mathf.Floor(timeInSecond - (3600 * hour + 60 * min));
        int ms  = Mathf.RoundToInt((timeInSecond - (3600 * hour + 60 * min + sec)) * 100);

        switch(format)
        {
            case TimeFormat.HourMinuteSecond:
            return "" + hour + ":" + min.ToString("00") + ":" + sec.ToString("00");

            case TimeFormat.HourMinuteSecondCent:
            return "" + hour + ":" + min.ToString("00") + ":" + sec.ToString("00") + ":" + ms.ToString("00");

            case TimeFormat.MinuteSecond:
            return ""              + min.ToString("00") + ":" + sec.ToString("00");

            case TimeFormat.MinuteSecondCent:
            return ""              + min.ToString("00") + ":" + sec.ToString("00") + ":" + ms.ToString("00");

            default : 
            return "";
        }        
    }
}

public enum TimeFormat
{
    HourMinuteSecond,
    HourMinuteSecondCent,
    MinuteSecond,
    MinuteSecondCent,
}
