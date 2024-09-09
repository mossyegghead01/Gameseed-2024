using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class obeliskScript : MonoBehaviour
{
    private Animator animator;
    private int stage = 1;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetStage(int stage)
    {
        this.stage = stage;
        animator.SetInteger("stage", stage);
    }
    public int GetStage()
    {
        return stage;
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
