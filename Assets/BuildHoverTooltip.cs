using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildHoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slot slot;
    [SerializeField] private GameObject tooltip, player;
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
        player.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/hoverUI"));

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
        player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();

        tooltip = GameObject.Find("GameManager").GetComponent<GameManager>().GetBuildTooltip();
    }
}
