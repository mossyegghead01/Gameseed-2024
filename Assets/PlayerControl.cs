using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        // TODO: Fix issue where clickin on UI component still register
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            if (pivotObject.GetChild(0).TryGetComponent<GunProperty>(out var prop))
            {
                prop.Fire();
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (pivotObject.GetChild(0).TryGetComponent<GunProperty>(out var prop))
            {
                prop.EndFire();
            }
        }

        // Inventory Hotkey
        for (int i = 1; i <= 3; i++)
        {
            if (Input.GetButtonDown("Num" + i))
            {
                EventSystem.current.GetComponent<UIHandlers>().InventoryButtonClicked(i - 1);
                break;
            }
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
