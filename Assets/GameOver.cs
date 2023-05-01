using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public Transform holder;
    public TextMeshProUGUI title;
    public TextMeshProUGUI body;

    private void Start()
    {
    }

    public void GameLost(bool killed)
    {
        ViewManager.instance.SwapToView(this.GetComponent<View>());
        title.text = "DEFEAT";
        body.text = killed ? "You have been killed by the Corporation." : "Your time has run out.";
    }

    public void GameWon(float timer)
    {
        ViewManager.instance.SwapToView(this.GetComponent<View>());
        title.text = "VICTORY";
        body.text = $"Congratulations you won! " +
            $"Try to beat your personal score of : " +
            $"{GameTimer.GetTimeString(timer, TimeFormat.HourMinuteSecondCent)}";
    }

}
