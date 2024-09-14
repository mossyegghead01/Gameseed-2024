using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    private float score;
    [SerializeField] TextMeshProUGUI scoreText;
    public void SetScore(float score)
    {
        this.score = score;
        StartCoroutine(AnimateScore());
    }

    private IEnumerator AnimateScore()
    {
        float animationDuration = 1.5f;
        float elapsedTime = 0f;
        float currentScore = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            t = 1f - Mathf.Pow(1f - t, 3f);
            currentScore = Mathf.Lerp(0, score, t);
            scoreText.text = "Final Score : \n" + Mathf.Round(currentScore);
            yield return null;
        }

        scoreText.text = "Final Score : \n" + score;
    }
}
