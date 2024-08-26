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
    private GameObject cellObject;

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
        UpdateObject();
    }
    public void setCell(CellType cellType, CellState cellState)
    {
        this.cellType = cellType;
        this.cellState = cellState;
        UpdateObject();
    }

    private void UpdateObject()
    {
        if (cellObject == null)
        {

            Debug.Log("Cell Object is null");
            var cellPrefab = Resources.Load<GameObject>("Prefabs/Cell");
            if (cellPrefab != null)
            {
                cellObject = GameObject.Instantiate(cellPrefab, GetWorldPosition(x, y) + new Vector3(cellSize * 0.5f, cellSize * 0.5f), Quaternion.identity);
                cellObject.GetComponent<SpriteRenderer>().sprite = GetSprite();
                cellObject.transform.parent = gridContainer.transform;
                cellObject.GetComponent<CellObject>().x = x;
                cellObject.GetComponent<CellObject>().y = y;
            }
            else
            {
                Debug.Log("File Not Found");
            }
        }
        else
        {
            Debug.Log("Cell Object is not null");
            Sprite sprite = GetSprite();
            if (sprite != null)
            {
                Debug.Log("CellObject" + cellObject.GetComponent<CellObject>().x + " " + cellObject.GetComponent<CellObject>().y);
                cellObject.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {
                Debug.Log("Sprite is null");
            }
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

    private Sprite GetSprite()
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