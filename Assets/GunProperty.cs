using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProperty : MonoBehaviour
{
    // Gun Properties
    // Some properties will be inherited to bullets it fired.
    public double fireRate = 0.2;
    public float projectileSpeed = 10;
    public GameObject bulletPrefab;
    public float weaponRange = 50;
    public float damage = 5;

    // Internal values
    private double cooldown = 0.0;

    void FixedUpdate()
    {
        // Cooldown decrease and reset
        if (cooldown > 0.0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            cooldown = 0.0;
        }
    }

    // Fire this gun
    public void Fire()
    {
        if (cooldown == 0.0)
        {
            // Set the cooldown
            cooldown = fireRate;

            // Partcle system, temporary
            GetComponentInChildren<ParticleSystem>().Play();
            
            // Instantiate bullet from prefab into the world with position inherited from the BulletSpawn Object and rotation inherited from gun rotation axis
            GameObject bullet = Instantiate(bulletPrefab, transform.GetChild(1).transform.position, GetComponentInParent<Transform>().rotation);
            // Apply Force then set range and damage of the bullet
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * projectileSpeed, ForceMode2D.Impulse);
            bullet.GetComponent<BulletHit>().SetRange(weaponRange);
            bullet.GetComponent<BulletHit>().SetDamage(damage);
        }
    }
}
