using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    public void OnClickStart()
    {
        Bootstraper.instance.startGame = true;
    }

    public void OnClickLeave()
    {
        Application.Quit();
    }
}
