using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using Helpers;

public class GridGenerator
{
    [SerializeField] private WFCAlgorithm wFC;

    [HideInInspector] public GameObject Map;

    [Header("Grid")]
    //[Range(2, 20)] public int gridDimension = 2;
    [HideInInspector] public List<Cell> grid = new List<Cell>();
    [HideInInspector] public bool gridGenerated = false;

    public GridGenerator(WFCAlgorithm wFC)
    {
        this.wFC = wFC;
    }

    /// <summary>
    /// Selects a random Cell from a give list of cells
    /// </summary>
    /// <param name="cells"> The list of cells to iterate through </param>
    /// <returns> The randomly selected cell </returns>
    public Cell SelectRandomCell(List<Cell> cells)
    {
        return cells[UnityEngine.Random.Range(0, cells.Count)];
    }

    /// <summary>
    /// Generates the map and grid to be solved
    /// </summary>
    public void GenerateGrid()
    {
        gridGenerated = false;

        GenerateNewMap();
        GenerateNewGrid();

        /*foreach (Cell cell in grid)
        {
            cell.ShowPossibleTileInstancesinCell();
        }*/

        wFC.Solver.ResetNumberOfCellsCollapsed();
        gridGenerated = true;
    }

    /// <summary>
    /// Creates an object to store everything in the hierarchy
    /// </summary>
    /// <returns> A new blank gameObject</returns>
    private void GenerateNewMap()
    {
        if (GameObject.Find(wFC.MapName))
        {
            GameObject.DestroyImmediate(GameObject.Find(wFC.MapName));
        }
        Map = new GameObject(wFC.MapName);
    }

    // TODO: should be made public and return grid
    /// <summary>
    /// Creates cells for the new grid
    /// </summary>
    private void GenerateNewGrid()
    {
        grid.Clear();

        for (int row = 0; row < wFC.GridDimension; row++)
        {
            for (int col = 0; col < wFC.GridDimension; col++)
            {
                int i = HelperFunctions.ConvertTo1dArray(row, col, wFC.GridDimension);

                Vector3 cellPositionOffset = new Vector3(-wFC.GridDimension * wFC.SizeOfTiles / 2, 0, -wFC.GridDimension * wFC.SizeOfTiles / 2);
                Vector3 cellPosition = cellPositionOffset + new Vector3(row * wFC.SizeOfTiles, 0, col * wFC.SizeOfTiles) + new Vector3(wFC.SizeOfTiles / 2, 0, wFC.SizeOfTiles / 2);

                Cell cell = new Cell(wFC.Solver, Map, i, cellPosition, wFC.Tileset.prefabs);
                grid.Add(cell);
            }
        }
    }

    /// <summary>
    /// Returns tile with lowest entropy if one exists or a random cell otherwise
    /// </summary>
    /// <returns> 
    /// Lowest entropy cell IF one cell has the fewest possible tiles
    ///   OR
    /// Random Cell IF no cell has fewest possible tiles
    /// </returns>
    /// 
    // TODO: simplify by adding small variation to each
    public Cell GetCellWithLowestEntropy()
    {
        Cell lowestEntropyCell = null;

        foreach (Cell cell in grid)
        { 
            if (!cell.Collapsed)
            {
                lowestEntropyCell = cell;
                break;
            }
        }

        foreach (Cell cell in grid)
        {
            if(cell.GetEntropy() < lowestEntropyCell.GetEntropy() && !cell.Collapsed)
            {
                lowestEntropyCell = cell;
            }
        }

        List<Cell> lowestEntropyCells = new List<Cell>();
        foreach (Cell cell in grid)
        {
            if (cell.GetEntropy() == lowestEntropyCell.GetEntropy())
            {
                lowestEntropyCells.Add(cell);
            }
        }

        if (lowestEntropyCells.Count > 1)
        {
            Cell randomCell = SelectRandomCell(lowestEntropyCells);

            return randomCell;
        }
        else
        {
            return lowestEntropyCells[0];
        }
    }
}
