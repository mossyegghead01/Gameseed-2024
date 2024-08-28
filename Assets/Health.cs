using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Object Health
    public float health = 100;
    // Object Maximum Health
    public float maxHealth = 100;

    void Start()
    {
        // Clamp health
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    void Update()
    {
        // Still clamping health
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
