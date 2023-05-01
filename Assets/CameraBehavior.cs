using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private void Start()
    {
        //Bootstraper.instance.gameLoaded += FindPlayerTransform();
        FindPlayerTransform();
    }


    private void Update()
    {
        FindPlayerTransform();
    }
    private void FindPlayerTransform()
    {
        GetComponent<Cinemachine.CinemachineBrain>().m_WorldUpOverride = TargetLibrary.Instance.PlayerTransform;
    }
}
