using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildHoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slot slot;
    [SerializeField] private GameObject tooltip;
    private bool hovering = false;
    void Update()
    {
        if (slot != null && hovering)
        {
            tooltip.GetComponentInChildren<TextMeshProUGUI>().text = slot.GetSlotState() + "\n" + "Health: " + CellFunctions.GetMaxHealth(BuildInventoryFunctions.SlotToCell(slot));
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot != null)
        {

            tooltip.SetActive(true);
            hovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }
    void Start()
    {
        tooltip = GameObject.Find("GameManager").GetComponent<GameManager>().GetBuildTooltip();
    }
}
