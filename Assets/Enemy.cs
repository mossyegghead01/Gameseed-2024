using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyType type = EnemyType.Broccoli;
    private GameObject eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
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
        var difficulty = getDifficulty();
        switch (type)
        {
            case EnemyType.Broccoli:
                GetComponent<Health>().SetMaxHealth(100 * difficulty);
                GetComponent<EnemyBreaking>().SetBreakSpeed(15f * difficulty);
                GetComponent<EnemyBreaking>().SetDamage(0.3f * difficulty);
                GetComponent<AIPath>().maxSpeed = Mathf.Clamp(1f * Mathf.Clamp(difficulty * 0.25f, 1, Mathf.Infinity), 1f, 5f);
                break;
            case EnemyType.Carrot:
                GetComponent<Health>().SetMaxHealth(50 * difficulty);
                GetComponent<EnemyBreaking>().SetBreakSpeed(3 * difficulty);
                GetComponent<EnemyBreaking>().SetDamage(0.3f * difficulty);
                GetComponent<AIPath>().maxSpeed = Mathf.Clamp(4f * Mathf.Clamp(difficulty * 0.25f, 1, Mathf.Infinity), 1f, 5f);
                break;
            case EnemyType.Corn:
                GetComponent<Health>().SetMaxHealth(60 * difficulty);
                GetComponent<EnemyBreaking>().SetBreakSpeed(6 * difficulty);
                GetComponent<EnemyBreaking>().SetDamage(2 * difficulty);
                GetComponent<AIPath>().maxSpeed = Mathf.Clamp(2f * Mathf.Clamp(difficulty * 0.25f, 1, Mathf.Infinity), 1f, 5f);
                break;
            case EnemyType.Cauliflower:
                GetComponent<Health>().SetMaxHealth(10 * difficulty);
                GetComponent<EnemyBreaking>().SetBreakSpeed(5 * difficulty);
                GetComponent<EnemyBreaking>().SetDamage(5 * difficulty);
                GetComponent<AIPath>().maxSpeed = Mathf.Clamp(2.5f * Mathf.Clamp(difficulty * 0.25f, 1, Mathf.Infinity), 1f, 5f);
                break;
            case EnemyType.Eggplant:
                GetComponent<Health>().SetMaxHealth(86 * difficulty);
                GetComponent<EnemyBreaking>().SetBreakSpeed(6 * difficulty);
                GetComponent<EnemyBreaking>().SetDamage(1 * difficulty);
                GetComponent<AIPath>().maxSpeed = Mathf.Clamp(3f * Mathf.Clamp(difficulty * 0.25f, 1, Mathf.Infinity), 1f, 5f);
                break;
            case EnemyType.Tomato:
                GetComponent<Health>().SetMaxHealth(2 * difficulty);
                GetComponent<EnemyBreaking>().SetBreakSpeed(1 * difficulty);
                GetComponent<EnemyBreaking>().SetDamage(20 * difficulty);
                GetComponent<AIPath>().maxSpeed = Mathf.Clamp(0.5f * Mathf.Clamp(difficulty * 0.25f, 1, Mathf.Infinity), 1f, 55f);
                break;
        }
    }

    private float getDifficulty()
    {
        eventSystem = GameObject.Find("EventSystem");
        return ScalingFunctions.EnemyScalling(eventSystem.GetComponent<UIHandlers>().GetScore());
    }

    public EnemyType GetEnemyType()
    {

        return type;
    }

    public static class Functions
    {
        private static Dictionary<EnemyType, SlotState> enemyAndSlotState = new Dictionary<EnemyType, SlotState>{
            {EnemyType.Broccoli, SlotState.Broccoli},
            {EnemyType.Carrot, SlotState.Carrot},
            {EnemyType.Corn, SlotState.Corn},
            {EnemyType.Cauliflower, SlotState.Cauliflower},
            {EnemyType.Eggplant, SlotState.Eggplant},
            {EnemyType.Tomato, SlotState.Tomato}
        };
        public static SlotState EnemyToSlotState(EnemyType enemyType)
        {
            return enemyAndSlotState[enemyType];
        }
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
