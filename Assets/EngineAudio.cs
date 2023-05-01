using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EngineAudio : MonoBehaviour
{
    public AudioSource boostGateSource;
    public AudioClip audioClipBoost;
    private ShipController shipController;
    private InputAction inputAction;

    private void Start()
    {
        shipController = GetComponentInParent<ShipController>();
        inputAction = shipController.thrust.action;
    }

    private bool flag = false;
    private void Update()
    {
    }

    public void BoostGateSound()
    {
        if (boostGateSource.isPlaying == false)
        {
            boostGateSource.PlayOneShot(audioClipBoost);
        }
    }
}
