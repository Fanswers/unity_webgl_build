using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EngineAudio : MonoBehaviour
{
    public AudioSource startSource;
    public AudioClip audioClipStart;
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
        if (inputAction.ReadValue<float>() > 0.5)
        {
            if (!flag && startSource.isPlaying == false)
            {
                flag = true;
                startSource.PlayOneShot(audioClipStart);
            }
        }
        else if (flag == true)
        {
            flag = false;
        }
    }

    private void MotorStart(InputAction.CallbackContext context)
    {
        startSource.Play();
    }
}
