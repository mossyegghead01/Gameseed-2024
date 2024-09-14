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
    public float scrollSensitivity = 10;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    // Internal Values
    // Input axis
    private float inputX;
    private float inputY;
    // Where the player should look (where its gun points
    private Vector2 lookVector;
    // Gun Pivot
    private Transform pivotObject;
    private float actualMovementSpeed = 5; private Vector3 lastPosition;
    private Vector3 movementDirection;
    private Vector3 originalCamPos;
    private float shakeDuration = 0f;

    void Start()
    {
        pivotObject = gameObject.transform.GetChild(0);
        EventSystem.current.GetComponent<WeaponRolling>().RollAny(true);
        EventSystem.current.GetComponent<UIHandlers>().InventoryButtonClicked(0);
        Destroy(EventSystem.current.GetComponent<UIHandlers>().inventoryHolder.transform.GetChild(0).gameObject);
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

        GunProperty held = null;
        if (pivotObject.GetChild(0).TryGetComponent<GunProperty>(out var prop))
        {
            held = prop;
            actualMovementSpeed = movementSpeed + prop.movementBonus;
        }
        else
        {
            actualMovementSpeed = movementSpeed;
        }

        // Camera FOV
        if (held != null)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 5, held.weaponRange);
        }
        else
        {
            Camera.main.orthographicSize = 5;
        }

        float inputScroll = Input.GetAxis("Mouse ScrollWheel");
        if (inputScroll != 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - inputScroll * scrollSensitivity, 5, held.weaponRange - (held.weaponRange/10));
            
        }
        Camera.main.transform.position = transform.position + (new Vector3(2.25f, 0, 0) * Camera.main.orthographicSize / 5);
        //originalCamPos = Camera.main.transform.position;

        //if (shakeDuration > 0)
        //{
        //    Camera.main.transform.localPosition = originalCamPos + Random.insideUnitSphere * shakeAmount;

        //    shakeDuration -= Time.deltaTime * decreaseFactor;
        //}
        //else
        //{
        //    shakeDuration = 0f;
        //    Camera.main.transform.localPosition = originalCamPos;
        //}

        // Shooting
        if (Input.GetButtonDown("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            if (held != null)
            {
                held.Fire();
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (held != null)
            {
                held.EndFire();
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

        if (Input.GetButtonDown("Reload"))
        {
            if (held != null)
            {
                held.Reload();
            }
        }
        if ((transform.position - lastPosition).normalized != Vector3.zero)
            movementDirection = (transform.position - lastPosition).normalized;
        Debug.Log(movementDirection.x);
        transform.GetComponent<Animator>().SetFloat("x", movementDirection.x);
        transform.GetComponent<Animator>().SetFloat("y", movementDirection.y);
        lastPosition = transform.position;
    }

    //public void Shake()
    //{
    //    shakeDuration = 0.05f;
    //}

    void FixedUpdate()
    {
        // Move the player
        GetComponent<Rigidbody2D>().velocity = new Vector3(inputX, inputY).normalized * actualMovementSpeed;

        var angle = -(Mathf.Atan2(lookVector.x, lookVector.y) * Mathf.Rad2Deg - 90f);
        var yOverride = 0;
        if (!(angle < 90 && angle > -90))
        {
            yOverride = 180;
            angle = -angle - 180;
        }
        else
        {
            yOverride = 0;
        }

        // Rotate the weapon
        pivotObject.eulerAngles = new Vector3(0, yOverride, angle);

    }
}
