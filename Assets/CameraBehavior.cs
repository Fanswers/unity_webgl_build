using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Transform worldUpOverride;

    private bool gameLoaded = false;

    private void Start()
    {
        Bootstraper.instance.gameLoaded += () => gameLoaded = true;
        Bootstraper.instance.gameUnloaded += () => gameLoaded = false;
    }


    private void Update()
    {
        if (!gameLoaded) return;
        FindPlayerTransform();
    }
    private void FindPlayerTransform()
    {
        GetComponent<Cinemachine.CinemachineBrain>().m_WorldUpOverride = FindObjectOfType<ShipController>().transform;
    }
}
