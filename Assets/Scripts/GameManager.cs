using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public static bool GamePaused = false;
    private Scene currentScene;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
        currentScene = scene;
        UIManager.instance.GetUIObjects();
        UIManager.instance.SetHPBar();
    }

    public void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
        currentScene = SceneManager.GetActiveScene();
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        currentScene = SceneManager.GetActiveScene();
    }

    public void Restart() {
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
