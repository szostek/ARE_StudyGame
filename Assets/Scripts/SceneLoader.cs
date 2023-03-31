using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int sceneIndexToLoad;
    public bool showCityScene;

    private void Awake() 
    {
        if (showCityScene) {
            SceneManager.LoadScene(sceneIndexToLoad, LoadSceneMode.Additive);    
        }
    }
}
