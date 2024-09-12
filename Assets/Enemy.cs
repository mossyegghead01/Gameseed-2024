using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyType type = EnemyType.Broccoli;
    // Start is called before the first frame update
    void Start()
    {
        if (type == EnemyType.Broccoli)
            GetComponent<Animator>().SetInteger("Type", 1);
        else if (type == EnemyType.Carrot)
            GetComponent<Animator>().SetInteger("Type", 2);
        else if (type == EnemyType.Corn)
            GetComponent<Animator>().SetInteger("Type", 3);
        else if (type == EnemyType.Cauliflower)
            GetComponent<Animator>().SetInteger("Type", 4);
        else if (type == EnemyType.Eggplant)
            GetComponent<Animator>().SetInteger("Type", 5);
        else if (type == EnemyType.Tomato)
            GetComponent<Animator>().SetInteger("Type", 6);
    }
    public void SetType(EnemyType type)
    {
        this.type = type;
        switch (type)
        {
            case EnemyType.Broccoli:
                GetComponent<Health>().SetMaxHealth(100);
                GetComponent<EnemyBreaking>().SetBreakSpeed(15f);
                GetComponent<EnemyBreaking>().SetDamage(0.3f);
                GetComponent<AIPath>().maxSpeed = 1f;
                break;
            case EnemyType.Carrot:
                GetComponent<Health>().SetMaxHealth(50);
                GetComponent<EnemyBreaking>().SetBreakSpeed(3);
                GetComponent<EnemyBreaking>().SetDamage(0.3f);
                GetComponent<AIPath>().maxSpeed = 5f;
                break;
            case EnemyType.Corn:
                GetComponent<Health>().SetMaxHealth(60);
                GetComponent<EnemyBreaking>().SetBreakSpeed(6);
                GetComponent<EnemyBreaking>().SetDamage(2);
                GetComponent<AIPath>().maxSpeed = 2f;
                break;
            case EnemyType.Cauliflower:
                GetComponent<Health>().SetMaxHealth(10);
                GetComponent<EnemyBreaking>().SetBreakSpeed(5);
                GetComponent<EnemyBreaking>().SetDamage(5);
                GetComponent<AIPath>().maxSpeed = 2.5f;
                break;
            case EnemyType.Eggplant:
                GetComponent<Health>().SetMaxHealth(86);
                GetComponent<EnemyBreaking>().SetBreakSpeed(6);
                GetComponent<EnemyBreaking>().SetDamage(1);
                GetComponent<AIPath>().maxSpeed = 3f;
                break;
            case EnemyType.Tomato:
                GetComponent<Health>().SetMaxHealth(2);
                GetComponent<EnemyBreaking>().SetBreakSpeed(1);
                GetComponent<EnemyBreaking>().SetDamage(20);
                GetComponent<AIPath>().maxSpeed = 0.5f;
                break;
        }
    }

    private void SetMeleeProperties()
    {
        // Set properties specific to melee enemies
    }

    private void SetRangedProperties()
    {
        // Set properties specific to ranged enemies
    }

    private void SetBossProperties()
    {
        // Set properties specific to boss enemies
    }
}

public enum EnemyType
{
    Broccoli,
    Carrot,
    Corn,
    Cauliflower,
    Eggplant,
    Tomato
}
