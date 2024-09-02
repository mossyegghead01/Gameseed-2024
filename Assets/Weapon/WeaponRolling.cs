using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using static GunProperty;

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
    public Sprite placeholderSprite;

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

    public enum WeaponPlantType { Eggplant, Carrot, Corn, Tomato, Cauliflower, Broccolli }
    public List<string> weaponListScatter = new()
    {
        "Mossberg",
        "CoachGun",
        "BrowningCitori",
        "Winchester1887",
    };
    public List<string> weaponListSingle = new()
    {
        "DessertEagle",
        "Remington700",
        "AR15",
        "SCARH",
    };
    public List<string> weaponListBurst = new()
    {
        "FAMAS",
        "AN94",
        "Beretta93R",
        "MP5A2",
    };
    public List<string> weaponListAuto = new()
    {
        "AK47",
        "Uzi",
        "M16",
        "PKM",
    };


    public Dictionary<WeaponPlantType, AbilityTypes> abilityMap = new()
    {
        {WeaponPlantType.Eggplant, AbilityTypes.Accuracy},
        {WeaponPlantType.Carrot, AbilityTypes.Range},
        {WeaponPlantType.Corn, AbilityTypes.FireRate},
        {WeaponPlantType.Tomato, AbilityTypes.Damage},
        {WeaponPlantType.Cauliflower, AbilityTypes.MovementBonus},
        {WeaponPlantType.Broccolli, AbilityTypes.PointsMultiplier},
    };
    //{
    //    new(WeaponPlantType.Eggplant, AbilityTypes.Accuracy),
    //    new(WeaponPlantType.Carrot, AbilityTypes.Range),
    //    new(WeaponPlantType.Corn, AbilityTypes.FireRate),
    //    new(WeaponPlantType.Tomato, AbilityTypes.Damage),
    //    new(WeaponPlantType.Cauliflower, AbilityTypes.MovementBonus),
    //    new(WeaponPlantType.Broccolli, AbilityTypes.PointsMultiplier),
    //};

    // Note for fireRate
    // GoodRollRange is actually BadRollRange and vice versa
    // Fuck mossy (me) for coding it that way
    public StatBounds fireRateBounds = new(0.015f, 1.0f);
    public StatBounds projectileSpeedBounds = new(10, 50);
    public StatBounds rangeBounds = new(20, 70);
    public StatBounds PiercingBounds = new(0, 3);
    public StatBounds innacuracyBounds = new(0, 5);
    public StatBounds movementBonusBounds = new(-2, 2);
    public StatBounds reloadSpeedBounds = new(2, 7);

    // TEMPORARY, CHANGE WITH DIFFICULTY SCALING LATER
    public StatBounds damageBounds = new(5, 100);

    public void RollAny()
    {
        Roll((WeaponPlantType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(WeaponPlantType)).Length));
    }

    // Roll the dice baby, its time to go gambling
    // AW DANGIT
    public void Roll(WeaponPlantType plantType = WeaponPlantType.Eggplant)
    {
        // Tinky Winky, Dipsy, La-La, Po. Who's getting sacreficed into the almighty god?
        // Oh wait, it's just rolling for how the gun would work.
        var firetype = (GunProperty.FireType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(GunProperty.FireType)).Length);
        // Change inventory.transform later, used for testing for now
        GunProperty gun = Instantiate(gunPrefab, inventory.transform).GetComponent<GunProperty>();
        Sprite gunSprite = placeholderSprite;
        switch (firetype)
        {
            case GunProperty.FireType.Single:
                // No, there's no hot single in your area. You're on your own bro.
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.GoodRollRange().lower, fireRateBounds.GoodRollRange().upper) * 100)/100.0f;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.GoodRollRange().lower, rangeBounds.GoodRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.GoodRollRange().lower, PiercingBounds.GoodRollRange().upper));
                gun.innacuracy = Mathf.Round(UnityEngine.Random.Range(innacuracyBounds.GoodRollRange().lower, innacuracyBounds.GoodRollRange().upper));
                gun.movementBonus = Mathf.Round(UnityEngine.Random.Range(movementBonusBounds.MidRollRange().lower, movementBonusBounds.MidRollRange().upper));
                gun.reloadSpeed = Mathf.Round(UnityEngine.Random.Range(reloadSpeedBounds.MidRollRange().lower, reloadSpeedBounds.MidRollRange().upper));
                List<int> magSizesSingle = new() { 10, 12, 30 };
                gun.magazineSize = magSizesSingle[UnityEngine.Random.Range(0, magSizesSingle.Count)];
                gun.gunAbility = new Ability(abilityMap[plantType], 2);
                gunSprite = Resources.Load<Sprite>("Sprites/Guns/" + weaponListSingle[UnityEngine.Random.Range(0, weaponListSingle.Count)] + "_" + Enum.GetName(typeof(WeaponPlantType), plantType));
                break;
            case GunProperty.FireType.Scatter:
                // Your gun go pew, mine goes pew pew pew
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.GoodRollRange().lower, fireRateBounds.GoodRollRange().upper)*100)/100.0f;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.GoodRollRange().lower, damageBounds.GoodRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.MidRollRange().lower, PiercingBounds.MidRollRange().upper));
                gun.innacuracy = Mathf.Round(UnityEngine.Random.Range(innacuracyBounds.MidRollRange().lower, innacuracyBounds.MidRollRange().upper));
                gun.movementBonus = Mathf.Round(UnityEngine.Random.Range(movementBonusBounds.BadRollRange().lower, movementBonusBounds.BadRollRange().upper));
                gun.reloadSpeed = Mathf.Round(UnityEngine.Random.Range(reloadSpeedBounds.BadRollRange().lower, reloadSpeedBounds.BadRollRange().upper));
                List<int> magSizesScatter = new() { 5, 7 };
                gun.magazineSize = magSizesScatter[UnityEngine.Random.Range(0, magSizesScatter.Count)];
                gun.gunAbility = new Ability(abilityMap[plantType], 2);
                gunSprite = Resources.Load<Sprite>("Sprites/Guns/" + weaponListScatter[UnityEngine.Random.Range(0, weaponListScatter.Count)] + "_" + Enum.GetName(typeof(WeaponPlantType), plantType));
                break;
            case GunProperty.FireType.Automatic:
                // America's wife
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.BadRollRange().lower, fireRateBounds.BadRollRange().upper) * 100)/100.0f;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.BadRollRange().lower, PiercingBounds.BadRollRange().upper));
                gun.innacuracy = Mathf.Round(UnityEngine.Random.Range(innacuracyBounds.BadRollRange().lower, innacuracyBounds.BadRollRange().upper));
                gun.movementBonus = Mathf.Round(UnityEngine.Random.Range(movementBonusBounds.GoodRollRange().lower, movementBonusBounds.GoodRollRange().upper));
                gun.reloadSpeed = Mathf.Round(UnityEngine.Random.Range(reloadSpeedBounds.GoodRollRange().lower, reloadSpeedBounds.GoodRollRange().upper));
                List<int> magSizesAuto = new() { 30, 31 };
                gun.magazineSize = magSizesAuto[UnityEngine.Random.Range(0, magSizesAuto.Count)];
                gun.gunAbility = new Ability(abilityMap[plantType], 2);
                gunSprite = Resources.Load<Sprite>("Sprites/Guns/" + weaponListAuto[UnityEngine.Random.Range(0, weaponListAuto.Count)] + "_" + Enum.GetName(typeof(WeaponPlantType), plantType));
                break;
            case FireType.Burst:
                // Basically scatter but better, or worse actually?
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.BadRollRange().lower, fireRateBounds.BadRollRange().upper) * 100) / 100.0f;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.MidRollRange().lower, PiercingBounds.MidRollRange().upper));
                gun.innacuracy = Mathf.Round(UnityEngine.Random.Range(innacuracyBounds.BadRollRange().lower, innacuracyBounds.BadRollRange().upper));
                gun.movementBonus = Mathf.Round(UnityEngine.Random.Range(movementBonusBounds.GoodRollRange().lower, movementBonusBounds.GoodRollRange().upper));
                gun.reloadSpeed = Mathf.Round(UnityEngine.Random.Range(reloadSpeedBounds.GoodRollRange().lower, reloadSpeedBounds.GoodRollRange().upper));
                List<int> magSizesBurst = new() { 27, 30 };
                gun.magazineSize = magSizesBurst[UnityEngine.Random.Range(0, magSizesBurst.Count)];
                gunSprite = Resources.Load<Sprite>("Sprites/Guns/" + weaponListBurst[UnityEngine.Random.Range(0, weaponListBurst.Count)] + "_" + Enum.GetName(typeof(WeaponPlantType), plantType));
                gun.gunAbility = new Ability(abilityMap[plantType], 2);
                break;
            default:
                // Still here?
                // Yeah, I also wonder how did you managed to trick the compiler to let you reference a nonexistent enum value and run it.
                // Either way, enjoy your second error message in this whole debacle.
                Debug.LogException(new System.MissingFieldException("First in the gun property, now in its gacha. Seriously, pick something that exist!"));
                break;
        }
        gun.gunFireType = firetype;
        gun.GetComponent<Item>().itemIcon = gunSprite;
        gun.GetComponent<SpriteRenderer>().sprite = gunSprite;
        EventSystem.current.transform.GetComponent<UIHandlers>().UpdateUI();
    }
}
