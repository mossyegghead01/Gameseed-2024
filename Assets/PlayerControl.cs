using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Basic player control
    // Movements still suck

    // Movement Speed for the player
    public float movementSpeed = 5;
    // Gun Object
    public GunProperty gun;

    // Internal Values
    // Input axis
    private float inputX;
    private float inputY;
    // Where the player should look (where its gun points
    private Vector2 lookVector;
    // Gun Pivot
    private Transform pivotObject;

    void Start()
    {
        pivotObject = gameObject.transform.GetChild(0);
    }

    void Update()
    {
        // Input axis
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        // Mouse position in world
        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0;
        
        // Where the player should look (where their gun should point)
        lookVector = worldMousePos - pivotObject.position;

        // Shooting
        if (Input.GetButton("Fire1"))
        {
            gun.Fire();
        }
    }

    void FixedUpdate()
    {
        // Move the player
        GetComponent<Rigidbody2D>().velocity = new Vector3(inputX * movementSpeed, inputY * movementSpeed);
        // Rotate the weapon
        pivotObject.eulerAngles = new Vector3(0, 0, -(Mathf.Atan2(lookVector.x, lookVector.y) * Mathf.Rad2Deg - 90f));
    }
}
