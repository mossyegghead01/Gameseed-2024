using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Object Health
    public float health = 100;
    // Object Maximum Health
    public float maxHealth = 100;
    // Should object be destroyed after the health ran out?
    public bool destroyAfterDeath = false;
    // Should this object death increment score?
    public bool incrementScoreOnDeath = false;
    public float IncrementMultiplier { private get; set; } = 1;
    public float scoreValue = 1;
    private AudioSource audioSource;
    [SerializeField] GameObject scoreObject, black;
    [SerializeField] Volume volume;
    private Vignette vignette;
    private ChromaticAberration ca;
    private bool isDead = false;
    [SerializeField] RectTransform healthbar;

    void Start()
    {
        // healthbar = GameObject.Find("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).transform.GetComponent<RectTransform>();
        // Clamp health
        health = Mathf.Clamp(health, 0, maxHealth);
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < volume.profile.components.Count; i++)
        {
            if (volume.profile.components[i].GetType() == typeof(Vignette))
            {
                vignette = (Vignette)volume.profile.components[i];
            }
            else if (volume.profile.components[i].GetType() == typeof(ChromaticAberration))
            {
                ca = (ChromaticAberration)volume.profile.components[i];
            }
        }
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        // Still clamping health
        if (health <= 0)
        {
            if (incrementScoreOnDeath)
            {
                EventSystem.current.GetComponent<UIHandlers>().IncrementScore(scoreValue * IncrementMultiplier);
            }
            Dead();
        }
        if (!audioSource.isPlaying)
        {
            vignette.intensity.value = 0.371f;
            vignette.color.value = new Color(0, 0, 0, 1);
        }

    }
    public float GetHealth()
    {
        return health;
    }
    public void SetHealth(float health)
    {
        this.health = health;
    }
    public void ModifyHealth(float modHealth)
    {
        healthbar.offsetMax = new Vector2(-(170 - (health / maxHealth * 170)), healthbar.offsetMax.y);
        health += modHealth;
        if (modHealth <= 0)
        {
            ca.intensity.value = .163f + (.197f * (1 - (health / maxHealth)));
            // else if (volume.profile.components[i].GetType() == typeof(Vignette))
            // {
            //     Vignette vignette = (Vignette)volume.profile.components[i];
            //     vignette.intensity.value = .371f + (.129f * (1 - (health / maxHealth)));
            //     vignette.color.value = new Color(1 - (health / maxHealth), 0, 0, 1);
            // }
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                vignette.intensity.value = 0.5f;
                vignette.color.value = new Color(1, 0, 0, 1);
            }
            else
            {
                vignette.intensity.value = 0.371f;
                vignette.color.value = new Color(0, 0, 0, 1);
            }

        }
    }
    void Dead()
    {

        // Change scene when player is dead


        // Transition black to full opacity
        if (!isDead)
            StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        isDead = true;
        float duration = 1f; // Duration of the fade in seconds
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            black.GetComponent<Image>().color = new Color(0.08627451f, 0.08627451f, .08627451f, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we reach full opacity
        black.GetComponent<Image>().color = new Color(.08627451f, .08627451f, .08627451f, 1);

        // After fading to black, proceed with changing the scene

        AudioSource flatlineAudio = GameObject.Find("Flatline").GetComponent<AudioSource>();
        flatlineAudio.Play();
        Debug.Log("Play flatline");
        while (flatlineAudio.isPlaying)
        {
            yield return null;
        }
        Debug.Log("Switch scene");

        scoreObject.GetComponent<Score>().DeathScene();
    }
}
