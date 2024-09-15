using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHandlers : MonoBehaviour
{
    public GameObject playerGunAxis;
    public TextMeshProUGUI statText;
    public GameObject inventoryHolder;
    public CanvasRenderer inventoryPanels;
    public Image currentGunIcon;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ammoText;
    public GameObject notificationUiPrefab;
    public GameObject gachaHolder;
    public GameObject canvas;
    public Transform ReloadOverlay;
    public Transform gunFireType;
    private Image fireTypeImage;
    private GameObject obelisk;
    public TextMeshProUGUI weaponTooltipText, slotTooptipText;
    [SerializeField] private GameObject selectedWeapon, scoreObject;

    [SerializeField] private float score = 0;

    void Start()
    {
        fireTypeImage = gunFireType.GetComponent<Image>();
        obelisk = GameObject.Find("Obelisk").transform.GetChild(0).gameObject;
    }
    void Awake()
    {
        DontDestroyOnLoad(scoreObject);
    }

    void Update()
    {
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
                inventoryPanels.transform.Find("Panel" + i).GetComponent<HoverTooltip>().playerGun = child.GetComponent<GunProperty>();
            }
        }

        // Update the equipped gun icon
        Transform currentGun = playerGunAxis.transform.GetChild(0);
        if (currentGun.TryGetComponent<Item>(out var itemCurrent))
        {
            currentGunIcon.sprite = itemCurrent.itemIcon;
            selectedWeapon.GetComponent<HoverTooltip>().playerGun = currentGun.GetComponent<GunProperty>();
        }

        // Update debug statistic text
        GunProperty playerGun = playerGunAxis.GetComponentInChildren<GunProperty>();
        weaponTooltipText.text =
        "Fire Rate: " + playerGun.fireRate.ToString() + "\n"
        + "Bullet Speed: " + playerGun.projectileSpeed.ToString() + "\n"
        + "Range: " + playerGun.weaponRange.ToString() + "\n"
        + "Piercing: " + playerGun.piercing.ToString() + "\n"
        + "Damage: " + playerGun.damage.ToString() + "\n"
        + "Ammo: " + playerGun.MagazineContent.ToString() + "/" + playerGun.magazineSize.ToString() + "\n"
        + "Fire Type: " + Enum.GetName(typeof(GunProperty.FireType), playerGun.gunFireType);

        switch (playerGun.gunFireType)
        {
            case GunProperty.FireType.Single:
                fireTypeImage.sprite = Resources.Load<Sprite>("Sprites/ui/gunSingle");
                break;
            case GunProperty.FireType.Burst:
                fireTypeImage.sprite = Resources.Load<Sprite>("Sprites/ui/gunBurst");
                break;
            case GunProperty.FireType.Scatter:
                fireTypeImage.sprite = Resources.Load<Sprite>("Sprites/ui/gunScatter");
                break;
            case GunProperty.FireType.Automatic:
                fireTypeImage.sprite = Resources.Load<Sprite>("Sprites/ui/gunAuto");
                break;
        }
        if (playerGun.reloading)
        {

            ammoText.text = "...";
            ReloadOverlay.GetComponent<Animator>().SetBool("playReload", true);
        }
        else
        {
            ammoText.text = playerGun.MagazineContent.ToString() + "/" + playerGun.magazineSize.ToString();
            ReloadOverlay.GetComponent<Animator>().SetBool("playReload", false);
        }

        for (int i = 0; i < gachaHolder.transform.childCount; i++)
        {
            if (canvas.transform.childCount > 0)
            {
                if (canvas.transform.GetChild(i).GetComponent<GachaNotificationHandler>().held != gachaHolder.transform.GetChild(i).gameObject)
                {
                    var ui = Instantiate(notificationUiPrefab, canvas.transform);
                    ui.GetComponent<GachaNotificationHandler>().inventory = inventoryHolder;
                    ui.GetComponent<GachaNotificationHandler>().held = gachaHolder.transform.GetChild(i).gameObject;
                    ui.transform.GetChild(1).GetComponent<Image>().sprite = gachaHolder.transform.GetChild(i).GetComponent<Item>().itemIcon;
                }
            }
            else
            {
                var ui = Instantiate(notificationUiPrefab, canvas.transform);
                ui.GetComponent<GachaNotificationHandler>().inventory = inventoryHolder;
                ui.GetComponent<GachaNotificationHandler>().held = gachaHolder.transform.GetChild(i).gameObject;
                ui.transform.GetChild(1).GetComponent<Image>().sprite = gachaHolder.transform.GetChild(i).GetComponent<Item>().itemIcon;
            }
        }

        scoreText.text = score.ToString();
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
            GameObject.Find("GameManager").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/gunSwitch"));
        }
    }

    public void IncrementScore(float scoreIncrement = 1)
    {
        score += scoreIncrement;
        if (obelisk != null)
            obelisk.GetComponent<obeliskScript>().SetStage(Mathf.FloorToInt(score / 40) + 1);
        scoreObject.GetComponent<Score>().score = score;
    }

    public float GetScore()
    {
        return score;
    }
}