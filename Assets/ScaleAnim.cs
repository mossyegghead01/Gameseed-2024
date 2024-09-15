using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScaleAnim : MonoBehaviour
{
    [SerializeField] private float delay, animTime = 0.5f;
    [SerializeField] private float startScale = 1.5f; // New variable for initial larger scale
    public bool active = true;
    void Start()
    {
        // Make object invisible and larger on start
        transform.localScale = Vector3.one * startScale;
        if (GetComponent<Image>() != null)
        {

            GetComponent<Image>().enabled = false;

            // Disable Image component for all child objects
        }
        else if (GetComponent<TMP_Text>() != null)
        {
            GetComponent<TMP_Text>().enabled = false;
        }

        foreach (Transform child in transform)
        {
            Image childImage = child.GetComponent<Image>();
            TMP_Text childText = child.GetComponent<TMP_Text>();
            if (childImage != null)
            {
                childImage.enabled = false;
            }
            else if (childText != null)
            {
                childText.enabled = false;
            }
        }

        // Start coroutine to transition to original size after delay
        StartCoroutine(TransitionToOriginalSize());
    }

    public IEnumerator TransitionToOriginalSize()
    {
        if (active)
        {

            // Wait for the specified delay
            yield return new WaitForSeconds(delay);

            // Make object visible
            if (GetComponent<Image>() != null)
            {

                GetComponent<Image>().enabled = true;
            }
            else if (GetComponent<TMP_Text>() != null)
            {
                GetComponent<TMP_Text>().enabled = true;
            }
            foreach (Transform child in transform)
            {
                Image childImage = child.GetComponent<Image>();
                TMP_Text childText = child.GetComponent<TMP_Text>();
                if (childImage != null)
                {
                    childImage.enabled = true;
                }
                else if (childText != null)
                {
                    childText.enabled = true;
                }
            }
            // Transition to original size over animTime seconds with smooth easing
            float elapsedTime = 0f;
            Vector3 startSize = Vector3.one * startScale;
            Vector3 targetSize = Vector3.one;
            while (elapsedTime < animTime)
            {
                float t = elapsedTime / animTime;
                t = t * t * (3f - 2f * t); // Smooth step function for easing
                transform.localScale = Vector3.Lerp(startSize, targetSize, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Scale animation completed.");

            // Ensure the final scale is exactly Vector3.one
            transform.localScale = targetSize;
        }
    }
}