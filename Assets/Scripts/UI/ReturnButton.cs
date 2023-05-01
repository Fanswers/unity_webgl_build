using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButton : MonoBehaviour
{
    public void Return()
    {
        ViewManager.instance.Return();
        GameStateManager.instance.Play();
    }
}
