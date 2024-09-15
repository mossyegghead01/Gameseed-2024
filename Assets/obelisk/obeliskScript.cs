using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public class obeliskScript : MonoBehaviour
{
    private float health = 100, maxHealth = 100;
    private Animator animator;
    [SerializeField] private GameObject gameManager;
    private TileBase backgroundTile;
    private Dictionary<(int, int), bool> obeliskCoordinates;

    private int stage = 1;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        backgroundTile = Resources.Load<TileBase>("Tilemap/Tiles/Background");
        SetStage(1);
    }
    public void SetStage(int stage)
    {
        //Debug.Log("Stage: " + stage);
        if (stage != this.stage)
            gameManager.GetComponent<GameManager>().GetPlayer().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/levelUp"));
        this.stage = stage;
        animator.SetInteger("stage", stage);
        obeliskCoordinates = ObeliskFunctions.GetObeliskCoordinates(stage);
        foreach (KeyValuePair<(int, int), bool> coord in obeliskCoordinates)
        {
            gameManager.GetComponent<GameManager>().GetLightTilemap().GetComponent<Tilemap>().SetTile(new Vector3Int(coord.Key.Item1, coord.Key.Item2, 0), backgroundTile);
        }
    }
    public int GetStage()
    {
        return stage;
    }

    public Dictionary<(int, int), bool> GetObeliskCoordinates()
    {
        return obeliskCoordinates;
    }

    public void Damage(float damage)
    {
        health -= damage;
        var healthbar = GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetChild(0).transform.GetComponent<RectTransform>();
        healthbar.offsetMax = new Vector2(-(170 - (health / maxHealth * 170)), healthbar.offsetMax.y);
        if (GetComponent<AudioSource>().isPlaying == false)
        {

            GetComponent<AudioSource>().Play();
        }
        if (health <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

}

public class ObeliskFunctions
{

    public static Dictionary<(int, int), bool> GetObeliskCoordinates(int stage)
    {
        int[] center = { 8, -4 };
        Dictionary<(int, int), bool> obeliskCoordinates = new Dictionary<(int, int), bool>();
        for (double i = -stage; i <= stage; i++)
        {
            for (double j = -stage; j <= stage; j++)
            {
                if (Mathf.Abs((float)i) + Mathf.Abs((float)j) <= stage && (i != 0 || j != 0))
                {
                    // Debug.Log((center[0] + i, center[1] + j));
                    obeliskCoordinates.Add((center[0] + (int)i, center[1] + (int)j), true);
                }
            }
        }

        return obeliskCoordinates;
    }
}
