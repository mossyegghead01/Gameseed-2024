using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandlers : MonoBehaviour
{
    public GunProperty playerGun;
    public TextMeshProUGUI statText;

    void Start()
    {
        statText.text = "Fire Rate: " + playerGun.fireRate.ToString() + "\n"
            + "Bullet Speed: " + playerGun.projectileSpeed.ToString() + "\n"
            + "Range: " + playerGun.weaponRange.ToString() + "\n"
            + "Damage: " + playerGun.damage.ToString();
    }

    public void Reroll()
    {
        
        playerGun.fireRate = Mathf.Round(Random.Range(0.1f, 1f) * 100) / 100.0;
        playerGun.projectileSpeed = Random.Range(10, 40);
        playerGun.weaponRange = Random.Range(10, 60);
        playerGun.damage = Random.Range(1, 50);

        statText.text = "Fire Rate: " + playerGun.fireRate.ToString() + "\n"
            + "Bullet Speed: " + playerGun.projectileSpeed.ToString() + "\n"
            + "Range: " + playerGun.weaponRange.ToString() + "\n"
            + "Damage: " + playerGun.damage.ToString();
    }
}
