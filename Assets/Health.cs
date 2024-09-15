using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Health : MonoBehaviour
{
    // Object Health
    public float health = 100;
    // Object Maximum Health
    public float maxHealth = 100;
    // Should object be destroyed after the health ran out?
    public bool destroyAfterDeath = false;
    // Should this object death increment score?
    public bool incrementScoreOnDeath = false;
    public float IncrementMultiplier { private get; set; } = 1;
    public float scoreValue = 1;
    private EnemyType enemyType;
    private GameObject gameManager;
    private UIHandlers uiHandlers;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        // Clamp health
        health = Mathf.Clamp(health, 0, maxHealth);
        enemyType = GetComponent<Enemy>().GetEnemyType();
        uiHandlers = gameManager.GetComponent<GameManager>().GetEventSystem().GetComponent<UIHandlers>();
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        // Still clamping health
        if (health <= 0)
        {
            if (incrementScoreOnDeath)
            {
                if (Random.Range(0, 100) <= 50) gameManager.GetComponent<GridManager>().GetBuildInventory().AddSlot(Enemy.Functions.EnemyToSlotState(enemyType));
                var score = uiHandlers.GetScore();
                var random = System.Math.Clamp(Random.Range(0, 100) * score / 200, 0, 100);
                Debug.Log(random);
                if (random <= 20)
                    AddSlot(SlotState.Post);
                else if (random <= 40)
                    AddSlot(SlotState.Fence);
                else if (random <= 70)
                    AddSlot(SlotState.Wall);
                else if (random <= 85)
                    AddSlot(SlotState.ReinforcedWall);
                else if (random <= 90)
                    AddSlot(SlotState.Concrete);
                else if (random <= 95)
                    AddSlot(SlotState.ReinforcedConcrete);
                EventSystem.current.GetComponent<UIHandlers>().IncrementScore(scoreValue * IncrementMultiplier);
            }
            if (destroyAfterDeath)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void AddSlot(SlotState slotState)
    {
        gameManager.GetComponent<GridManager>().GetBuildInventory().AddSlot(slotState);
    }

    public float GetHealth()
    {
        return health;
    }
    public void SetHealth(float health)
    {
        this.health = health;
    }
    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }
    public void ModifyHealth(float modHealth)
    {
        health += modHealth;
    }
}
