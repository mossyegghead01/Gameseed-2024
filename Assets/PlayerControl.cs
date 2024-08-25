using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Basic player control
    // Movements still suck

    // Movement Speed for the player
    public float movementSpeed = 5;

    private float inputX;
    private float inputY;
    private Vector2 lookVector;
    private Transform pivotObject;

    // Start is called before the first frame update
    void Start()
    {
        pivotObject = gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        
        //gameObject.transform.GetChild(0).Rotate(0, 0, 1);

        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0;
        
        lookVector = worldMousePos - pivotObject.position;
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(inputX * movementSpeed, inputY * movementSpeed);
        pivotObject.eulerAngles = new Vector3(0, 0, -(Mathf.Atan2(lookVector.x, lookVector.y) * Mathf.Rad2Deg - 90f));
        
    }
}
