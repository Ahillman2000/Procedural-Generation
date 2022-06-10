using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class WFC : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;

    void Start()
    {
        
    }

    /// <summary>
    /// collapses an initial cell to a single tile, checks each cardinal direction, gets the socket in that direction,
    /// gets the neighbour in direction, iterates through neighbours possible tileset, gets the socket in the opposite direction
    /// if the valid neighbour of that socket isnt the value of this socket then remove the possible tile from the set
    /// </summary>
    public void CollpaseCells()
    {
        //Cell cellToCollapse = gridGenerator.GetCellWithLowestEntropy();
        Cell cellToCollapse = gridGenerator.grid[HelperFunctions.ConvertTo1dArray(4, 4, gridGenerator.gridWidth)];
        Debug.Log("Collapsing cell: " + HelperFunctions.ConvertTo2dArray(cellToCollapse.CellIndex, gridGenerator.gridWidth));
        while (!cellToCollapse.Collapsed)
        {
            cellToCollapse.RemovePossibleTile(gridGenerator.SelectRandomTile(gridGenerator.tilePrefabs));
        }

        Cell[] neighbours = new Cell[Enum.GetValues(typeof(Direction)).Length];
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Socket thisSocket = cellToCollapse.GetTile().GetComponent<Tile>().sockets[(int)direction];
            Debug.Log("SOCKET DIRECTION CHECK: the value of socket in Direction: " + direction + " is: " + thisSocket.value);

            neighbours[(int)direction] = gridGenerator.grid[GetNeinbourInDirection(cellToCollapse.CellIndex, direction, gridGenerator.gridHeight)];
            Debug.Log("neighbour in this direction is: " + neighbours[(int)direction].CellIndex);
            Debug.Log("and has possible tiles: ");

            // TODO: ERROR CAUSED BY CHECKING AND REMOVING FROM LIST: InvalidOperationException: Collection was modified; enumeration operation may not execute.
            foreach (GameObject tile in neighbours[(int)direction].possibleTiles)
            {
                Debug.Log("-" + tile);

                Socket oppositeSocket = tile.GetComponent<Tile>().sockets[(int)direction.GetOppositeDirection()];

                if (oppositeSocket.validNeighbours[0] != thisSocket.value)
                {
                    Debug.LogWarning("invalid socket connection, removing tile from neighbours possible tileset");
                    neighbours[(int)direction].RemovePossibleTile(tile);
                }
            }
        }
    }

    /// <summary>
    /// Gets the index of a neighbour in 1 of the 4 cardinal directions
    /// </summary>
    /// <param name="index"> The index of the object from which directions will be checked </param>
    /// <param name="dir"> The direction in which to get a neighbour </param>
    /// <param name="gridHeight"> The height of the grid to act as an offset to jump index values </param>
    /// <returns></returns>
    public int GetNeinbourInDirection(int index, Direction dir, int gridHeight)
    {
        switch (dir)
        {
            case Direction.Up:
                return index + 1;
            case Direction.Down:
                return index - 1;
            case Direction.Left:
                return index - gridHeight;
            case Direction.Right:
                return index + gridHeight;
            default:
                Debug.LogWarning("No tile in given direction");
                return index;
        }
    }

    void Update()
    {
        
    }
}
