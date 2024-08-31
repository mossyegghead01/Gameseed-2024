using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBreaking : MonoBehaviour
{
    public float playerDamage = 1, breakSpeed = 3f, playerDamageMultiplier = 0.01f, breakSpeedMultiplier = 0.01f;
    private GameObject player, tilemap, gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<GameManager>().GetPlayer();
        tilemap = gameManager.GetComponent<GameManager>().GetTilemap();
        GetComponent<AIDestinationSetter>().target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {

        // Get the enemy's collider
        Collider2D enemyCollider = GetComponent<Collider2D>();

        // Check for collision with tilemap
        if (enemyCollider.IsTouching(tilemap.GetComponent<CompositeCollider2D>()))
        {
            // Get the contact points
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            enemyCollider.GetContacts(contacts);

            // Log the position of intersection
            if (contacts.Length > 0)
            {
                Vector3 intersectionPoint = contacts[0].point;
                gameManager.GetComponent<GridManager>().GetGrid().BreakCell(breakSpeed * breakSpeedMultiplier, intersectionPoint + new Vector3(0, 0.01f, 0));
            }
        }

        if (enemyCollider.IsTouching(player.GetComponent<Collider2D>()))
        {
            gameManager.GetComponent<GameManager>().GetPlayer().GetComponent<Health>().ModifyHealth(-playerDamage * playerDamageMultiplier);
        }
    }
}