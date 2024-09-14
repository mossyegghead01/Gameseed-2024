using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] GameObject scoreObject;
    [SerializeField] Volume volume;

    void Start()
    {
        // Clamp health
        health = Mathf.Clamp(health, 0, maxHealth);
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
        var healthbar = GameObject.Find("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).transform.GetComponent<RectTransform>();
        healthbar.offsetMax = new Vector2(-(170 - (health / maxHealth * 170)), healthbar.offsetMax.y);

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
        health += modHealth;
        if (modHealth <= 0)
        {
            for (int i = 0; i < volume.profile.components.Count; i++)
            {
                if (volume.profile.components[i].GetType() == typeof(ChromaticAberration))
                {
                    ChromaticAberration ca = (ChromaticAberration)volume.profile.components[i];
                    ca.intensity.value = .163f + (.197f * (1 - (health / maxHealth)));
                }
                else if (volume.profile.components[i].GetType() == typeof(Vignette))
                {
                    Vignette vignette = (Vignette)volume.profile.components[i];
                    vignette.intensity.value = .371f + (.129f * (1 - (health / maxHealth)));
                    vignette.color.value = new Color(1 - (health / maxHealth), 0, 0, 1);
                    Debug.Log(new Color(1 - (health / maxHealth), 0, 0, 1));
                }
            }
        }
    }
    void Dead()
    {

        // Change scene when player is dead

        scoreObject.GetComponent<Score>().DeathScene();
    }
}
