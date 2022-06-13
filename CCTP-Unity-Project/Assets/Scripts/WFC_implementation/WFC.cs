using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class WFC : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;

    public int NumberOfTilesCollapsed { get; set; } = 0;

    void Start()
    {
        
    }

    /// <summary>
    /// A container to store a neighbouring cell and the direction from the collapsing cell towards it
    /// </summary>
    struct ValidNeighbour
    {
        public Cell cell;
        public Direction conectionDirection;
    }

    /// <summary>
    /// collapses an initial cell to a single tile, checks each cardinal direction, gets the socket in that direction,
    /// gets the neighbour in direction, iterates through neighbours possible tileset, gets the socket in the opposite direction
    /// if the valid neighbour of that socket isnt the value of this socket then remove the possible tile from the set
    /// </summary>
    public void CollpaseCell()
    {
        // collapse primary cell
        Cell cellToCollapse = gridGenerator.GetCellWithLowestEntropy();

        Debug.Log("Collapsing cell: " + HelperFunctions.ConvertTo2dArray(cellToCollapse.CellIndex, gridGenerator.gridWidth));
        while (!cellToCollapse.Collapsed)
        {
            cellToCollapse.RemovePossibleTile(gridGenerator.SelectRandomTile(gridGenerator.tilePrefabs));
        }

        // get cells in valid directions
        List<ValidNeighbour> validNeighbours = new List<ValidNeighbour>();
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (HelperFunctions.CheckForValidNeighbourInDirection(cellToCollapse.CellIndex, gridGenerator.gridHeight, gridGenerator.gridWidth, direction))
            {
                ValidNeighbour validNeighbour = new ValidNeighbour
                {
                    cell = gridGenerator.grid[GetNeinbourInDirection(cellToCollapse.CellIndex, direction, gridGenerator.gridHeight)],
                    conectionDirection = direction
                };
                validNeighbours.Add(validNeighbour);
            }
            /*else Debug.Log(direction + " is NOT valid");*/
        }

        // see if the potential neighbours have an invalid socket that does not fit with this tiles socket
        foreach (ValidNeighbour validNeighbour in validNeighbours)
        {
            Socket currentSocket = cellToCollapse.GetTile().GetComponent<Tile>().sockets[(int)validNeighbour.conectionDirection];
            
            List<GameObject> invalidTiles = new List<GameObject>();

            foreach (GameObject possibleTile in validNeighbour.cell.possibleTiles)
            {
                Socket oppositeSocket = possibleTile.GetComponent<Tile>().sockets[(int)validNeighbour.conectionDirection.GetOppositeDirection()];
                foreach (var validNeighbourTile in oppositeSocket.validNeighbours)
                {
                    if (validNeighbourTile != currentSocket.value)
                    {
                        invalidTiles.Add(possibleTile);
                    }
                }
            }

            // eliminate invalid tiles from the neighbours possible tileset
            foreach (GameObject invalidTile in invalidTiles)
            {
                validNeighbour.cell.RemovePossibleTile(invalidTile);
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
