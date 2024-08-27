using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildInventory
{
    private BuildState buildState;
    private List<Slot> slots;
    private int selectedIndex;
    public BuildInventory()
    {
        slots = new();
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
    }
    public void SelectSlot(int index)
    {
        selectedIndex = index;
    }
    public Slot GetSelectedSlot()
    {
        if (selectedIndex == -1)
        {
            return null;
        }
        return slots[selectedIndex];
    }
    public void RemoveSlot(int index)
    {
        selectedIndex = -1;
        slots.RemoveAt(index);
    }
    public void RemoveSlot(Slot slot)
    {
        selectedIndex = -1;
        slots.Remove(slot);
    }
    public void SubtractSlot(int index)
    {
        if (slots[index].GetCount() > 0)
        {
            slots[index].SubtractCount(1);
        }
    }
    public void AddSlot(SlotState slotState, int count = 1)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetSlotState() == slotState)
            {
                slots[i].AddCount(1);
                return;
            }
            else
            {

                slots.Add(new Slot(slotState, this, count));
            }
        }
    }
}

public class Slot
{
    private SlotState slotState;
    private int count;
    private BuildInventory buildInventory;
    public Slot(SlotState slotState, BuildInventory buildInventory, int count = 1)
    {
        this.count = count;
        SetSlot(slotState);
        this.buildInventory = buildInventory;
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
        this.count += count;
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
    Wall,
    Carrot,
    Corn,
    Fence
}

public static class BuildInventoryFunctions
{
    private static Dictionary<SlotState, CellState> slotAndCell = new Dictionary<SlotState, CellState>{
        {SlotState.Wall, CellState.Wall},
        {SlotState.Carrot, CellState.Carrot},
        {SlotState.Corn, CellState.Corn},
        {SlotState.Fence, CellState.Fence}
    };
    public static CellState SlotToCell(SlotState slotState)
    {
        return slotAndCell[slotState];
    }
}