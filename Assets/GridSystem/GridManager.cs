using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private Grid grid;
    [SerializeField] GameObject cellObject;
    [SerializeField] Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(tilemap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetCell(CellState.Fence, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            print("click");
        }
    }

    public enum ToolCategory
    {
        Weapon,
        Build,
        Break
    }


}
