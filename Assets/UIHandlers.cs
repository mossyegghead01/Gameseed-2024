using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandlers : MonoBehaviour
{
    public GameObject playerGunAxis;
    public TextMeshProUGUI statText;
    public GameObject inventoryHolder;
    public CanvasRenderer inventoryPanels;

    void Start()
    {
        // First UI update, in case inventory is populated.
        UpdateUI();
    }

    // Update the inventory UI
    // May be expanded for other UI update
    public void UpdateUI()
    {
        // Run through all items in inventory gameobject
        for (int i = 0; i < inventoryHolder.transform.childCount; i++)
        {
            Transform child = inventoryHolder.transform.GetChild(i);
            if (child.TryGetComponent<Item>(out var item) && inventoryPanels.transform.Find("Panel" + i) != null)
            {
                // Change the icon
                inventoryPanels.transform.Find("Panel" + i).GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
            }
        }

        // Update debug statistic text
        GunProperty playerGun = playerGunAxis.GetComponentInChildren<GunProperty>();
        statText.text = "Fire Rate: " + playerGun.fireRate.ToString() + "\n"
        + "Bullet Speed: " + playerGun.projectileSpeed.ToString() + "\n"
        + "Range: " + playerGun.weaponRange.ToString() + "\n"
        + "Piercing: " + playerGun.piercing.ToString() + "\n"
        + "Damage: " + playerGun.damage.ToString();
    }

    // Handler for equipping
    // TODO: Bind hotkey into this function too
    public void InventoryButtonClicked(int index)
    {
        if (index < inventoryHolder.transform.childCount)
        {
            var clicked = inventoryHolder.transform.GetChild(index);
            var current = playerGunAxis.transform.GetChild(0);
            clicked.transform.SetParent(playerGunAxis.transform, false);
            clicked.GetComponentInChildren<ParticleSystem>().Stop();
            current.transform.SetParent(inventoryHolder.transform, false);
            current.SetSiblingIndex(index);
            current.GetComponentInChildren<ParticleSystem>().Stop();

            UpdateUI();
        }
    }
}
