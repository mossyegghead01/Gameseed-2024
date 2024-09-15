using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildHoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slot slot;
    [SerializeField] private GameObject tooltip, player, gameManager;
    private bool hovering = false;
    private Button button;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();
        tooltip = GameObject.Find("GameManager").GetComponent<GameManager>().GetBuildTooltip();

        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

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

    void OnButtonClick()
    {
        gameManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/click"));

    }
}
