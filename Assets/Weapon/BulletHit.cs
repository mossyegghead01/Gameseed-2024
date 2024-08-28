using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    // Bullet properties, inheritted from the gun it was fired from.
    private float range;
    private float damage;
    private float piercing;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<BulletHit>(out _))
        {
            // Check if the object it collided with has health
            // If it has, reduce it by the amount of bullet damage
            if (collision.gameObject.TryGetComponent<Health>(out var collidedHealth))
            {
                collidedHealth.health -= damage;
            }

            // Piercing
            // Instead of destroying on its first hit, destroy it after several hit
            if (piercing > 0)
            {
                piercing -= 1;
            }
            else
            {
                Destroy(gameObject);
            }
        }
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

    // Set how many enemy can the bullet pierce
    public void SetPiercing(float piercing)
    {
        this.piercing = piercing;
    }
}
