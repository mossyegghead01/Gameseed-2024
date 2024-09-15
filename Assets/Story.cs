using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
    private TMP_Text textMesh;
    private string[] dialogues = new string[]
    {
        "You were walking back\nfrom your job",
        "When suddenly, you forgot\nto actually use your damn eyes",
        "You fell for what\nfelt like forever",
        "Into a clearly marked\nhole in the ground",
        "Now you're in a\ncave stranded",
        "Hopefully no monsters\nare around",
        "lol",
        "[LMB] Shoot\n[RMB] Build/Break"
    };
    private int currentDialogueIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        if (textMesh == null)
        {
            Debug.LogError("TextMesh component not found!");
            return;
        }
        StartCoroutine(ChangeText(dialogues[currentDialogueIndex], 2.0f));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAnimating)
        {
            NextDialogue();
        }
    }

    void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
        {
            StartCoroutine(ChangeText(dialogues[currentDialogueIndex], 2.0f));
        }
        else
        {
            ChangeScene();
        }
    }

    public IEnumerator ChangeText(string newText, float animationDuration)
    {
        isAnimating = true;
        if (textMesh == null)
        {
            Debug.LogError("TextMesh component not found!");
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            int charCount = Mathf.FloorToInt(Mathf.Lerp(0, newText.Length, t));
            textMesh.text = newText.Substring(0, charCount);

            yield return null;
        }

        textMesh.text = newText;
        isAnimating = false;
    }

    void ChangeScene()
    {
        // Assuming the next scene is the one with build index + 1
        // You may need to adjust this based on your scene setup
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene("SampleScene");
    }
}
