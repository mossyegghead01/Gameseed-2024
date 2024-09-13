using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaNotificationHandler : MonoBehaviour
{
    public GameObject held;
    public GameObject inventory;

    public void Take(int slot)
    {
        if (inventory.transform.childCount > slot)
        {
            Destroy(inventory.transform.GetChild(slot).gameObject);
        }
        held.transform.SetParent(inventory.transform, false);
        held.transform.SetSiblingIndex(slot);
        Destroy(gameObject);
    }

    public void Trash()
    {
        Destroy(held);
        Destroy(gameObject);
    }

    void Update()
    {
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            Transform child = inventory.transform.GetChild(i);
            if (child.TryGetComponent<Item>(out var item) && transform.Find("Panel" + i) != null)
            {
                // Change the icon
                transform.Find("Panel" + i).GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
            }
        }
    }
}
