using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Runtime.InteropServices;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern short GetKeyState(int keyCode);
    private Grid grid;
    private AstarPath astarPath;
    private BuildInventory buildInventory;
    [SerializeField] GameObject cellObject, cursorObject;
    [SerializeField] Tilemap tilemap;
    private Sprite cursorBuild, cursorBreak, cursorHarvest;
    private Vector3Int currentXY;
    private bool isCaps = false;
    [SerializeField] private float lerpSpeed = 15f;
    [SerializeField] private GameObject gameManager;
    private Vector3 targetPosition;
    private bool isMoving = false;
    [SerializeField] private GameObject buildStateImage;
    private Sprite buildImage, breakImage;
    // Start is called before the first frame update
    void Start()
    {
        buildInventory = new BuildInventory();
        grid = new Grid(tilemap, buildInventory, gameManager);
        Debug.Log("grid is" + grid);
        isCaps = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
        cursorBuild = Resources.Load<Sprite>("Sprites/cursorBuild");
        cursorBreak = Resources.Load<Sprite>("Sprites/cursorBreak");
        cursorHarvest = Resources.Load<Sprite>("Sprites/cursorHarvest");
        buildImage = Resources.Load<Sprite>("Sprites/ui/build");
        breakImage = Resources.Load<Sprite>("Sprites/ui/break");
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

        if (isCaps)
            buildStateImage.GetComponent<Image>().sprite = breakImage;
        else
            buildStateImage.GetComponent<Image>().sprite = buildImage;

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
            var cell = grid.GetCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (cell != null && CellFunctions.plant.Contains(cell.GetCellState()) && cell.getPlant().GetGrowStage() == 3)
                cursorObject.GetComponent<SpriteRenderer>().sprite = cursorHarvest;
            else
                cursorObject.GetComponent<SpriteRenderer>().sprite = cursorBreak;
        }
        else if (!isCaps)
        {
            // var position = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            // var lightCells = gameManager.GetComponent<GameManager>().GetObelisk().GetComponent<obeliskScript>().GetObeliskCoordinates();
            // if (CellFunctions.plant.Contains(BuildInventoryFunctions.SlotToCell(buildInventory.GetSelectedSlot().GetSlotState())) && lightCells.ContainsKey((position.x, position.y)) && lightCells[(position.x, position.y)] == true)
            //     cursorObject.GetComponent<SpriteRenderer>().sprite = cursorBreak;
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

    public BuildInventory GetBuildInventory()
    {
        return buildInventory;
    }
}
