using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;

public class HealthBar : MonoBehaviour
{
    private ProgressBar progressBar;

    private void Start()
    {
        progressBar = GetComponent<ProgressBar>();
    }

    private void Update()
    {
        var playerShip = FindObjectOfType<ShipController>().GetComponent<Ship>();
        progressBar.ChangeValue((playerShip.Health / playerShip.maxHealth) * 100);
    }
}
