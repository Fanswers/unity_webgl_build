using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Shop : MonoBehaviour
{
    public InputActionReference openShop;
    public InputActionReference closeShop;
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
            inputActionAsset.FindActionMap("Gameplay").Enable();
            inputActionAsset.FindActionMap("UI").Disable();
        }
    }

    void Start()
    {
        //inputActionAsset.FindActionMap(mapId).Enable();
        openShop.action.started += ButtonPress;
        closeShop.action.started += ButtonPress;

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

    private bool shopOpen = false;

    void ButtonPress(InputAction.CallbackContext context)
    {
        if (!isInRange) return;
        if (!shopOpen)
        {
            shopOpen = true;
            inputActionAsset.FindActionMap("Gameplay").Disable();
            inputActionAsset.FindActionMap("UI").Enable();
            GameManager.instance.SetPauseGame(true);
            ViewManager.instance.SwapToView(ViewManager.instance.shopView);


        }
        else
        {
            GameManager.instance.SetPauseGame(false);
            ViewManager.instance.Return();
            inputActionAsset.FindActionMap("Gameplay").Enable();
            inputActionAsset.FindActionMap("UI").Disable();
            shopOpen = false;
        }
    }
}
