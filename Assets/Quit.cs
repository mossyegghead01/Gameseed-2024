using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        GetComponent<AudioSource>().Play();
        Application.Quit();
    }

    public void ToMenu()
    {
        // Load the next scene (assuming it's the next scene in the build index)
        GetComponent<AudioSource>().Play();
        StartCoroutine(WaitForAudioAndLoadScene());


        IEnumerator WaitForAudioAndLoadScene()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            while (audioSource.isPlaying)
            {
                yield return null;
            }

            SceneManager.LoadScene("StartMenu");
        }
    }
}
