using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;

public class HealthBar : MonoBehaviour
{
    private ProgressBar progressBar;

    private bool gameLoaded = false;

    private void Start()
    {
        progressBar = GetComponent<ProgressBar>();
        Bootstraper.instance.gameLoaded += () => gameLoaded = true;
        Bootstraper.instance.gameUnloaded += () => gameLoaded = false;
    }

    private void Update()
    {
        if (!gameLoaded) return;
        var playerShip = FindObjectOfType<ShipController>().GetComponent<Ship>();
        progressBar.ChangeValue(playerShip.Health);
        progressBar.maxValue = playerShip.maxHealth;
    }
}
