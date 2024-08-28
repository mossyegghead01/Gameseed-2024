using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
public class GridManager : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern short GetKeyState(int keyCode);
    private Grid grid;
    private BuildInventory buildInventory;
    [SerializeField] GameObject cellObject, cursorObject;
    [SerializeField] Tilemap tilemap;
    private Sprite cursorBuild, cursorBreak;
    private Vector3Int currentXY;
    private bool isCaps = false;
    [SerializeField] private float lerpSpeed = 15f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        buildInventory = new BuildInventory();
        grid = new Grid(tilemap, buildInventory);
        isCaps = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
        cursorBuild = Resources.Load<Sprite>("Sprites/cursorBuild");
        cursorBreak = Resources.Load<Sprite>("Sprites/cursorBreak");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            isCaps = !isCaps;
            buildInventory.ToggleBuildState(isCaps);
        }
        if (Input.GetMouseButton(1))
        {
            if (!isCaps)
            {
                if (buildInventory.GetSelectedSlot() != null)
                {

                    grid.BuildCell(BuildInventoryFunctions.SlotToCell(buildInventory.GetSelectedSlot().GetSlotState()), Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isCaps)
            {
                grid.BreakCell(2, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Debug.Log("break");
            }
        }

        var XY = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (XY != currentXY)
        {
            currentXY = XY;
            targetPosition = tilemap.GetCellCenterWorld(XY);
            isMoving = true;
        }

        if (isMoving)
        {
            cursorObject.transform.position = Vector3.Lerp(cursorObject.transform.position, targetPosition, lerpSpeed * Time.deltaTime);

            // Check if the cursor is close enough to the target position
            if (Vector3.Distance(cursorObject.transform.position, targetPosition) < 0.01f)
            {
                cursorObject.transform.position = targetPosition;
                isMoving = false;
            }
        }

        if (isCaps)
        {
            cursorObject.GetComponent<SpriteRenderer>().sprite = cursorBreak;
        }
        else if (!isCaps)
        {
            cursorObject.GetComponent<SpriteRenderer>().sprite = cursorBuild;
        }
    }


    public enum ToolCategory
    {
        Weapon,
        Build,
        Break
    }

    public Grid GetGrid()
    {
        return grid;
    }
}
