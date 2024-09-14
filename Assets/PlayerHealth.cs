using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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

    }
    void Dead()
    {

        // Change scene when player is dead

        scoreObject.GetComponent<Score>().DeathScene();
    }
}
