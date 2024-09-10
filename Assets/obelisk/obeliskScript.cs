using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public class obeliskScript : MonoBehaviour
{
    private float health = 100;
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
        SetStage(3);
    }
    public void SetStage(int stage)
    {
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
