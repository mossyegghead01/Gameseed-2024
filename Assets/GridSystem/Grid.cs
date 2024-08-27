using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid
{
    private Dictionary<(int, int), Cell> cells;
    private Tilemap tilemap;
    private BuildInventory buildInventory;
    // public Grid(int width, int height, int cellSize, GameObject cellObject, GameObject gridContainer)
    // {

    //     gridArray = new int[width, height];
    //     cell = new Cell[width, height];

    //     for (int x = 0; x < gridArray.GetLength(0); x++)
    //     {
    //         for (int y = 0; y < gridArray.GetLength(1); y++)
    //         {
    //             cell[x, y] = new Cell(CellType.Empty, CellState.Empty, x, y, this);
    //             // spawnedTile.GetComponent<SpriteRenderer>().sprite = cell[x, y].GetSprite();
    //         }
    //     }
    // }
    public Tilemap GetTilemap()
    {
        return tilemap;
    }

    public Grid(Tilemap tilemap, BuildInventory buildInventory)
    {
        this.tilemap = tilemap;
        cells = new Dictionary<(int, int), Cell>();
    }


    // public void SetCell(int x, int y, CellType cellType, CellState cellState)
    // {
    //     if (x >= 0 && y >= 0 && x < width && y < height)
    //     {
    //         Debug.Log(x + " " + y);
    //         cell[x, y].SetCell(CellType.Structure, CellState.Wall);
    //     }
    // }
    public BuildInventory GetBuildInventory()
    {
        return buildInventory;
    }
    public void SetCell(Vector3Int position, CellState cellState)
    {
        cells.TryGetValue((position.x, position.y), out Cell res);
        if (res == null)
        {
            cells[(position.x, position.y)] = new Cell(position, cellState, this);
        }
        else
        {
            cells[(position.x, position.y)].SetCell(cellState);
        }
    }

    public void SetCell(CellState cellState, Vector3 mousePosition)
    {
        SetCell(tilemap.WorldToCell(mousePosition), cellState);
    }
    public void BreakCell(int damage, Vector3Int position)
    {
        cells.TryGetValue((position.x, position.y), out Cell res);
        if (res != null)
        {
            res.Break(damage);
        }
    }
    public void BreakCell(int damage, Vector3 position)
    {
        BreakCell(damage, tilemap.WorldToCell(position));
    }
}