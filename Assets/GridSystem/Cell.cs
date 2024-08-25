using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private CellType cellType;
    private int health, maxHealth;
    public bool isLit;
    private Sprite sprite;

    public Cell(CellType cellType, PlantType plantType)
    {
        this.cellType = cellType;
        if (cellType == CellType.Plant)
        {
            Plant plant = new Plant(plantType);
            maxHealth = plant.GetMaxHealth();
            sprite = plant.GetSprite();
        }
        health = maxHealth;
    }
    public Cell(CellType cellType, StructureType structureType)
    {
        this.cellType = cellType;
        if (cellType == CellType.Plant)
        {
            Structure structure = new Structure(structureType);
            maxHealth = structure.GetMaxHealth();
        }
        health = maxHealth;
    }

    public Cell(CellType cellType)
    {
        if (cellType == CellType.Empty)
        {
            maxHealth = -1;
        }
        health = 0;
    }

    public void Break(int damage)
    {
        if (health > 0)
        {
            health -= damage;

        }
    }

    public void Heal(int health)
    {
        this.health += health;
    }

    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Assets/Sprites/Square.png");
    }
}
public enum CellType
{
    Empty,
    Plant,
    Structure
}
