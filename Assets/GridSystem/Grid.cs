using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Grid
{
    private Dictionary<(int, int), Cell> cells;
    private Dictionary<(int, int), bool> lightCells;
    private Tilemap tilemap;
    private BuildInventory buildInventory;
    private GameObject gameManager;
    private TileBase backgroundTile;
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
    public Cell GetCell(Vector3Int position)
    {
        cells.TryGetValue((position.x, position.y), out Cell res);
        return res;
    }
    public Cell GetCell(Vector3 mousePosition)
    {
        return GetCell(tilemap.WorldToCell(mousePosition));
    }
    public Tilemap GetTilemap()
    {
        return tilemap;
    }

    public Grid(Tilemap tilemap, BuildInventory buildInventory, GameObject gameManager)
    {
        this.gameManager = gameManager;
        this.buildInventory = buildInventory;
        this.tilemap = tilemap;
        backgroundTile = Resources.Load<TileBase>("Tilemap/Tiles/Background");
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
        var res = GetCell(position);
        // lightCells = ObeliskFunctions.GetObeliskCoordinates(gameManager.GetComponent<GameManager>().GetObelisk().GetComponent<obeliskScript>().GetStage());
        var obelisk = gameManager.GetComponent<GameManager>().GetObelisk();
        if (obelisk != null)
            lightCells = obelisk.GetComponent<obeliskScript>().GetObeliskCoordinates();
        if (res == null || cells[(position.x, position.y)].GetCellState() == CellState.Empty)
        {
            if (obelisk != null && CellFunctions.plant.Contains(cellState))
            {
                if (lightCells.ContainsKey((position.x, position.y)) && lightCells[(position.x, position.y)] == true)
                {

                    cells[(position.x, position.y)] = new Cell(position, cellState, this);
                    gameManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/build"));
                    Debug.Log("new cell");
                    buildInventory.SubtractSlot(BuildInventoryFunctions.SlotToIndex(BuildInventoryFunctions.CellToSlot(cellState), buildInventory.GetSlots()));
                }
                else
                {
                    Debug.Log("not light");
                    gameManager.GetComponent<GameManager>().GetPlayer().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/failBuild"));
                }
            }
            else if (CellFunctions.structure.Contains(cellState))
            {
                cells[(position.x, position.y)] = new Cell(position, cellState, this);
                Debug.Log("new cell");
                gameManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/build"));
                buildInventory.SubtractSlot(BuildInventoryFunctions.SlotToIndex(BuildInventoryFunctions.CellToSlot(cellState), buildInventory.GetSlots()));
            }
            else
                gameManager.GetComponent<GameManager>().GetPlayer().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/failBuild"));
        }
    }
    public void BuildCell(CellState cellState, Vector3 mousePosition)
    {
        SetCell(cellState, mousePosition);
    }
    public void SetCell(CellState cellState, Vector3 mousePosition)
    {
        SetCell(tilemap.WorldToCell(mousePosition), cellState);
    }
    public void BreakCell(float damage, Vector3Int position)
    {
        cells.TryGetValue((position.x, position.y), out Cell res);
        if (res != null)
        {
            res.Break(damage);
        }
    }
    public void BreakCell(float damage, Vector3 position)
    {
        BreakCell(damage, tilemap.WorldToCell(position));
    }
}