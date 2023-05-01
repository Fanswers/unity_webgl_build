using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnManager : SerializedMonoBehaviour
{    
    [InfoBox("La liste des points possible pour cette partie"), ReadOnly]
    public Dictionary<SpawnType,List<SpawnPoint>> points;
    [InfoBox("Le nombre d'objet à faire apparaitre par point de spawn")]
    public Dictionary<SpawnType, List<int>> numberToSpawnPerSpawnPoint;
    [InfoBox("Le nombre de point de spawn")]
    public Dictionary<SpawnType, int> activeSpawnPointPerType;
    [InfoBox("Pour chaque type de point de spawn définir le ou les prefabs")]
    public Dictionary<SpawnType, List<GameObject>> prefabPerType;

    private bool gameReady = false;

    void Awake()
    {
        foreach(string spawnTypeName in System.Enum.GetNames(typeof(SpawnType)))
        {
            points.Add((SpawnType)System.Enum.Parse(typeof(SpawnType), spawnTypeName), new List<SpawnPoint>());
        }

        StartCoroutine("WaitForRegister");
    }

    public void RegisterSpawnPoint(SpawnPoint point)
    {
        points[point.type].Add(point);
    }

    IEnumerator WaitForRegister()
    {
        int registeredPointCount = 0;
        do
        {
            foreach(List<SpawnPoint> list in points.Values)
                registeredPointCount += list.Count;

            yield return new WaitForFixedUpdate();
        } while (registeredPointCount != FindObjectsOfType<SpawnPoint>().Length);

        ActivateObjects();
    }

    private void ActivateObjects()
    {
        foreach(SpawnType type in activeSpawnPointPerType.Keys)
        {
            List<SpawnPoint> buffer = points[type];
            int counter = 0;
            try
            {
                counter = activeSpawnPointPerType[type];
                for(int i = activeSpawnPointPerType[type]; i > 0; i--)
                {
                    SpawnPoint point;
                    do
                    {
                        int rand = Random.Range(0, buffer.Count);
                        point = buffer[rand];
                    }
                    while(point.IsActive());
                    point.Activate();
                    counter--;
                }
            }
            catch
            {
                Debug.LogError("Not enought " + type.ToString() + " spawn points in the scene. At least " + counter + " missing");
            }            
        }
        FreeDisabledSpawnpoints();
    }

    private void FreeDisabledSpawnpoints()
    {
        foreach(List<SpawnPoint> spawnPointList in points.Values)
        {
            for(int i = spawnPointList.Count-1; i>= 0; i--)
            {
                if(!spawnPointList[i].IsActive())
                {
                    Destroy(spawnPointList[i].gameObject);
                    spawnPointList.RemoveAt(i);
                }
            }
        }

        gameReady = true;
    }

    public bool GameReady()
    {
        return gameReady;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            foreach(SpawnPoint point in points[SpawnType.Enemy])
            {
                point.SpawnObjects();
            }
        }
    }
}
