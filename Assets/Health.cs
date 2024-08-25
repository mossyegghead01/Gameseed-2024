using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;

    void Update()
    {
        if (health < 0) {
            health = 0;
        }
        if (health <= 10 && health >= 5)
        {
            print(gameObject.name + " is low on health!");
        }
    }
}
