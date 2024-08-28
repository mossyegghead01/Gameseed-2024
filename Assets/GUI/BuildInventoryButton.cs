using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildInventoryButton : MonoBehaviour
{
    Slot slot;
    public void SetSlot(Slot slot)
    {
        this.slot = slot;
        transform.GetChild(0).GetComponent<Image>().sprite = BuildInventoryFunctions.SlotToSprite(slot);
    }
}
