using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GunProperty playerGun;
    [SerializeField] private TextMeshProUGUI weaponTooltip;
    [SerializeField] private GameObject weaponToolTipObject;
    private GameObject player;
    private bool hovering = false;
    // Start is called before the first frame update
    void Start()
    {
        weaponToolTipObject.SetActive(false);
        player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();
    }


    // Update is called once per frame
    void Update()
    {
        if (playerGun != null && hovering)
            weaponTooltip.text = "Fire Rate: " + playerGun.fireRate.ToString() + "\n"
            + "Bullet Speed: " + playerGun.projectileSpeed.ToString() + "\n"
            + "Range: " + playerGun.weaponRange.ToString() + "\n"
            + "Piercing: " + playerGun.piercing.ToString() + "\n"
            + "Damage: " + playerGun.damage.ToString() + "\n"
            + "Ammo: " + playerGun.MagazineContent.ToString() + "/" + playerGun.magazineSize.ToString() + "\n"
            + "Fire Type: " + Enum.GetName(typeof(GunProperty.FireType), playerGun.gunFireType);

    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        player.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/hoverUI"));
        if (playerGun != null)
        {

            weaponToolTipObject.SetActive(true);
            hovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        weaponToolTipObject.SetActive(false);
        hovering = false;
    }
}
