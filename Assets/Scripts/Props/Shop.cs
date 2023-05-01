using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Shop : MonoBehaviour
{
    public InputActionReference openShop;
    public bool isInRange = false;
    public InputActionAsset inputActionAsset;
    public string mapId;
    private bool buttonDown = false;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<ShipController>() != null)
        {
            isInRange = true;
            ViewManager.instance.SwapToView(ViewManager.instance.shopIconView);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponentInParent<ShipController>() != null)
        {
            isInRange = false;
            ViewManager.instance.Return();
        }
    }

    void Start()
    {
        inputActionAsset.FindActionMap(mapId).Enable();
        openShop.action.started += ButtonPress;

    }

    void Update()
    {
        if(isInRange && buttonDown)
        {
            buttonDown = false;
            GameStateManager.instance.Pause();
            ViewManager.instance.SwapToView(ViewManager.instance.shopView);
        }
    }

    void ButtonPress(InputAction.CallbackContext context)
    {
        if(isInRange)
            buttonDown = true;
    }
}
