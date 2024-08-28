using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponRolling : MonoBehaviour
{
    public GameObject gunPrefab;
    public GameObject inventory;
    [Serializable]
    public class Bounds
    {
        public float lower;
        public float upper;

        public Bounds(float lower, float upper)
        {
            this.lower = lower;
            this.upper = upper;
        }
    }

    [Serializable]
    public class StatBounds : Bounds
    {
        public StatBounds(float lower, float upper) : base(lower, upper) { }

        public float Range()
        {
            return this.upper - this.lower;
        }

        public Bounds GoodRollRange()
        {
            var a = this.Range();
            return new(Mathf.Round((a / 3) * 200) / 100.0f, this.upper);
        }

        public Bounds MidRollRange()
        {
            var a = this.Range();
            return new(Mathf.Round((a / 3) * 100) / 100.0f, Mathf.Round((a / 3) * 200) / 100.0f);
        }

        public Bounds BadRollRange()
        {
            var a = this.Range();
            return new(this.lower, Mathf.Round((a / 3) * 100) / 100.0f);
        }
    }

    // Note for fireRate
    // GoodRollRange is actually BadRollRange and vice versa
    // Fuck mossy (me) for coding it that way
    public StatBounds fireRateBounds = new(0.015f, 1.0f);
    public StatBounds projectileSpeedBounds = new(10, 50);
    public StatBounds rangeBounds = new(20, 70);

    // TEMPORARY, CHANGE WITH DIFFICULTY SCALING LATER
    public StatBounds damageBounds = new(5, 100);

    public void Roll()
    {
        var firetype = (GunProperty.FireType)UnityEngine.Random.Range(0, 3);
        // Change inventory.transform later, used for testing for now
        GunProperty gun = Instantiate(gunPrefab, inventory.transform).GetComponent<GunProperty>();
        switch (firetype)
        {
            case GunProperty.FireType.Single:
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.GoodRollRange().lower, fireRateBounds.GoodRollRange().upper) * 100)/100.0;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.GoodRollRange().lower, rangeBounds.GoodRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                break;
            case GunProperty.FireType.Scatter:
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.GoodRollRange().lower, fireRateBounds.GoodRollRange().upper)*100)/100.0;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.GoodRollRange().lower, damageBounds.GoodRollRange().upper));
                break;
            case GunProperty.FireType.Automatic:
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.BadRollRange().lower, fireRateBounds.BadRollRange().upper) * 100)/100.0;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                break;
            default:
                Debug.LogException(new System.MissingFieldException("First in the gun property, now in its gacha. Seriously, pick something that exist!"));
                break;
        }
        gun.gunFireType = firetype;
        EventSystem.current.transform.GetComponent<UIHandlers>().UpdateUI();
    }
}
