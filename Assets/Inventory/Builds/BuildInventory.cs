using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildInventory
{
    private BuildState buildState;
    private List<Slot> slots;
    private BuildInventoryUI buildInventoryUI;
    private int selectedIndex;
    public BuildInventory()
    {
        slots = new List<Slot>(){
            new Slot(SlotState.Wall, this),
            new Slot(SlotState.Carrot, this),
            new Slot(SlotState.Corn, this),
            new Slot(SlotState.Fence, this)
        };
        buildInventoryUI = new BuildInventoryUI(this);
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
        Update();
    }
    public void RemoveSlot(Slot slot)
    {
        selectedIndex = -1;
        slots.Remove(slot);
        Update();
    }
    public void SubtractSlot(int index)
    {
        if (slots[index].GetCount() > 0)
        {
            slots[index].SubtractCount(1);
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
        for (int i = 0; i < slots.Count; i++)
        {
            Debug.Log(slots[i].GetSlotState());
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
    public static CellState SlotToCell(Slot slot)
    {
        return SlotToCell(slot.GetSlotState());
    }
    public static Sprite SlotToSprite(SlotState slotState)
    {
        return Resources.Load<Sprite>($"Sprites/SlotState/{slotState}");
    }
    public static Sprite SlotToSprite(Slot slot)
    {
        return SlotToSprite(slot.GetSlotState());
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
        buildInventoryContainer = GameObject.Find("Canvas").transform.Find("BuildInventoryContainer").GameObject();
        UpdateBuild();
    }
    private void UpdateBuild()
    {
        slots = buildInventory.GetSlots();
        foreach (Transform child in buildInventoryContainer.transform)
        {
            Object.Destroy(child.gameObject);
        }
        foreach (Slot slot in slots)
        {
            GameObject buildSlot = Object.Instantiate(buildSlotPrefab, buildInventoryContainer.transform);
            buildSlot.GetComponent<BuildInventoryButton>().SetSlot(slot);
        }
    }
}