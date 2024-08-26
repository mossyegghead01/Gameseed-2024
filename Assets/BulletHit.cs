using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    // Bullet properties, inheritted from the gun it was fired from.
    private float range;
    private float damage;

    // Internal properties
    private Vector3 initialPos;

    void Start()
    {
        // Initial position where the bullet were fired
        initialPos = transform.position;
    }

    void FixedUpdate()
    {
        // How far the bullet has traveled
        // If it exceed the bullet range, destroy the bullet
        var dist = (transform.position - initialPos).magnitude;

        if (dist > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object it collided with has health
        // If it has, reduce it by the amount of bullet damage
        if (collision.gameObject.TryGetComponent<Health>(out var collidedHealth))
        {
            collidedHealth.health -= damage;
        }
        // Idea: Piercing
        // Instead of destroying on its first hit, destroy it after several hit
        Destroy(gameObject);
    }

    // Set the bullet range
    public void SetRange(float range)
    {
        this.range = range;
    }

    // Set the bullet damage
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
