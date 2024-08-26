using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Grid grid;
    [SerializeField] GameObject cellObject;
    [SerializeField] GameObject gridContainer;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(20, 10, 1, cellObject, gridContainer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXY(Camera.main.ScreenToWorldPoint(Input.mousePosition), out int x, out int y);
            grid.SetCell(x, y, CellType.Plant, CellState.Carrot);
            print("click");
        }
    }
}
