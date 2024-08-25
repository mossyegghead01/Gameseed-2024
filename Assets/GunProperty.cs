using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProperty : MonoBehaviour
{
    public double fireRate = 0.2;
    public float projectileSpeed = 10;
    public GameObject bulletPrefab;
    public float weaponRange = 50;
    public float damage = 5;

    private double cooldown = 0.0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (cooldown > 0.0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            cooldown = 0.0;
        }
    }

    public void Fire()
    {
        if (cooldown == 0.0)
        {
            cooldown = fireRate;
            GetComponentInChildren<ParticleSystem>().Play();
            GameObject bullet = Instantiate(bulletPrefab, transform.GetChild(1).transform.position, GetComponentInParent<Transform>().rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * projectileSpeed, ForceMode2D.Impulse);
            bullet.GetComponent<BulletHit>().SetRange(weaponRange);
            bullet.GetComponent<BulletHit>().SetDamage(damage);
        }
    }
}
