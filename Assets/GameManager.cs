using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player, tilemap, obelisk;
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
        Debug.Log("tilemap is " + tilemap);
        return tilemap;
    }
    public GameObject GetObelisk()
    {

        return obelisk;
    }
}
