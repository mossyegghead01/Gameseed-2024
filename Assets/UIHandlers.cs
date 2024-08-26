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
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < inventoryHolder.transform.childCount; i++)
        {
            Transform child = inventoryHolder.transform.GetChild(i);
            if (child.TryGetComponent<Item>(out var item))
            {
                inventoryPanels.transform.Find("Panel" + i).GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
            }
        }

        GunProperty playerGun = playerGunAxis.GetComponentInChildren<GunProperty>();
        statText.text = "Fire Rate: " + playerGun.fireRate.ToString() + "\n"
        + "Bullet Speed: " + playerGun.projectileSpeed.ToString() + "\n"
        + "Range: " + playerGun.weaponRange.ToString() + "\n"
        + "Damage: " + playerGun.damage.ToString();
    }

    // DO NOT USE, TESTING ONLY
    public void Reroll()
    {
        GunProperty playerGun = playerGunAxis.GetComponentInChildren<GunProperty>();
        playerGun.fireRate = Mathf.Round(Random.Range(0.1f, 1f) * 100) / 100.0;
        playerGun.projectileSpeed = Random.Range(10, 40);
        playerGun.weaponRange = Random.Range(10, 60);
        playerGun.damage = Random.Range(1, 50);

        statText.text = "Fire Rate: " + playerGun.fireRate.ToString() + "\n"
            + "Bullet Speed: " + playerGun.projectileSpeed.ToString() + "\n"
            + "Range: " + playerGun.weaponRange.ToString() + "\n"
            + "Damage: " + playerGun.damage.ToString();
    }

    public void InventoryButtonClicked(int index)
    {
        if (index < inventoryHolder.transform.childCount)
        {
            var clicked = inventoryHolder.transform.GetChild(index);
            var current = playerGunAxis.transform.GetChild(0);
            clicked.transform.SetParent(playerGunAxis.transform, false);
            current.transform.SetParent(inventoryHolder.transform, false);
            current.SetSiblingIndex(index);
            //inventoryHolder.transform.GetChild(index).transform.SetParent(playerGun.transform.parent, false);
            //playerGun.transform.SetParent(inventoryHolder.transform, false);
            //playerGun
            //print(inventoryHolder.transform.GetChild(index).name);

            UpdateUI();
        }
    }
}
