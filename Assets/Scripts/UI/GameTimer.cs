using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TimeFormat format;

    void Update()
    {
        if(GameManager.instance.gameDurationTimer != null)
        {
            if(!GameManager.instance.gameDurationTimer.IsFinished())
                timerText.text = GetTimeString(GameManager.instance.gameDurationTimer.endTime - GameManager.instance.gameDurationTimer.Time, format);
            else
                gameObject.SetActive(false);
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
