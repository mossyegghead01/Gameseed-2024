using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class Quit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UnityEngine.UI.Button button;

    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
    }

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

            StartCoroutine(GameObject.Find("Canvas/Black").GetComponent<FadeBlack>().FadeToBlack(() => SceneManager.LoadScene("StartMenu")));
        }
    }

    // Implement IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/hoverUI"));
    }

    // Implement IPointerExitHandler
    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
