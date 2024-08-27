using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell
{
    private CellType cellType = CellType.Empty;
    public int health, maxHealth, x, y, cellSize;
    private float lightLevel;
    private bool isLit, canCollide;
    private CellState cellState;
    private Vector3Int position;
    private Tilemap tilemap;

    public Cell(CellType cellType, CellState cellState, int x, int y, Grid grid)
    {
        this.cellType = cellType;
        if (cellType == CellType.Structure)
        {

        }
        else if (cellType == CellType.Plant)
        {
            maxHealth = 1;
        }
        else
        {
            maxHealth = -1;
        }
        StartCell(x, y, grid);
    }
    public Cell(Vector3Int position, CellState cellState, Grid grid)
    {
        this.position = position;
        tilemap = grid.GetTilemap();
        SetCell(cellState);
    }



    private TileBase GetTile(CellState cellState)
    {
        TileBase tileBase = Resources.Load($"Tilemap/Tiles/{cellState}") as TileBase;
        Debug.Log(tileBase);
        return tileBase;
    }

    private void StartCell(int x, int y, Grid grid)
    {
        // gridContainer = grid.GetGridContainer();
        // health = maxHealth;
        // this.x = x;
        // this.y = y;
        // cellSize = grid.GetCellSize();
        // UpdateObject();
    }
    public void SetCell(CellState cellState)
    {
        Debug.Log(cellState + " " + position);
        this.cellState = cellState;
        cellType = CellFunctions.GetCellType(cellState);
        // UpdateObject();
        tilemap.SetTile(position, GetTile(cellState));
    }

    // private void UpdateObject()
    // {
    //     if (cellObject == null)
    //     {

    //         Debug.Log("Cell Object is null");
    //         var cellPrefab = Resources.Load<GameObject>("Prefabs/Cell");
    //         if (cellPrefab != null)
    //         {
    //             cellObject = GameObject.Instantiate(cellPrefab, GridFunctions.GetWorldPosition(x, y, cellSize) + new Vector3(cellSize * 0.5f, cellSize * 0.5f), Quaternion.identity);
    //             cellObject.GetComponent<SpriteRenderer>().sprite = GetSprite();
    //             cellObject.transform.parent = gridContainer.transform;
    //             cellObject.GetComponent<CellObject>().x = x;
    //             cellObject.GetComponent<CellObject>().y = y;
    //         }
    //         else
    //         {
    //             Debug.Log("File Not Found");
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("Cell Object is not null");
    //         Sprite sprite = GetSprite();
    //         if (sprite != null)
    //         {
    //             Debug.Log("CellObject" + cellObject.GetComponent<CellObject>().x + " " + cellObject.GetComponent<CellObject>().y);
    //             cellObject.GetComponent<SpriteRenderer>().sprite = sprite;
    //         }
    //         else
    //         {
    //             Debug.Log("Sprite is null");
    //         }
    //     }
    // }

    public void Break(int damage)
    {
        if (health > 0)
        {
            health -= damage;

        }
    }

    public void Heal(int health)
    {
        this.health += health;
    }

    private Sprite GetSprite()
    {
        return SpriteManager.GetGrid(cellType, cellState);
    }

}
public enum CellType
{
    Empty,
    Plant,
    Structure
}

public enum CellState
{
    Empty,
    Carrot,
    Corn,
    Fence,
    Wall
}

public static class CellFunctions
{
    public static CellState[] plant = {
        CellState.Carrot,
        CellState.Corn
    };
    public static CellState[] structure = {
        CellState.Fence,
        CellState.Wall
    };
    public static CellType GetCellType(CellState cellState)
    {
        if (plant.Contains(cellState))
        {
            return CellType.Plant;
        }
        if (structure.Contains(cellState))
        {
            return CellType.Structure;
        }
        else return CellType.Empty;
    }
}