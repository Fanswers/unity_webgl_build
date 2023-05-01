using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstraper : Module.Singleton.Controller.Singleton<Bootstraper>
{
    [System.Serializable]
    public class SceneGroup
    {
        public string[] alwaysLoaded;
        public string[] start;
        public string[] gameStart;
    }

    public Transform    Canvas;

    public event Action initalLoadingComplete;
    public event Action menuLoaded;
    public event Action gameLoaded;
    public event Action gameUnloaded;

    [SerializeField] private SceneGroup scenesToLoad;

    private void Start()
    {
        StartCoroutine(LoadAlwaysLoadedScenes());
    }

    private IEnumerator LoadAlwaysLoadedScenes()
    {
        Canvas.gameObject.SetActive(true);
        for (int i = 0; i < scenesToLoad.alwaysLoaded.Length; i++)
        {
            yield return SceneManager.LoadSceneAsync(scenesToLoad.alwaysLoaded[i], LoadSceneMode.Additive);
        }
        initalLoadingComplete?.Invoke();
        // When alwaysLoaded scenes are loaded, proceed with the game flow
        StartCoroutine(LoadStartScenes());
    }

    private IEnumerator LoadStartScenes()
    {
        for (int i = 0; i < scenesToLoad.start.Length; i++)
        {
            yield return SceneManager.LoadSceneAsync(scenesToLoad.start[i], LoadSceneMode.Additive);
        }
        menuLoaded?.Invoke();
        Canvas.gameObject.SetActive(false);

        // Wait for user input or other conditions before starting the game
        // You can use events or other communication methods to detect when to start the game
        // For now, we'll just wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Unload start scenes
        Canvas.gameObject.SetActive(true);
        for (int i = 0; i < scenesToLoad.start.Length; i++)
        {
            yield return SceneManager.UnloadSceneAsync(scenesToLoad.start[i]);
        }

        // Load game start scenes
        StartCoroutine(LoadGameStartScenes());
    }

    private IEnumerator LoadGameStartScenes()
    {
        for (int i = 0; i < scenesToLoad.gameStart.Length; i++)
        {
            yield return SceneManager.LoadSceneAsync(scenesToLoad.gameStart[i], LoadSceneMode.Additive);
        }
        gameLoaded?.Invoke();
        Canvas.gameObject.SetActive(false);
    }

    private IEnumerator UnloadGameStartScenes()
    {
        for (int i = 0; i < scenesToLoad.gameStart.Length; i++)
        {
            yield return SceneManager.UnloadSceneAsync(scenesToLoad.gameStart[i]);
        }
        gameUnloaded?.Invoke();
    }
}
