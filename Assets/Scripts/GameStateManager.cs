using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module.Singleton.Controller;

public class GameStateManager : Singleton<GameStateManager>
{
    public View gameView, shopView, winView, loseView;
    private GameState currentGameState = GameState.Menu;

    void Start()
    {

    }

    public void FirstPlay()
    {
        
    }

    public void PerkChosen()
    {
        currentGameState = GameState.Running;
    }

    public void Win()
    {
        Debug.LogWarning("Fin de la partie : Victoire");
    }

    public void Lost()
    {
        Debug.LogWarning("Fin de la partie : Défaite");
    }

    public void Pause()
    {
        currentGameState = GameState.Paused;
    }

    public void Play()
    {
        currentGameState = GameState.Running;
    }
}

enum GameState
{
    Menu,
    Running,
    Paused,
    Won,
    Lost,
}