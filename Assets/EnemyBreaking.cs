using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBreaking : MonoBehaviour
{
    public int breakSpeed;
    public GameObject player, tilemap, gridManager;
    // Start is called before the first frame update
    void Start()
    {

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
                gridManager.GetComponent<GridManager>().GetGrid().BreakCell(breakSpeed, intersectionPoint);
                Debug.Log("Collision with tilemap at position: " + intersectionPoint);
            }
        }

        if (enemyCollider.IsTouching(player.GetComponent<Collider2D>()))
        {
            Debug.Log("Hit Player");
        }
    }
}
