using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public float score;
    public void DeathScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DeathMenu");
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "DeathMenu")
            GameObject.Find("SceneManager").GetComponent<DeathSceneManager>().SetScore(score);
    }
}
