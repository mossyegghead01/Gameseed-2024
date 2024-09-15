using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour
{
    public bool fadeOnStart = false;
    public float delay = 0f;
    // Start is called before the first frame update
    public IEnumerator FadeToBlack(System.Action callback = null)
    {
        float duration = 1f; // Duration of the fade in seconds
        float elapsedTime = 0f;
        GetComponent<Image>().color = new Color(0.08627451f, 0.08627451f, .08627451f, 0);
        Debug.Log("hihih");
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            GetComponent<Image>().color = new Color(0.08627451f, 0.08627451f, .08627451f, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we reach full opacity
        GetComponent<Image>().color = new Color(.08627451f, .08627451f, .08627451f, 1);

        callback?.Invoke();
    }
    public IEnumerator FadeToNormal(System.Action callback = null)
    {
        GetComponent<Image>().color = new Color(0.08627451f, 0.08627451f, .08627451f, 1);
        yield return new WaitForSeconds(delay);
        float duration = 1f; // Duration of the fade in seconds
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            GetComponent<Image>().color = new Color(0.08627451f, 0.08627451f, .08627451f, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we reach full transparency
        GetComponent<Image>().color = new Color(.08627451f, .08627451f, .08627451f, 0);
        callback?.Invoke();
    }

    void Start()
    {
        if (fadeOnStart)
        {
            StartCoroutine(FadeToNormal());
        }
    }
}
