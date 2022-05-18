using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse;

public class WFCTestScript : MonoBehaviour
{
    public Tilemap input;

    void Start()
    {
        InputReader reader = new InputReader(input);
        var grid = reader.ReadInputToGrid();

        /*for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[0].Length; col++)
            {
                Debug.Log("Row: " + row + ", Column: " + col + " tile name " + grid[row][col].Value.name);
            }
        }*/

        ValuesManager<TileBase> valueManager = new ValuesManager<TileBase>(grid);
        StringBuilder builder = null;
        List<string> list = new List<string>();

        Debug.Log("number of rows: " + grid.Length);
        Debug.Log("number of coloumns: " + grid[0].Length);

        for (int row = -1; row <= grid.Length; row++)
        {
            builder = new StringBuilder();
            for (int col = -1; col <= grid[0].Length; col++)
            {
                builder.Append(valueManager.GetGridValuesIncludingOffset(col, row) + " ");
            }
            list.Add(builder.ToString());
        }

        Debug.Log("list size = " + list.Count);

        list.Reverse();
        foreach (var item in list)
        {
            Debug.Log(item);
        }
    }


    void Update()
    {
        
    }
}
