using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell
{
    private CellType cellType = CellType.Empty;
    public int x, y, cellSize;
    public float health, maxHealth;
    private float lightLevel;
    private bool isLit, canCollide;
    private CellState cellState;
    private Vector3Int position;
    private Tilemap tilemap;
    private BuildInventory buildInventory;
    private Plant plant;
    private Grid grid;

    public Cell(Vector3Int position, CellState cellState, Grid grid)
    {
        this.grid = grid;
        buildInventory = grid.GetBuildInventory();
        this.position = position;
        tilemap = grid.GetTilemap();
        SetCell(cellState);
        if (CellFunctions.GetCellType(cellState) == CellType.Plant)
        {
            plant = new Plant(cellState);
        }
    }

    private TileBase GetTile(CellState cellState)
    {
        TileBase tileBase = Resources.Load($"Tilemap/Tiles/{cellState}") as TileBase;
        return tileBase;
    }
    public CellState GetCellState()
    {
        return cellState;
    }

    public void SetCell(CellState cellState)
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
        var bounds = new GraphUpdateObject(grid.GetTilemap().GetComponent<CompositeCollider2D>().bounds);
        bounds.updatePhysics = true;
        AstarPath.active.UpdateGraphs(bounds);

        tilemap.SetTile(position, GetTile(cellState));
        // var graphToScan = AstarPath.active.data.gridGraph;
        // AstarPath.active.Scan(graphToScan);
    }



    public void Break(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (cellType == CellType.Plant && plant.IsGrown())
            {
                Debug.Log("harvest");
                // * GACHA CODE HERE, make new class pls
            }
            Debug.Log("not harvest");
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
    Wall,
    ReinforcedWall,
    Concrete,
    ReinforcedConcrete,
    Post
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
    public static Dictionary<CellState, float> health = new Dictionary<CellState, float>{
        {CellState.Empty,-1},
        {CellState.Fence, 3},
        {CellState.Carrot, 1},
        {CellState.Corn, 1},
        {CellState.Wall, 5}
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
    public static float GetMaxHealth(CellState cellState)
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