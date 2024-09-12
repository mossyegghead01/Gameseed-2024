using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildInventory
{
    private BuildState buildState;
    private List<Slot> slots;
    private BuildInventoryUI buildInventoryUI;
    private int selectedIndex;
    public BuildInventory()
    {
        slots = new List<Slot>()
        {
            // new Slot(SlotState.Empty, this, 1),
        };
        buildInventoryUI = new BuildInventoryUI(this);
        SelectSlot(0);
    }
    public List<Slot> GetSlots()
    {
        return slots;
    }
    public void ToggleBuildState()
    {
        if (buildState == BuildState.Build)
        {
            buildState = BuildState.Break;
        }
        else
        {
            buildState = BuildState.Build;
        }
        Update();
    }
    public void ToggleBuildState(bool state)
    {
        if (state)
        {
            buildState = BuildState.Build;
        }
        else
        {
            buildState = BuildState.Break;
        }
        Update();
    }
    public void SelectSlot(int index)
    {
        if (selectedIndex < slots.Count && selectedIndex >= 0)
        {
            slots[selectedIndex].selected = false;
        }
        selectedIndex = index;
        if (selectedIndex < slots.Count && selectedIndex >= 0 && slots[selectedIndex] != null)
            slots[selectedIndex].selected = true;
        Update();
    }
    public void SelectSlot(Slot slot)
    {
        SelectSlot(slots.FindIndex(s => s == slot));
    }
    public Slot GetSelectedSlot()
    {
        if (selectedIndex < slots.Count && selectedIndex >= 0)
        {
            return slots[selectedIndex];
        }
        return null;
    }
    public void RemoveSlot(int index)
    {
        slots.RemoveAt(index);
        selectedIndex = 1;
        Update();
    }
    public void RemoveSlot(Slot slot)
    {
        selectedIndex = -1;
        slots.Remove(slot);
        Update();
    }
    public void SubtractSlot(int index, int count = 1)
    {
        if (slots[index].GetCount() > 0)
        {
            slots[index].SubtractCount(count);
        }
        Update();
    }
    public void AddSlot(SlotState slotState, int count = 1)
    {

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetSlotState() == slotState)
            {
                slots[i].AddCount(1);
                Update();
                return;
            }
        }
        slots.Add(new Slot(slotState, this, count));
        Update();
    }
    private void Update()
    {
        buildInventoryUI.Update();
    }
}

public class Slot
{
    public bool selected;
    private SlotState slotState;
    private int count;
    private BuildInventory buildInventory;
    public Slot(SlotState slotState, BuildInventory buildInventory, int count = 1)
    {
        this.count = count;
        this.buildInventory = buildInventory;
        SetSlot(slotState);
    }
    public SlotState GetSlotState()
    {
        return slotState;
    }
    public void SetSlot(SlotState slotState)
    {
        this.slotState = slotState;
        Update();
    }
    public int GetCount()
    {
        return count;
    }
    public void AddCount(int count)
    {
        this.count += count;
        Update();
    }
    public void SubtractCount(int count)
    {
        this.count -= count;
        Update();
    }
    private void Update()
    {
        if (count <= 0)
        {
            buildInventory.RemoveSlot(this);
        }
    }
}
public enum BuildState
{
    Build,
    Break
}
public enum SlotState
{
    Empty,
    Carrot,
    Corn,
    Fence,
    Wall,
    ReinforcedWall,
    Concrete,
    ReinforcedConcrete,
    Post,
    Eggplant,
    Cauliflower,
    Tomato,
    Broccoli,
}

public static class BuildInventoryFunctions
{
    private static Dictionary<SlotState, CellState> slotAndCell = new Dictionary<SlotState, CellState>{
        {SlotState.Wall, CellState.Wall},
        {SlotState.Carrot, CellState.Carrot},
        {SlotState.Corn, CellState.Corn},
        {SlotState.Fence, CellState.Fence},
        {SlotState.ReinforcedWall, CellState.ReinforcedWall},
        {SlotState.Concrete, CellState.Concrete},
        {SlotState.ReinforcedConcrete, CellState.ReinforcedConcrete},
        {SlotState.Post, CellState.Post},
        {SlotState.Eggplant, CellState.Eggplant},
        {SlotState.Cauliflower, CellState.Cauliflower},
        {SlotState.Tomato, CellState.Tomato},
        {SlotState.Broccoli, CellState.Broccoli},
    };
    public static CellState SlotToCell(SlotState slotState)
    {
        return slotAndCell[slotState];
    }
    public static CellState SlotToCell(Slot slot)
    {
        return SlotToCell(slot.GetSlotState());
    }
    public static SlotState CellToSlot(CellState cellState)
    {
        return slotAndCell.FirstOrDefault(x => x.Value == cellState).Key;
    }
    public static Sprite SlotToSprite(SlotState slotState)
    {
        return Resources.Load<Sprite>($"Sprites/SlotState/{slotState}");
    }
    public static Sprite SlotToSprite(Slot slot)
    {
        return SlotToSprite(slot.GetSlotState());
    }
    public static int SlotToIndex(SlotState slotState, List<Slot> slots)
    {
        return slots.FindIndex(s => s.GetSlotState() == slotState);
    }
}

public class BuildInventoryUI
{
    //* BuildInventory
    private BuildInventory buildInventory;
    private List<Slot> slots;
    private GameObject buildSlotPrefab;
    private GameObject buildInventoryContainer;
    public BuildInventoryUI(BuildInventory buildInventory)
    {
        this.buildInventory = buildInventory;
        buildSlotPrefab = (GameObject)Resources.Load("Prefabs/BuildSlot");
        buildInventoryContainer = GameObject.Find("Canvas").transform.Find("BuildInventoryContainer").GetChild(0).GetChild(0).GameObject();
        Update();
    }
    public void Update()
    {
        slots = buildInventory.GetSlots();
        foreach (Transform child in buildInventoryContainer.transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
        foreach (Slot slot in slots)
        {
            GameObject buildSlot = UnityEngine.Object.Instantiate(buildSlotPrefab, buildInventoryContainer.transform);
            if (slot.selected)
            {
                buildSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ui/UIBuildSlotSelected");
            }
            buildSlot.GetComponent<BuildInventoryButton>().SetSlot(slot, buildInventory);
        }
    }
}