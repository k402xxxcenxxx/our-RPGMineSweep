using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : MonoBehaviour {

    public void loadStageScene(string sceneName)
    {
        try { 
        Protocol.Instance.PrepareGameSetting(sceneName);
        SceneManager.LoadScene(sceneName);
        }catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void show() {
        Debug.Log("test");
    }
}
