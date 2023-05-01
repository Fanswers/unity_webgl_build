using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public SpawnType type;
    [HideInInspector]
    public List<GameObject> spawnedObject;
    public float spawnRange = 0;
    private SpawnManager manager;
    private bool active = false;

    void Start()
    {
        manager = FindObjectOfType<SpawnManager>();
        manager.RegisterSpawnPoint(this);
        this.gameObject.name = "SpawnPoint - " + type.ToString();
        spawnedObject = new List<GameObject>();
    }

    public void Activate()
    {
        active = true;
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        try
        {
            for(int i = 0; i < manager.numberToSpawnPerSpawnPoint[type][Random.Range(0, manager.numberToSpawnPerSpawnPoint[type].Count)]; i++)
            {
                GameObject obj = Instantiate(manager.prefabPerType[type][Random.Range(0, manager.prefabPerType[type].Count)]);
                Vector3 spawnOffset = new Vector3(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f)
                ).normalized * Random.Range(spawnRange/2, spawnRange);
                
                obj.transform.position = transform.position + spawnOffset;
                obj.transform.rotation = transform.rotation;

                spawnedObject.Add(obj);
            }
        }
        catch
        {
            Debug.LogError("Prefabs missing in the manager : " + type.ToString());
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color color = Color.white;
        switch (type)
        {
            case SpawnType.Character:
                color = Color.red;
                break;
            case SpawnType.Enemy:
                color = Color.blue;
                break;
            case SpawnType.Shop:
                color = Color.yellow;
                break;
            case SpawnType.Chest:
                color = Color.green;
                break;
            case SpawnType.Target:
                color = Color.magenta;
                break;
        }
        Gizmos.color = color;
        float size = spawnRange > 0 ? spawnRange : 1;
        Gizmos.DrawSphere(transform.position, size);
        Gizmos.DrawWireSphere(transform.position, 30f);
    }
#endif

    public bool IsActive()
    {
        return active;
    }

    void OnDrawGizmosSelected()
    {
        switch(type)
        {
            case SpawnType.Character :
            Gizmos.color = Color.red;
            break;

            case SpawnType.Enemy :
            Gizmos.color = Color.cyan;
            break;

            case SpawnType.Shop :
            Gizmos.color = Color.magenta;
            break;

            case SpawnType.Chest :
            Gizmos.color = Color.yellow;
            break;

            case SpawnType.Target :
            Gizmos.color = Color.green;
            break;

            default:
            break;
        }
        Gizmos.DrawSphere(transform.position, Mathf.Max(spawnRange, 0.5f));
    }
}
