using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MoneyCounter : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = $"{GameManager.instance.Gold} $";
    }

}
