using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteManager
{
  public static Sprite GetGrid(CellType cellType, CellState cellState)
  {
    if (cellType == CellType.Plant)
    {
      return Resources.Load<Sprite>($"Sprites/Grid/Plants/{cellState}");
    }
    else if (cellType == CellType.Structure)
    {
      return Resources.Load<Sprite>($"Sprites/Grid/Structures/{cellState}");
    }
    else
    {
      return Resources.Load<Sprite>("Sprites/Grid/Shadow");
    }
  }
}

