using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure
{
    private StructureType structureType;
    private int maxHealth;
    public Structure(StructureType structureType)
    {
        this.structureType = structureType;
    }

    public int GetMaxHealth()
    {
        switch (structureType)
        {
            case StructureType.Fence:
                return 100;
            case StructureType.Wall:
                return 500;
        }
        return 0;
    }
}

public enum StructureType
{
    Fence,
    Wall
}
