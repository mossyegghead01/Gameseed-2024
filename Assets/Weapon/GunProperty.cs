using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GunProperty : MonoBehaviour
{
    // Firing type of  the gun
    public enum FireType { Automatic, Single, Scatter, Burst }

    // Gun Properties
    // Some properties will be inherited to bullets it fired.
    public float fireRate = 0.2f;
    public float projectileSpeed = 10;
    public GameObject bulletPrefab;
    public float weaponRange = 50;
    public float damage = 5;
    public FireType gunFireType = FireType.Automatic;
    public float piercing = 0;
    public float movementBonus = 0;
    public float magazineSize = 30;
    public float reloadSpeed = 2;
    public float innacuracy = 1;

    // Single Shot and Scatter Shot internals
    private bool fired = false;

    // Burst Shot Internals
    private float burstCount = 0;

    // Universal internals
    private double cooldown = 0.0;
    private double reloadCooldown = 0.0;
    private bool shooting = false;
    private bool reloading = false;
    public float MagazineContent { get; private set; }

    // Spawn bullet(s), what else do you expect?
    private void SpawnBullet(float angleOffset, bool decrementBulletHere = false)
    {
        // Instantiate bullet from prefab into the world with position inherited from the BulletSpawn Object and rotation inherited from gun rotation axis
        GameObject bullet = Instantiate(bulletPrefab, transform.GetChild(1).transform.position, transform.GetChild(1).transform.rotation * Quaternion.Euler(0, 0, angleOffset + Random.Range(-innacuracy, innacuracy)));
        // Apply Force then set range and damage of the bullet
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right * projectileSpeed, ForceMode2D.Impulse);
        bullet.GetComponent<BulletHit>().SetRange(weaponRange);
        bullet.GetComponent<BulletHit>().SetDamage(damage);
        bullet.GetComponent<BulletHit>().SetPiercing(piercing);

        if (decrementBulletHere) 
        {
            MagazineContent --;
        }
    }

    void Start()
    {
        MagazineContent = magazineSize;
    }

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

        if (reloadCooldown > 0.0)
        {
            reloadCooldown -= Time.deltaTime;
        }
        else if (reloading)
        {
            reloading = false;
            MagazineContent = magazineSize;
            reloadCooldown = 0.0;
        }

        if (shooting && MagazineContent > 0)
        {
            switch (gunFireType)
            {
                case FireType.Automatic:
                    // Continuous fire
                    if (cooldown == 0.0)
                    {
                        cooldown = fireRate;

                        // Partcle system, temporary
                        GetComponentInChildren<ParticleSystem>().Play();

                        SpawnBullet(0, true);
                    }
                    break;
                case FireType.Single:
                    // "Bolt Action"
                    if (!fired && cooldown == 0.0)
                    {
                        fired = true;
                        cooldown = fireRate;

                        // Partcle system, temporary
                        GetComponentInChildren<ParticleSystem>().Play();

                        SpawnBullet(0, true);
                    }
                    break;
                case FireType.Scatter:
                    // Still "Bolt Action" but cooler
                    if (!fired && cooldown == 0.0)
                    {
                        fired = true;
                        cooldown = fireRate;

                        // Partcle system, temporary
                        GetComponentInChildren<ParticleSystem>().Play();

                        SpawnBullet(0);
                        SpawnBullet(20);
                        SpawnBullet(-20);
                        MagazineContent --;
                    }
                    break;
                case FireType.Burst:
                    if (cooldown == 0.0 && burstCount < 3)
                    {
                        cooldown = fireRate;

                        // Partcle system, temporary
                        GetComponentInChildren<ParticleSystem>().Play();

                        SpawnBullet(0, true);
                        burstCount ++;
                    }
                    break;
                default:
                    // For whoever messed up so bad their compiler gave up on checking errors
                    // I don't even know how or why you reached this point. Usually, the compiler screamed at you if you used a nonexistent enum value.
                    Debug.LogException(new System.MissingFieldException("What the heck are you trying to do mate? This thing does not exist!"));
                    break;
            }
        }
    }

    // Fire this gun
    public void Fire()
    {
        shooting = true;
        
    }

    // Stop firing the gun
    public void EndFire()
    {
        shooting = false;
        fired = false;
        burstCount = 0;
    }

    public void Reload()
    {
        if (reloadCooldown <= 0)
        {
            reloading = true;
            reloadCooldown = reloadSpeed;
        }
    }
}
