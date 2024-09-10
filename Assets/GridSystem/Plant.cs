using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Plant
{
    private int growTime;
    private CellState cellState;
    private bool isGrown = false;
    private int growStage = 0;
    private Cell cell;
    public Plant(Cell cell)
    {
        this.cell = cell;
        cellState = cell.GetCellState();
        growTime = PlantFunctions.GetGrowTime(cellState);
        _ = Grow();
    }
    public int GetGrowStage()
    {
        return growStage;
    }
    public async System.Threading.Tasks.Task Grow()
    {
        if (growTime != -1)
        {
            cell.SetTile(Resources.Load($"Tilemap/Tiles/{cellState}_1") as TileBase);
            growStage = 1;
            await System.Threading.Tasks.Task.Delay(growTime * 500);
            if (cell.getPlant() == null || !Application.isPlaying)
            {
                Debug.Log(cell.getPlant());
                return;
            }
            cell.SetTile(Resources.Load($"Tilemap/Tiles/{cellState}_2") as TileBase);
            growStage = 2;
            await System.Threading.Tasks.Task.Delay(growTime * 500);
            if (cell.getPlant() == null || !Application.isPlaying)
            {

                Debug.Log("RETURN");
                return;
            }
            cell.SetTile(Resources.Load($"Tilemap/Tiles/{cellState}_3") as TileBase);
            growStage = 3;
            isGrown = true;
        }
    }
    public bool IsGrown()
    {
        return isGrown;
    }
}

public static class PlantFunctions
{
    public static int GetGrowTime(CellState cellState)
    {
        switch (cellState)
        {
            case CellState.Carrot:
                return 10;
            case CellState.Corn:
                return 15;
            case CellState.Eggplant:
                return 20;
            case CellState.Cauliflower:
                return 40;
            case CellState.Tomato:
                return 120;
            case CellState.Broccoli:
                return 200;
            default:
                return -1;
        }
    }
}
