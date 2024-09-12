using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaNotificationHandler : MonoBehaviour
{
    public GameObject held;
    public GameObject inventory;

    public void Take()
    {
        held.transform.SetParent(inventory.transform, false);
        Destroy(gameObject);
    }

    public void Trash()
    {
        Destroy(held);
        Destroy(gameObject);
    }
}
