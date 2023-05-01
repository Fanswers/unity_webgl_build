using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

public class Minimap : MonoBehaviour
{

    public DisplayedItemOptions[] displayedItemOptions;
    // Update is called once per frame
    private Vector3[] enemies;
    private List<SpawnPoint> shops;
    private Target target;
    private List<Chest> chests;
    private Chest objectives;
    private Material[] pointsMaterials;
    private GameObject[][] itemsPointInstance ;
    public int itemsTypeAmount = 5;
    public float sphereSize;
    public float maxDisplayDistance = 1000;
    public Mesh indicatorMesh;
    private Transform playerTransform;
    public GameObject sphereContainer;
    private SpawnManager spawnerManager;

    private void Awake()
    {
        Bootstraper.instance.gameLoaded += () => gameLoaded = true;
        Bootstraper.instance.gameUnloaded += () => gameLoaded = false;
    }

    private void Start() {
        spawnerManager = FindObjectOfType<SpawnManager>();
        itemsPointInstance = new GameObject[itemsTypeAmount][];
        pointsMaterials = new Material[itemsTypeAmount];
        for (int i = 0; i < itemsTypeAmount; i++) {
            pointsMaterials[i] = new Material(Shader.Find("Shader Graphs/custom_item_minimap"));
            pointsMaterials[i].SetColor("_Color", displayedItemOptions[i].color);
            itemsPointInstance[i] = new GameObject[displayedItemOptions[i].maxItemsDisplayed];
            for (var j = 0; j < displayedItemOptions[i].maxItemsDisplayed; j++) {
                itemsPointInstance[i][j] = CreatePointInstance(displayedItemOptions[i], pointsMaterials[i]);
            }
        }
        itemsPointInstance[0][0].transform.localPosition = Vector3.zero;
    }

    private GameObject CreatePointInstance(DisplayedItemOptions displayedItemOption, Material material) {
        GameObject sphere = new GameObject("Sphere");
        SphereCollider collider = sphere.AddComponent<SphereCollider>();
        MeshFilter meshFilter = sphere.AddComponent<MeshFilter>();
        meshFilter.mesh = indicatorMesh;
        MeshRenderer renderer = sphere.AddComponent<MeshRenderer>();
        sphere.transform.parent = this.transform;
        sphere.transform.localScale = Vector3.one * displayedItemOption.size ;
        sphere.transform.localPosition = Vector3.one * 1000;
        sphere.layer = 7;

        collider.radius = 1 ;
        renderer.material = material;

        sphere.transform.position = Vector3.up * 1000;

        return sphere;
    }

    private bool gameLoaded = false;

    void Update() {
        if (!gameLoaded) return;
        FetchAllPos();
        UpdateMapPos();
    }

    private void FetchAllPos()
    {
        playerTransform = TargetLibrary.Instance.PlayerTransform;
        var query = from enemy in TargetLibrary.Instance.enemies
                    where Vector3.Distance(enemy.transform.position, playerTransform.transform.position) < maxDisplayDistance
        select enemy.transform.position;
        enemies = query.ToArray();
        shops = spawnerManager.points[SpawnType.Shop];
        chests = Chest.instances;
        target = Target.activeTarget;
    }

    private void UpdateMapPos() {
        // Handle enemies display
        // Handle chest display
        // int chests
        int index = 0;
        foreach (Vector3 enemyPos in enemies)
        {
            if (index < itemsPointInstance[1].Length && Vector3.Distance(enemyPos, playerTransform.position) < maxDisplayDistance)
            {
                itemsPointInstance[1][index].transform.localPosition = ConvertPosition(enemyPos);
                index++;
            }
        }
        for (int i = index; i < itemsPointInstance[1].Length; i++)
        {
            itemsPointInstance[1][i].transform.localPosition = Vector3.one * 1000;
        }

        if (target) {
            itemsPointInstance[2][0].transform.localPosition = ConvertPosition(target.transform.position);
        }

        // Handle chest display
        // int chests
        index = 0;
        foreach (Chest chest in chests) {
            
            if (index < itemsPointInstance[3].Length && Vector3.Distance(chest.transform.position, playerTransform.position) < maxDisplayDistance ) {
                itemsPointInstance[3][index].transform.localPosition = ConvertPosition(chest.transform.position);
                index++;
            }
        }
        for (int i = index; i < itemsPointInstance[3].Length; i++) {
            itemsPointInstance[3][i].transform.localPosition = Vector3.one * 1000;
        }


        // Handle shops
        index = 0;
        foreach (SpawnPoint shopSpawnPoint in shops)
        {
            if (index < itemsPointInstance[4].Length && Vector3.Distance(shopSpawnPoint.transform.position, playerTransform.position) < maxDisplayDistance)
            {
                itemsPointInstance[4][index].transform.localPosition = ConvertPosition(shopSpawnPoint.transform.position);
                index++;
            }
        }
        for (int i = index; i < itemsPointInstance[4].Length; i++)
        {
            itemsPointInstance[4][i].transform.localPosition = Vector3.one * 1000;
        }

        this.transform.rotation = Quaternion.Inverse(Camera.main.transform.rotation);
    }

    private Vector3 ConvertPosition(Vector3 positionToConvert) {
        Vector3 differencePos = positionToConvert - playerTransform.position;
        float distanceFromSphereCenter = Mathf.InverseLerp(0, maxDisplayDistance, differencePos.magnitude);
        // Debug.Log(distanceFromSphereCenter);
        return differencePos.normalized * (distanceFromSphereCenter * sphereSize);
    }
}

[Serializable]
public struct DisplayedItemOptions
{
    public Color color;
    [Range(.01f,1f)] public float size;
    public int maxItemsDisplayed;
}
