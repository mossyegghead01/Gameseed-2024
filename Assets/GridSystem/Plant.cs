using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant
{
    private Sprite carrot, corn;
    private PlantType plantType;
    public Plant(PlantType plantType)
    {
        this.plantType = plantType;
    }

    public void Harvest()
    {
        //insert gacha code
    }
    public int GetMaxHealth()
    {
        return 1;
    }

    public Sprite GetSprite()
    {
        switch (plantType)
        {
            case PlantType.Carrot:
                return Resources.Load<Sprite>("Assets/Sprites/Grid/Plants/Carrot");
            case PlantType.Corn:
                return Resources.Load<Sprite>("Assets/Sprites/Grid/Plants/Corn");
            default:
                Debug.LogWarning("Unknown plant type: " + plantType);
                return null;
        }

    }
}

public enum PlantType
{
    Carrot,
    Corn
}
