using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
            plant = new Plant(this);
            Debug.Log(plant);
        }
    }
    public Plant getPlant()
    {
        return plant;
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
    }

    public void SetTile(TileBase tileBase)
    {
        tilemap.SetTile(position, tileBase);
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
                switch (cellState) { 
                    case CellState.Carrot:
                        EventSystem.current.GetComponent<WeaponRolling>().Roll(WeaponRolling.WeaponPlantType.Carrot);
                        break;
                    case CellState.Corn:
                        EventSystem.current.GetComponent<WeaponRolling>().Roll(WeaponRolling.WeaponPlantType.Corn);
                        break;
                    case CellState.Eggplant:
                        EventSystem.current.GetComponent<WeaponRolling>().Roll(WeaponRolling.WeaponPlantType.Eggplant);
                        break;
                    case CellState.Cauliflower:
                        EventSystem.current.GetComponent<WeaponRolling>().Roll(WeaponRolling.WeaponPlantType.Cauliflower);
                        break;
                    case CellState.Tomato:
                        EventSystem.current.GetComponent<WeaponRolling>().Roll(WeaponRolling.WeaponPlantType.Tomato);
                        break;
                    case CellState.Broccoli:
                        EventSystem.current.GetComponent<WeaponRolling>().Roll(WeaponRolling.WeaponPlantType.Broccoli);
                        break;
                }
                
            }
            Debug.Log("not harvest");
            SetCell(CellState.Empty);
            plant = null;
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
    Post,
    Eggplant,
    Cauliflower,
    Tomato,
    Broccoli,
}

public static class CellFunctions
{
    public static CellState[] plant = {
        CellState.Carrot,
        CellState.Corn,
        CellState.Eggplant,
        CellState.Cauliflower,
        CellState.Tomato,
        CellState.Broccoli
    };
    public static CellState[] structure = {
        CellState.Fence,
        CellState.Wall,
        CellState.ReinforcedWall,
        CellState.Concrete,
        CellState.ReinforcedConcrete,
        CellState.Post,

    };
    public static Dictionary<CellState, float> health = new Dictionary<CellState, float>{
        {CellState.Empty,-1},
        {CellState.Fence, 3},
        {CellState.Carrot, 1},
        {CellState.Corn, 1},
        {CellState.Wall, 5},
        {CellState.ReinforcedWall, 10},
        {CellState.Concrete, 20},
        {CellState.ReinforcedConcrete, 35},
        {CellState.Post, 2},
        {CellState.Eggplant, 1},
        {CellState.Cauliflower, 1},
        {CellState.Tomato, 1},
        {CellState.Broccoli, 1},

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