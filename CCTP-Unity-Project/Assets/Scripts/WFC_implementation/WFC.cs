using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class WFC : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;

    private int numberOfTilesCollapsed;

    void Start()
    {
        
    }

    void Update()
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
    /// The complete WFC algorithm to create a grid and solve it
    /// BROKEN
    /// </summary>
    public void WFCAlgorithm()
    {
        gridGenerator.GenerateGrid();
        Solve();
    }

    /// <summary>
    /// The main WFC solver algorithm
    /// </summary>
    public void Solve()
    {
        numberOfTilesCollapsed = 0;

        while (numberOfTilesCollapsed < gridGenerator.grid.Count)
        {
            Iterate();
            Debug.Log("iteration complete");
        }
        Debug.Log("Grid fully collapsed. " + numberOfTilesCollapsed + " tiles collapsed");
    }

    /// <summary>
    /// A single iteration of the wave collapse function
    /// </summary>
    public void Iterate()
    {
        Cell cell = gridGenerator.GetCellWithLowestEntropy();
        Debug.Log("collapsing cell: " + HelperFunctions.ConvertTo2dArray(cell.CellIndex, gridGenerator.gridDimensionSquared));
        CollapseCell(cell);
        Propagate(cell);
    }

    /// <summary>
    /// collapses a given cell to a single tile
    /// </summary>
    public void CollapseCell(Cell cellToCollapse)
    {
        while (!cellToCollapse.Collapsed)
        {
            cellToCollapse.RemovePossibleTile(gridGenerator.SelectRandomTile(gridGenerator.tilePrefabs));
        }
    }

    /// <summary>
    /// Removes any invalid possible tiles from neighbouring tilesets after a cell has been collapsed
    /// </summary>
    /// <param name="cellToPropagate"> The cell to propagate out from </param>
    private void Propagate(Cell cellToPropagate)
    {
        Stack<Cell> stack = new Stack<Cell>();
        stack.Push(cellToPropagate);

        while (stack.Count > 0)
        {
            Cell curentCell = stack.Pop();
            bool addCellToStack = false;
            foreach (var validNeighbour in GetValidNeighbours(curentCell))
            {
                Cell neighbouringCell = validNeighbour.cell;

                foreach (GameObject posibleTile in curentCell.possibleTiles)
                {
                    Socket currentSocket = posibleTile.GetComponent<Tile>().sockets[(int)validNeighbour.conectionDirection];
                    List<GameObject> invalidTiles = new List<GameObject>();

                    foreach (GameObject possibleNeighbouringTile in neighbouringCell.possibleTiles)
                    {
                        Socket oppositeSocket = possibleNeighbouringTile.GetComponent<Tile>().sockets[(int)validNeighbour.conectionDirection.GetOppositeDirection()];

                        foreach (var neighbour in oppositeSocket.validNeighbours)
                        {
                            if (neighbour != currentSocket.value)
                            {
                                invalidTiles.Add(possibleNeighbouringTile);
                            }
                        }
                    }

                    if (invalidTiles.Count > 0)
                    {
                        foreach (GameObject invalidTile in invalidTiles)
                        {
                            validNeighbour.cell.RemovePossibleTile(invalidTile);
                            addCellToStack = true;
                            //stack.Push(validNeighbour.cell);
                        }
                    }
                    else
                    {
                        // the neighbouring cell cannot be propagated
                    }
                }

                if(addCellToStack)
                {
                    stack.Push(validNeighbour.cell);
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

    private List<ValidNeighbour> GetValidNeighbours(Cell cell)
    {
        List<ValidNeighbour> validNeighbours = new List<ValidNeighbour>();
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (HelperFunctions.CheckForValidNeighbourInDirection(cell.CellIndex, gridGenerator.gridDimensionSquared, gridGenerator.gridDimensionSquared, direction))
            {
                ValidNeighbour validNeighbour = new ValidNeighbour
                {
                    cell = gridGenerator.grid[GetNeinbourInDirection(cell.CellIndex, direction, gridGenerator.gridDimensionSquared)],
                    conectionDirection = direction
                };

                if (!validNeighbour.cell.Collapsed)
                {
                    validNeighbours.Add(validNeighbour);
                }
            }
            /*else Debug.Log(direction + " is NOT valid");*/
        }
        return validNeighbours;
    }

    public void OnCellCollapse()
    {
        numberOfTilesCollapsed++;
    }
}
