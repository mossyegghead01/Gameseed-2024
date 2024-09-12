using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player, tilemap, obelisk, lightTilemap;
    // Start is called before the first frame update
    public void Start()
    {
        // tilemap = GameObject.Find("Tilemap");
        // player = GameObject.Find("Player");
    }
    public GameObject GetPlayer()
    {
        return player;
    }
    public GameObject GetTilemap()
    {
        return tilemap;
    }
    public GameObject GetObelisk()
    {

        return obelisk;
    }
    public GameObject GetLightTilemap()
    {
        return lightTilemap;
    }
}
