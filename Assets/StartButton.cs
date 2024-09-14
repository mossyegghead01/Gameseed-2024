using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the button component
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();

        // Add a listener for the button click event
        button.onClick.AddListener(ChangeScene);
    }

    // Function to change the scene
    void ChangeScene()
    {
        // Load the next scene (assuming it's the next scene in the build index)
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

}
