using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player, tilemap, obelisk, lightTilemap, eventSystem, buildTooltip;
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

    public GameObject GetEventSystem()
    {
        return eventSystem;
    }
    public GameObject GetBuildTooltip()
    {
        return buildTooltip;
    }
}
