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
    private Vector3[] enemies, objectives;
    private Material[] pointsMaterials;
    private GameObject[][] itemsPointInstance ;
    public int itemsTypeAmount = 3;
    public float sphereSize;
    public float maxDisplayDistance = 1000;
    public Mesh indicatorMesh;
    private Transform playerTransform;
    public GameObject sphereContainer;
    private void Start() {
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

    void Update() {
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
        objectives = new Vector3[] { 
            new Vector3(1000,1000,1000),
            new Vector3(1000,1000,1000) * -1,
            new Vector3(10000,10000,-10000) * -1,
        };
    }

    private void UpdateMapPos() {
        // Handle enemies display
        for (int i = 0; i < itemsPointInstance[1].Length; i++) {
            if (i < enemies.Length) {
                itemsPointInstance[1][i].transform.localPosition = ConvertPosition(enemies[i]);
            }
            else {
                itemsPointInstance[1][i].transform.localPosition = Vector3.one * 1000;
            }
        }

        // Handle objectives display
        for (int i = 0; i < itemsPointInstance[2].Length; i++)
        {
            if (i < objectives.Length)
            {
                itemsPointInstance[2][i].transform.localPosition = ConvertPosition(objectives[i]);
            }
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
