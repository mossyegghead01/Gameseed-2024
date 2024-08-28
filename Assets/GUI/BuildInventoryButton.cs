using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildInventoryButton : MonoBehaviour
{
    Slot slot;
    BuildInventory buildInventory;
    public void SetSlot(Slot slot, BuildInventory buildInventory)
    {
        this.slot = slot;
        this.buildInventory = buildInventory;
        transform.GetChild(0).GetComponent<Image>().sprite = BuildInventoryFunctions.SlotToSprite(slot);
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = slot.GetCount().ToString();
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        buildInventory.SelectSlot(slot);
    }
}
