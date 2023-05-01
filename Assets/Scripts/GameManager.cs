using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module.Singleton.Controller;

public class GameManager : Singleton<GameManager>
{
    public int gameDuration = 300;
    public int totalTrips = 5;
    public List<Target> targets = new List<Target>();
    public Timer gameDurationTimer;
    public bool timerIsMax = false;

    private int gold = 0;

    private SpawnManager spawnManager;
    private bool gameStart = false;

    public int Gold 
    { 
        get => gold;
        set
        {
            if (value < 0)
                gold = 0;
            else
                gold = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Bootstraper.instance.gameLoaded += StartGame;
        spawnManager = FindObjectOfType<SpawnManager>();
        spawnManager.activeSpawnPointPerType[SpawnType.Target] = totalTrips;
    }

    private void Start()
    {
        //StartGame();
    }

    void Update()
    {
    }

    public void StartGame()
    {
        spawnManager.ActivateObjects();

        gameDurationTimer = new Timer(gameDuration, GameOver);
        gameDurationTimer.ResetPlay();

        foreach(SpawnPoint point in spawnManager.points[SpawnType.Target])
        {
            Target t = point.spawnedObject[0].GetComponent<Target>();
            
            if(t != null)
                targets.Add(t);
        }
        
        PickNewTarget();
    }

    public void GameOver()
    {
        FindObjectOfType<GameOver>().GameLost(false);
    }

    public void GameWin()
    {
        FindObjectOfType<GameOver>().GameWon(gameDurationTimer.GetTimeLeft());
    }

    public void TargetReached(Target t)
    {
        PickNewTarget();
    }

    private void PickNewTarget()
    {
        foreach(Target t in targets)
        {
            if(!t.IsReached())
            {
                new Timer(0.1f, t.SetActive).ResetPlay();
                return;
            }
        }

        GameWin();
    }

    public int GetRemainingTargetCount()
    {
        int a = 0;
        foreach(var t in targets)
        {
            if (!t.IsReached()) a++;
        }
        return a;
    }

    public int GetTotalTargetCount() => targets.Count;

    private IEnumerator WaitForGeneration()
    {
        while(!spawnManager.GameReady())
        {
            yield return new WaitForEndOfFrame();
        }
        StartGame();
    }
}
