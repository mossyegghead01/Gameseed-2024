using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Health : MonoBehaviour
{
    // Object Health
    public float health = 100;
    // Object Maximum Health
    public float maxHealth = 100;
    // Should object be destroyed after the health ran out?
    public bool destroyAfterDeath = false;
    // Should this object death increment score?
    public bool incrementScoreOnDeath = false;

    void Start()
    {
        // Clamp health
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    void Update()
    {
        // Still clamping health
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            if (destroyAfterDeath)
            {
                Destroy(this.gameObject);
            }
            if (incrementScoreOnDeath)
            {
                EventSystem.current.GetComponent<UIHandlers>().IncrementScore();
            }
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
        health += modHealth;
    }
}
