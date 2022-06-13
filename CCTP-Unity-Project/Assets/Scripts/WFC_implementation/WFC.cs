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
    public void CollpaseCells()
    {
        // select cell with lowest entropy or at random
        Cell cellToCollapse = gridGenerator.GetCellWithLowestEntropy();

        // collpase chosen cell
        Debug.Log("Collapsing cell: " + HelperFunctions.ConvertTo2dArray(cellToCollapse.CellIndex, gridGenerator.gridWidth));
        while (!cellToCollapse.Collapsed)
        {
            cellToCollapse.RemovePossibleTile(gridGenerator.SelectRandomTile(gridGenerator.tilePrefabs));
        }

        // create a list of valid nieghbours
        List<ValidNeighbour> validNeighbours = new List<ValidNeighbour>();
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (HelperFunctions.CheckForValidNeighbourInDirection(cellToCollapse.CellIndex, gridGenerator.gridHeight, gridGenerator.gridWidth, direction))
            {
                Debug.Log(direction + " is valid");
                ValidNeighbour validNeighbour = new ValidNeighbour
                {
                    cell = gridGenerator.grid[GetNeinbourInDirection(cellToCollapse.CellIndex, direction, gridGenerator.gridHeight)],
                    conectionDirection = direction
                };
                validNeighbours.Add(validNeighbour);
            }
            else
            {
                //Debug.Log(direction + " is NOT valid");
            }
        }

        // loop through each valid neighbour
        Debug.Log("Valid neighbours at:");
        foreach (ValidNeighbour validNeighbour in validNeighbours)
        {
            Debug.Log("- " + HelperFunctions.ConvertTo2dArray(validNeighbour.cell.CellIndex, gridGenerator.gridWidth));
            // get the tile's socket in the direction of the valid neighbour
            Socket currentSocket = cellToCollapse.GetTile().GetComponent<Tile>().sockets[(int)validNeighbour.conectionDirection];
            
            // list of tiles to removed from the conecting socket
            List<GameObject> invalidTiles = new List<GameObject>();

            // loop through each tile in the connecting sockets possible tileset
            foreach (GameObject possibleTile in validNeighbour.cell.possibleTiles)
            {
                Socket oppositeSocket = possibleTile.GetComponent<Tile>().sockets[(int)validNeighbour.conectionDirection.GetOppositeDirection()];
                Debug.Log(possibleTile.name + ": " + oppositeSocket.direction + " , " + oppositeSocket.value + " , " + oppositeSocket.validNeighbours[0]);
                foreach (var validNeighbourTile in oppositeSocket.validNeighbours)
                {
                    // if tile is not valid add to list to remove
                    if (validNeighbourTile != currentSocket.value)
                    {
                        invalidTiles.Add(possibleTile);
                    }
                }
            }

            foreach (GameObject invalidTile in invalidTiles)
            {
                validNeighbour/*s[(int)validNeighbour.conectionDirection]*/.cell.RemovePossibleTile(invalidTile);
                //Debug.Log(invalidTile.name + " being removed from cell" + HelperFunctions.ConvertTo2dArray(validNeighbour.cell.CellIndex, gridGenerator.gridWidth));
            }
        }

        //// collapse central tile
        ////Cell cellToCollapse = gridGenerator.grid[HelperFunctions.ConvertTo1dArray(4, 4, gridGenerator.gridWidth)];

        //// collapse cell to a single valid tile
        //while (!cellToCollapse.Collapsed)
        //{
        //    cellToCollapse.RemovePossibleTile(gridGenerator.SelectRandomTile(gridGenerator.tilePrefabs));
        //}

        //Cell[] neighbours = new Cell[Enum.GetValues(typeof(Direction)).Length];
        //foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        //{
        //    Socket thisSocket = cellToCollapse.GetTile().GetComponent<Tile>().sockets[(int)direction];
        //    // get neighbours in each direction
        //    neighbours[(int)direction] = gridGenerator.grid[GetNeinbourInDirection(cellToCollapse.CellIndex, direction, gridGenerator.gridHeight)];

        //    List<GameObject> invalidTiles = new List<GameObject>();
        //    // TODO: ERROR CAUSED BY CHECKING AND REMOVING FROM LIST: InvalidOperationException: Collection was modified; enumeration operation may not execute.
        //    foreach (GameObject tile in neighbours[(int)direction].possibleTiles)
        //    {
        //        Socket oppositeSocket = tile.GetComponent<Tile>().sockets[(int)direction.GetOppositeDirection()];

        //        foreach (var validNeighbour in oppositeSocket.validNeighbours)
        //        {
        //            // if tile is not valid add to list to remove
        //            if (validNeighbour != thisSocket.value)
        //            {
        //                invalidTiles.Add(tile);
        //            }
        //        }
        //    }

        //    // remove tiles from neighbours possible tileset
        //    foreach (GameObject invlaidTile in invalidTiles)
        //    {
        //        neighbours[(int)direction].RemovePossibleTile(invlaidTile);
        //    }
        //}
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
