using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UnityEngine.UI.Button button;

    void Start()
    {
        // Get the button component
        button = GetComponent<UnityEngine.UI.Button>();

        // Add a listener for the button click event
        button.onClick.AddListener(ChangeScene);
    }

    // Function to change the scene
    void ChangeScene()
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

            if (SceneManager.GetActiveScene().name == "StartMenu")
            {
                StartCoroutine(GameObject.Find("Canvas/Black").GetComponent<FadeBlack>().FadeToBlack(() => SceneManager.LoadScene("Story")));

            }
            else if (SceneManager.GetActiveScene().name == "DeathMenu")
            {
                StartCoroutine(GameObject.Find("Canvas/Black").GetComponent<FadeBlack>().FadeToBlack(() => SceneManager.LoadScene("SampleScene")));
            }
            else
            {
                SceneManager.LoadScene("StartMenu");
            }
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
