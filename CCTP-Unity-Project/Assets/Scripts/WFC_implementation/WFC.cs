using Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFC : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;


    void Start()
    {
        
    }

    public void CollpaseCells()
    {
        //grid[HelperFunctions.ConvertTo1dArray(6, 7, gridWidth)].RemovePossibleTile(SelectRandomTile(tilePrefabs));

        Cell cellToCollapse = gridGenerator.GetCellWithLowestEntropy();
        Debug.Log("Collapsing cell: " + HelperFunctions.ConvertTo2dArray(cellToCollapse.cellIndex, gridGenerator.gridWidth));
        while (!cellToCollapse.Collapsed)
        {
            cellToCollapse.RemovePossibleTile(gridGenerator.SelectRandomTile(gridGenerator.tilePrefabs));
        }
    }

    void Update()
    {
        
    }
}
