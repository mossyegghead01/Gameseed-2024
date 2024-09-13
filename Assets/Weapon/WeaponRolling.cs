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
    public GameObject gachaHolder;

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

    public enum WeaponPlantType { Eggplant, Carrot, Corn, Tomato, Cauliflower, Broccoli }
    [NonSerialized]
    public List<string> weaponScatter = new()
    {
        "Mossberg500",
        "CoachGun",
        "BrowningCitori",
        "Winchester1887",
    };
    [NonSerialized]
    public List<string> weaponBurst = new()
    {
        "FAMAS",
        "AN94",
        "Beretta93R",
        "MP5A2",
    };
    [NonSerialized]
    public List<string> weaponAuto = new()
    {
        "AK47",
        "Uzi",
        "M16",
        "PKM",
    };
    [NonSerialized]
    public List<string> weaponSingle = new()
    {
        "DesertEagle",
        "Remington700",
        "AR15",
        "SCARH",
    };

    public Dictionary<string, Vector3> weaponTypesOffsets = new()
    {
        {"Mossberg500", new(0.3f, 0.045f, 0) },
        {"CoachGun", new(0.3f, 0.04f, 0) },
        {"BrowningCitori", new(0.3f, 0.03f, 0) },
        {"Winchester1887", new(0.3f, 0.02f, 0) },
        {"DesertEagle", new(0.15f, 0.025f, 0) },
        {"Remington700", new(0.3f, 0.015f, 0) },
        {"AR15", new(0.3f, 0.025f, 0) },
        {"SCARH", new(0.3f, 0.025f, 0) },
        {"FAMAS", new(0.3f, 0.02f, 0) },
        {"AN94", new(0.3f, 0.035f, 0) },
        {"Beretta93R", new(0.12f, 0.04f, 0) },
        {"MP5A2", new(0.2f, 0.02f, 0) },
        {"AK47", new(0.3f, 0.015f, 0) },
        {"Uzi", new(0.23f, 0.0125f, 0) },
        {"M16", new(0.3f, 0.015f, 0) },
        {"PKM", new(0.3f, 0.0125f, 0) },
    };


    public Dictionary<WeaponPlantType, Tuple<AbilityTypes, StatBounds>> abilityMap = new()
    {
        {WeaponPlantType.Eggplant, new(AbilityTypes.Accuracy, new(0, 2))},
        {WeaponPlantType.Carrot, new(AbilityTypes.Range, new(0, 10))},
        {WeaponPlantType.Corn, new(AbilityTypes.FireRate, new(0, 0.05f))},
        {WeaponPlantType.Tomato, new(AbilityTypes.Damage, new(0, 10))},
        {WeaponPlantType.Cauliflower, new(AbilityTypes.MovementBonus, new(0, 2))},
        {WeaponPlantType.Broccoli, new(AbilityTypes.PointsMultiplier, new(0, 3))},
    };

    // Note for fireRate
    // GoodRollRange is actually BadRollRange and vice versa
    // Fuck mossy (me) for coding it that way
    public StatBounds fireRateBounds = new(0.03f, 1.25f);
    public StatBounds projectileSpeedBounds = new(10, 50);
    public StatBounds rangeBounds = new(20, 70);
    public StatBounds PiercingBounds = new(0, 3);
    public StatBounds innacuracyBounds = new(0, 5);
    public StatBounds movementBonusBounds = new(-5, 5);
    public StatBounds reloadSpeedBounds = new(2, 7);



    // TEMPORARY, CHANGE WITH DIFFICULTY SCALING LATER
    //public StatBounds damageBounds = new(5, 100);

    public void RollAny(bool pushToInv = false)
    {
        Roll((WeaponPlantType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(WeaponPlantType)).Length), pushToInv);
    }

    // Roll the dice baby, its time to go gambling
    // AW DANGIT
    public void Roll(WeaponPlantType plantType = WeaponPlantType.Eggplant, bool pushToInv = false)
    {
        StatBounds damageBounds = ScalingFunctions.WeaponDamageScalling(EventSystem.current.GetComponent<UIHandlers>().GetScore());
        // Tinky Winky, Dipsy, La-La, Po. Who's getting sacreficed into the almighty god?
        // Oh wait, it's just rolling for how the gun would work.
        var firetype = (GunProperty.FireType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(GunProperty.FireType)).Length);
        // Change inventory.transform later, used for testing for now
        GunProperty gun = Instantiate(gunPrefab, pushToInv ? inventory.transform : gachaHolder.transform).GetComponent<GunProperty>();
        Sprite gunSprite = placeholderSprite;
        string weaponType = "";
        switch (firetype)
        {
            case GunProperty.FireType.Single:
                // No, there's no hot single in your area. You're on your own bro.
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.GoodRollRange().lower, fireRateBounds.GoodRollRange().upper) * 100) / 100.0f;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.GoodRollRange().lower, rangeBounds.GoodRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.GoodRollRange().lower, PiercingBounds.GoodRollRange().upper));
                gun.innacuracy = Mathf.Round(UnityEngine.Random.Range(innacuracyBounds.GoodRollRange().lower, innacuracyBounds.GoodRollRange().upper));
                gun.movementBonus = Mathf.Round(UnityEngine.Random.Range(movementBonusBounds.MidRollRange().lower, movementBonusBounds.MidRollRange().upper));
                gun.reloadSpeed = Mathf.Round(UnityEngine.Random.Range(reloadSpeedBounds.MidRollRange().lower, reloadSpeedBounds.MidRollRange().upper));
                List<int> magSizesSingle = new() { 10, 12, 30 };
                gun.magazineSize = magSizesSingle[UnityEngine.Random.Range(0, magSizesSingle.Count)];
                gun.gunAbility = new Ability(abilityMap[plantType].Item1, UnityEngine.Random.Range(abilityMap[plantType].Item2.lower, abilityMap[plantType].Item2.upper));
                weaponType = weaponSingle[UnityEngine.Random.Range(0, weaponSingle.Count)];
                print(weaponType);
                gunSprite = Resources.Load<Sprite>("Sprites/Guns/" + weaponType + "_" + Enum.GetName(typeof(WeaponPlantType), plantType));
                break;
            case GunProperty.FireType.Scatter:
                // Your gun go pew, mine goes pew pew pew
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.GoodRollRange().lower, fireRateBounds.GoodRollRange().upper) * 100) / 100.0f;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.GoodRollRange().lower, damageBounds.GoodRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.MidRollRange().lower, PiercingBounds.MidRollRange().upper));
                gun.innacuracy = Mathf.Round(UnityEngine.Random.Range(innacuracyBounds.MidRollRange().lower, innacuracyBounds.MidRollRange().upper));
                gun.movementBonus = Mathf.Round(UnityEngine.Random.Range(movementBonusBounds.BadRollRange().lower, movementBonusBounds.BadRollRange().upper));
                gun.reloadSpeed = Mathf.Round(UnityEngine.Random.Range(reloadSpeedBounds.BadRollRange().lower, reloadSpeedBounds.BadRollRange().upper));
                List<int> magSizesScatter = new() { 5, 7 };
                gun.magazineSize = magSizesScatter[UnityEngine.Random.Range(0, magSizesScatter.Count)];
                gun.gunAbility = new Ability(abilityMap[plantType].Item1, UnityEngine.Random.Range(abilityMap[plantType].Item2.lower, abilityMap[plantType].Item2.upper));
                weaponType = weaponScatter[UnityEngine.Random.Range(0, weaponScatter.Count)];
                print(weaponType);
                gunSprite = Resources.Load<Sprite>("Sprites/Guns/" + weaponType + "_" + Enum.GetName(typeof(WeaponPlantType), plantType));
                break;
            case GunProperty.FireType.Automatic:
                // America's wife
                gun.fireRate = Mathf.Round(UnityEngine.Random.Range(fireRateBounds.BadRollRange().lower, fireRateBounds.BadRollRange().upper) * 100) / 100.0f;
                gun.projectileSpeed = Mathf.Round(UnityEngine.Random.Range(projectileSpeedBounds.MidRollRange().lower, projectileSpeedBounds.MidRollRange().upper));
                gun.weaponRange = Mathf.Round(UnityEngine.Random.Range(rangeBounds.BadRollRange().lower, rangeBounds.BadRollRange().upper));
                gun.damage = Mathf.Round(UnityEngine.Random.Range(damageBounds.MidRollRange().lower, damageBounds.MidRollRange().upper));
                gun.piercing = Mathf.Round(UnityEngine.Random.Range(PiercingBounds.BadRollRange().lower, PiercingBounds.BadRollRange().upper));
                gun.innacuracy = Mathf.Round(UnityEngine.Random.Range(innacuracyBounds.BadRollRange().lower, innacuracyBounds.BadRollRange().upper));
                gun.movementBonus = Mathf.Round(UnityEngine.Random.Range(movementBonusBounds.GoodRollRange().lower, movementBonusBounds.GoodRollRange().upper));
                gun.reloadSpeed = Mathf.Round(UnityEngine.Random.Range(reloadSpeedBounds.GoodRollRange().lower, reloadSpeedBounds.GoodRollRange().upper));
                List<int> magSizesAuto = new() { 30, 31 };
                gun.magazineSize = magSizesAuto[UnityEngine.Random.Range(0, magSizesAuto.Count)];
                gun.gunAbility = new Ability(abilityMap[plantType].Item1, UnityEngine.Random.Range(abilityMap[plantType].Item2.lower, abilityMap[plantType].Item2.upper));
                weaponType = weaponAuto[UnityEngine.Random.Range(0, weaponAuto.Count)];
                print(weaponType);
                gunSprite = Resources.Load<Sprite>("Sprites/Guns/" + weaponType + "_" + Enum.GetName(typeof(WeaponPlantType), plantType));
                break;
            case GunProperty.FireType.Burst:
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
                weaponType = weaponBurst[UnityEngine.Random.Range(0, weaponBurst.Count)];
                print(weaponType);
                gunSprite = Resources.Load<Sprite>("Sprites/Guns/" + weaponType + "_" + Enum.GetName(typeof(WeaponPlantType), plantType));
                gun.gunAbility = new Ability(abilityMap[plantType].Item1, UnityEngine.Random.Range(abilityMap[plantType].Item2.lower, abilityMap[plantType].Item2.upper));
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
        gun.transform.GetChild(0).localPosition = weaponTypesOffsets[weaponType];
        gun.transform.GetChild(1).localPosition = weaponTypesOffsets[weaponType];
    }
}
