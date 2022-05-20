using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        ValuesManager<TileBase> valueManager = new ValuesManager<TileBase>(grid);
        PatternManager manager = new PatternManager(2);
        manager.ProcessGrid(valueManager, false);
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Debug.Log(direction.ToString() + " " + string.Join(" ", manager.GetPossibleNeighboursForPatternInDirection(0, direction).ToArray()));
        }

        // debugger for value manager identiying correct values in grid
        /*StringBuilder builder = null;
        List<string> list = new List<string>();

        for (int row = -1; row <= grid.Length; row++)
        {
            builder = new StringBuilder();
            for (int col = -1; col <= grid[0].Length; col++)
            {
                builder.Append(valueManager.GetGridValuesIncludingOffset(col, row) + " ");
            }
            list.Add(builder.ToString());
        }

        list.Reverse();
        foreach (var item in list)
        {
            Debug.Log(item);
        }*/

    }


    void Update()
    {
        
    }
}
