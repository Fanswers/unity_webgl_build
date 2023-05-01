using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module.Singleton.Controller;

public class GameManager : Singleton<GameManager>
{
    public int gameDuration = 300;
    public int totalTrips = 5;
    public List<Target> targets = new List<Target>();
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

    public float Timer { get => timer; set => timer = value; }

    protected override void Awake()
    {
        base.Awake();
        Bootstraper.instance.gameLoaded += StartGame;
        Bootstraper.instance.gameLoaded += () => gameStart = true;
        Bootstraper.instance.gameUnloaded += () => gameStart = false;
        spawnManager = FindObjectOfType<SpawnManager>();
        spawnManager.activeSpawnPointPerType[SpawnType.Target] = totalTrips;
    }

    public void SetPauseGame(bool pause = true)
    {
        
    }

    private void Start()
    {
        //StartGame();
    }

    float timer = 0f;
    bool pause = false;
    void Update()
    {
        if (gameStart && !pause)
        {
            timer += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        spawnManager.ActivateObjects();


        foreach(SpawnPoint point in spawnManager.points[SpawnType.Target])
        {
            Target t = point.spawnedObject[0].GetComponent<Target>();
            
            if(t != null)
                targets.Add(t);
        }
        
        PickNewTarget();
    }

   

    public void GameWin()
    {
        if (!gameStart) return;
        ViewManager.instance.SwapToView(ViewManager.instance.gameOverView);
        FindObjectOfType<GameOver>().GameWon(Timer);
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
