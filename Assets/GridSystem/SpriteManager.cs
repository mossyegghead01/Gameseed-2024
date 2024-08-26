using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteManager
{
  public static Sprite GetGridPlant(PlantType name)
  {
    return Resources.Load<Sprite>($"Sprites/Grid/Plants/{name}");
  }
  public static Sprite GetGridStructure(StructureType name)
  {
    return Resources.Load<Sprite>($"Sprites/Grid/Structure/{name}");
  }
  public static Sprite GetGridEmpty()
  {
    return Resources.Load<Sprite>("Sprites/Grid/Shadow");
  }
}

