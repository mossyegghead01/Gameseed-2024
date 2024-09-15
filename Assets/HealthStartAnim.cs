using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthStartAnim : MonoBehaviour
{
    [SerializeField] private float delay, animTime = 0.5f;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(AnimateRight());
    }

    private IEnumerator AnimateRight()
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("hi");
        float startRight = -170f;
        float endRight = 0f; // RectTransform's right is the negative of the desired value
        float elapsedTime = 0f;

        while (elapsedTime < animTime)
        {
            float t = elapsedTime / animTime;
            float easedT = Mathf.SmoothStep(0f, 1f, t);
            float newRight = Mathf.Lerp(startRight, endRight, easedT);

            rectTransform.offsetMax = new Vector2(newRight, rectTransform.offsetMax.y);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.offsetMax = new Vector2(endRight, rectTransform.offsetMax.y);
    }
}
