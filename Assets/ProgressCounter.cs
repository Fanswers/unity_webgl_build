using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressCounter : MonoBehaviour
{
    private TextMeshProUGUI counter;
    void Start()
    {
        counter = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        counter.text = $"{GameManager.instance.GetTotalTargetCount() - GameManager.instance.GetRemainingTargetCount()}/{GameManager.instance.GetTotalTargetCount()}";
    }
}
