using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Consider merging with Harvesting script later.
public class WeaponRolling : MonoBehaviour
{
    // You need explanation bro?
    // Aight here you go
    // Gun template
    // Yeah, that's it.
    public GameObject gunPrefab;
    // Also confused?
    // Go play minecraft.
    public GameObject inventory;
    
    // Don't judge me on these two classes, I used too much java and I love inheritance.
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
    public StatBounds PiercingBounds = new(0, 3);

    // TEMPORARY, CHANGE WITH DIFFICULTY SCALING LATER
    public StatBounds damageBounds = new(5, 100);

    // Roll the dice baby, its time to go gambling
    // AW DANGIT
    public void Roll()
    {
        // Tinky Winky, Dipsy, La-La, Po. Who's getting sacreficed into the almighty god?
        // Oh wait, it's just rolling for how the gun would work.
        var firetype = (GunProperty.FireType)UnityEngine.Random.Range(0, 3);
        // Change inventory.transform later, used for testing for now
        GunProperty gun = Instantiate(gunPrefab, inventory.transform).GetComponent<GunProperty>();
        switch (firetype)
        {
            case GunProperty.FireType.Single:
                // No, there's no hot single in your area. You're on your own bro.
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.GoodRollRange().lower, fireRateBounds.GoodRollRange().upper) * 100)/100.0;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.GoodRollRange().lower, rangeBounds.GoodRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.GoodRollRange().lower, PiercingBounds.GoodRollRange().upper));
                break;
            case GunProperty.FireType.Scatter:
                // Your gun go pew, mine goes pew pew pew
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.GoodRollRange().lower, fireRateBounds.GoodRollRange().upper)*100)/100.0;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.GoodRollRange().lower, damageBounds.GoodRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.MidRollRange().lower, PiercingBounds.MidRollRange().upper));
                break;
            case GunProperty.FireType.Automatic:
                // America's wife
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.BadRollRange().lower, fireRateBounds.BadRollRange().upper) * 100)/100.0;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.BadRollRange().lower, PiercingBounds.BadRollRange().upper));
                break;
            default:
                // Still here?
                // Yeah, I also wonder how did you managed to trick the compiler to let you reference a nonexistent enum value and run it.
                // Either way, enjoy your second error message in this whole debacle.
                Debug.LogException(new System.MissingFieldException("First in the gun property, now in its gacha. Seriously, pick something that exist!"));
                break;
        }
        gun.gunFireType = firetype;
        EventSystem.current.transform.GetComponent<UIHandlers>().UpdateUI();
    }
}
