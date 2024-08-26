using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager
{
  private Dictionary<string, Sprite> spriteDict;

  public SpriteManager()
  {
    spriteDict = new Dictionary<string, Sprite>();
    Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");
    foreach (Sprite sprite in sprites)
    {
      spriteDict.Add(sprite.name, sprite);
    }
  }
  public Sprite GetSprite(string name)
  {
    return spriteDict[name];
  }
}

