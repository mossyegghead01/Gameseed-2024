using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    private BuildInventory buildInventory;

    public Cell(Vector3Int position, CellState cellState, Grid grid)
    {
        buildInventory = grid.GetBuildInventory();
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

    public void SetCell(CellState cellState)
    {
        if (this.cellState != cellState)
        {

            maxHealth = CellFunctions.GetMaxHealth(cellState);
            if (maxHealth != -1)
            {
                health = maxHealth;
            }
            else
            {
                health = 0;
            }
            this.cellState = cellState;
            cellType = CellFunctions.GetCellType(cellState);
            tilemap.SetTile(position, GetTile(cellState));
            buildInventory.SubtractSlot(BuildInventoryFunctions.SlotToIndex(BuildInventoryFunctions.CellToSlot(cellState), buildInventory.GetSlots()));
        }
    }


    public void Break(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
        else
        {
            if (cellType == CellType.Plant)
            {
                // * GACHA CODE HERE, make new class pls
            }
            SetCell(CellState.Empty);
        }
    }

    public void Heal(int health)
    {
        this.health += health;
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
    public static Dictionary<CellState, int> health = new Dictionary<CellState, int>{
        {CellState.Empty,-1},
        {CellState.Fence, 3},
        {CellState.Carrot, 1},
        {CellState.Corn, 1},
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
    public static int GetMaxHealth(CellState cellState)
    {
        if (health.ContainsKey(cellState))
        {
            return health[cellState];
        }
        else
        {
            return -1;
        }
    }
}