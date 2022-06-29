using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class Solver : MonoBehaviour
{
    //[SerializeField] private GridGenerator gridGenerator;
    private GridGenerator gridGenerator;

    private int numberOfTilesCollapsed;

    public static Solver Instance { get; set; } = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = (Solver)FindObjectOfType(typeof(Solver));
        else
            Instance = this;
    }

    void Start()
    {
        gridGenerator = GridGenerator.Instance;
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
    /// The main WFC solver function
    /// </summary>
    public void Solve()
    {
        numberOfTilesCollapsed = 0;
        while (numberOfTilesCollapsed < gridGenerator.grid.Count)
        {
            Iterate();
        }
    }

    /// <summary>
    /// A single iteration of the wave collapse function
    /// </summary>
    public void Iterate()
    {
        Cell cell = gridGenerator.GetCellWithLowestEntropy();
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
            cellToCollapse.RemovePossibleTile(cellToCollapse.SelectRandomPossibleTile()); 
        }
    }

    /// <summary>
    /// Removes any invalid possible tiles from neighbouring tilesets after a cell has been collapsed
    /// </summary>
    /// <param name="cellToPropagate"> The cell to propagate out from </param>
    private void Propagate(Cell cellToPropagate)
    {
        foreach (ValidNeighbour neighbour in GetValidNeighbours(cellToPropagate))
        {
            var possibleNeighbours = cellToPropagate.GetTile().neighbourList[(int)neighbour.conectionDirection].neighbours;
            var otherPossibleTiles = neighbour.cell.possibleTiles;

            List<GameObject> removals = new List<GameObject>();

            foreach (var otherTile in otherPossibleTiles)
            {
                if (!possibleNeighbours.Contains(otherTile))
                {
                    removals.Add(otherTile);
                }
            }

            foreach (var otherTile in removals)
            {
                neighbour.cell.RemovePossibleTile(otherTile);
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

    /// <summary>
    /// Determines which directions have a valid neighbour next to a given cell
    /// </summary>
    /// <param name="cell"> The cell to Check from </param>
    /// <returns> A list of neighbouring cells and the direction in which they connect</returns>
    private List<ValidNeighbour> GetValidNeighbours(Cell cell)
    {
        List<ValidNeighbour> validNeighbours = new List<ValidNeighbour>();
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (HelperFunctions.CheckForValidNeighbourInDirection(cell.CellIndex, gridGenerator.gridDimension, gridGenerator.gridDimension, direction))
            {
                ValidNeighbour validNeighbour = new ValidNeighbour
                {
                    cell = gridGenerator.grid[GetNeinbourInDirection(cell.CellIndex, direction, gridGenerator.gridDimension)],
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

    /// <summary>
    /// Trigger for when a cell collapses, used to keep count of total collapsed cells
    /// </summary>
    public void OnCellCollapse()
    {
        numberOfTilesCollapsed++;
    }
}
