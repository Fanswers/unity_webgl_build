using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetLibrary : MonoBehaviour
{
    private static TargetLibrary instance;
    public List<Enemy> enemies = new List<Enemy>();

    public static TargetLibrary Instance 
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("TargetLibrary");
                instance = go.AddComponent<TargetLibrary>();
                return instance;
            }
            return instance;
        }
        set => instance = value; 
    }

    public static KinematicTarget Player
    {
        get
        {
            if (!isPlayerSet)
            {
                player = Instance.FindPlayer();
                isPlayerSet = true;
                return player;
            }
            else
            {
                return player;
            }
        }
    }

    public Transform PlayerTransform { get => playerTransform; }

    private static KinematicTarget player;
    private static bool isPlayerSet = false;

    private void OnDestroy()
    {
        isPlayerSet = false;
    }


    private KinematicTarget FindPlayer()
    {
        var playerRb = FindObjectOfType<ShipMovement>().gameObject.GetComponent<Rigidbody>();
        playerTransform = playerRb.transform;
        return new KinematicTarget(playerRb);
    }
    private Transform playerTransform; 


    public static void Register(Enemy enemy, out int id)
    {
        Instance.enemies.Add(enemy);
        id = Instance.enemies.Count - 1;
    }

    public static void Remove(Enemy enemy)
    {
        if (instance == null) return;
        Instance.enemies.Remove(enemy);
    }

    public static Enemy[] EveryoneButMe(Enemy enemy)
    {
        var query = from e in Instance.enemies
                    where e != enemy
                    select e;
        return query.ToArray();
    }

    public static bool IsAnyEnemyActive() => instance.enemies.Any(e => e.chaseActive);

    public static KinematicTarget[] EveryoneButMeTarget(KinematicTarget me)
    {
        var query = from e in Instance.enemies
                    where new KinematicTarget(e.GetComponent<Rigidbody>()) != me
                    select new KinematicTarget(e.GetComponent<Rigidbody>());
        return query.ToArray();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
