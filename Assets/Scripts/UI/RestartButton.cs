using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
