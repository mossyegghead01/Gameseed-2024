using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private CellType cellType = CellType.Empty;
    public int health, maxHealth, x, y, cellSize;
    private float lightLevel;
    private bool isLit, canCollide;
    private Sprite sprite;
    private GameObject gridContainer;

    public Cell(CellType cellType, PlantType plantType, int x, int y, GameObject gridContainer, int cellSize)
    {
        this.cellType = cellType;
        if (cellType == CellType.Plant)
        {
            // Plant plant = new Plant(plantType);
            // maxHealth = plant.GetMaxHealth();
            // sprite = plant.GetSprite();
            StartCell(x, y, gridContainer, cellSize);
        }
    }
    public Cell(CellType cellType, StructureType structureType, int x, int y, GameObject gridContainer, int cellSize)
    {
        this.cellType = cellType;
        if (cellType == CellType.Structure)
        {
            // Structure structure = new Structure(structureType);
            // maxHealth = structure.GetMaxHealth();
            StartCell(x, y, gridContainer, cellSize);
        }
    }

    public Cell(int x, int y, GameObject gridContainer, int cellSize)
    {
        if (cellType == CellType.Empty)
        {
            maxHealth = 1;
        }
        StartCell(x, y, gridContainer, cellSize);
    }

    private void StartCell(int x, int y, GameObject gridContainer, int cellSize)
    {
        this.gridContainer = gridContainer;
        health = maxHealth;
        this.x = x;
        this.y = y;
        this.cellSize = cellSize;
        Instantiate();
    }

    private void Instantiate()
    {
        GameObject cellObject = Resources.Load<GameObject>("Prefabs/Cell");
        if (cellObject != null)
        {
            var spawnedTile = GameObject.Instantiate(cellObject, GetWorldPosition(x, y), Quaternion.identity);
            spawnedTile.transform.parent = gridContainer.transform;
            spawnedTile.GetComponent<CellObject>().x = x;
            spawnedTile.GetComponent<CellObject>().y = y;
        }
        else
        {
            Debug.Log("Cell Object is null");
        }
    }

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

    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Square");
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }
}
public enum CellType
{
    Empty,
    Plant,
    Structure
}

public enum PlantType
{
    Carrot,
    Corn
}

public enum StructureType
{
    Fence,
    Wall
}