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
    private CellState cellState;

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

    private void StartCell(int x, int y, Grid grid)
    {
        gridContainer = grid.GetGridContainer();
        health = maxHealth;
        this.x = x;
        this.y = y;
        cellSize = grid.GetCellSize();
        Instantiate();
    }


    private void Instantiate()
    {
        GameObject cellObject = Resources.Load<GameObject>("Prefabs/Cell");
        if (cellObject != null)
        {
            var spawnedTile = GameObject.Instantiate(cellObject, GetWorldPosition(x, y), Quaternion.identity);
            spawnedTile.GetComponent<SpriteRenderer>().sprite = GetSprite();
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
        return SpriteManager.GetGrid(cellType, cellState);
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

public enum CellState
{
    Empty,
    Carrot,
    Corn,
    Fence,
    Wall
}