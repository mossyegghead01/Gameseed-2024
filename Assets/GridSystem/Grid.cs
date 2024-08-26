using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Grid
{
    public GameObject gridContainer;
    private Cell[,] cell;
    private int width, height, cellSize;
    private int[,] gridArray;
    public Grid(int width, int height, int cellSize, GameObject cellObject, GameObject gridContainer)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridContainer = gridContainer;

        gridArray = new int[width, height];
        cell = new Cell[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                cell[x, y] = new Cell(x, y, this);
                // spawnedTile.GetComponent<SpriteRenderer>().sprite = cell[x, y].GetSprite();
            }
        }
    }
    public int GetCellSize()
    {
        return cellSize;
    }

    public GameObject GetGridContainer()
    {
        return gridContainer;
    }

    private void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
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

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

}