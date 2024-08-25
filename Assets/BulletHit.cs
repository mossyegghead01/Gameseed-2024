using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    private float range;
    private float damage;

    private Vector3 lastPos;
    private float totalDist;

    void Start()
    {
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        var dist = (transform.position - lastPos).magnitude;
        totalDist += dist;
        lastPos = transform.position;

        if (totalDist > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var collidedHealth = collision.gameObject.GetComponent<Health>();
        if (collidedHealth != null)
        {
            collidedHealth.health -= damage;
        }
        Destroy(gameObject);
    }

    public void SetRange(float range)
    {
        this.range = range;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
