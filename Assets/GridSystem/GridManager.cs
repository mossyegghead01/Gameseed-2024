using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Grid grid;
    [SerializeField] GameObject cellObject;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(20, 10, 1, cellObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
