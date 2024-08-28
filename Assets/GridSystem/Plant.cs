using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant
{
    private int growTime;
    private CellState cellState;
    private bool isGrown = false;
    public Plant(CellState cellState)
    {
        this.cellState = cellState;
        growTime = PlantFunctions.GetGrowTime(cellState);
        Grow();
    }
    public async System.Threading.Tasks.Task Grow()
    {
        if (growTime != -1)
        {
            await System.Threading.Tasks.Task.Delay(growTime * 1000);
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
            default:
                return -1;
        }
    }
}
