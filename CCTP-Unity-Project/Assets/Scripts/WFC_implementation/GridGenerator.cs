using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using Helpers;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] string mapName = "===== MAP =====";

    [Range(0, 10)] [SerializeField] private int gridWidth;
    [Range(0, 10)] [SerializeField] private int gridHeight;

    [SerializeField] private float tileOffset = 5f;

    [SerializeField] private List<GameObject> tilePrefabs;
    [SerializeField] GameObject spherePrefab;

    public List<Cell> grid = new List<Cell>();

    void Start()
    {
        
    }

    private GameObject SelectRandomTile(List<GameObject> tiles)
    {
        return tiles[UnityEngine.Random.Range(0, tiles.Count)];
    }

    private Cell SelectRandomCell(List<Cell> grid)
    {
        return grid[UnityEngine.Random.Range(0, grid.Count)];
    }

    public void GenerateTiles()
    {
        GameObject map = GenerateNewMap();

        grid.Clear();

        GenerateNewGrid(map);

        // Collapse cells
        //foreach (Cell cell in grid)
        //{
        //    // remove all tiles in order but 1
        //    /*for (int i = 0; i < 3; i++)
        //    {
        //        cell.RemovePossibleTile(tilePrefabs[i]);
        //    }*/

        //    // collapse cell to single random tile
        //    while (!cell.Collapsed)
        //    {
        //        cell.RemovePossibleTile(GenerateRandomTile(tilePrefabs));
        //    }
        //}

        grid[HelperFunctions.ConvertTo1dArray(6, 7, gridWidth)].RemovePossibleTile(SelectRandomTile(tilePrefabs));

        Cell cellToCollapse = GetCellWithLowestEntropy();
        Debug.Log("Collapsing cell: " + HelperFunctions.ConvertTo2dArray(cellToCollapse.cellIndex, gridWidth));
        while(!cellToCollapse.Collapsed)
        {
            cellToCollapse.RemovePossibleTile(SelectRandomTile(tilePrefabs));
        }
    }

    private void GenerateNewGrid(GameObject map)
    {
        GameObject debugSpheres = new GameObject("debugSpheres");
        debugSpheres.transform.parent = map.transform;

        for (int row = 0; row < gridWidth; row++)
        {
            for (int col = 0; col < gridHeight; col++)
            {
                int i = HelperFunctions.ConvertTo1dArray(row, col, gridWidth);

                Vector3 tilePositionOffset = new Vector3(-gridWidth * tileOffset / 2, 0, -gridHeight * tileOffset / 2);
                Vector3 tilePosition = tilePositionOffset + new Vector3(row * tileOffset, 0, col * tileOffset) + new Vector3(tileOffset / 2, 0, tileOffset / 2);

                Cell cell = new Cell(map, i, tilePosition, tilePrefabs);
                GameObject debugSphere = Instantiate(spherePrefab, tilePosition, Quaternion.identity);
                debugSphere.transform.parent = debugSpheres.transform;
                grid.Add(cell);
            }
        }
    }

    private GameObject GenerateNewMap()
    {
        if (GameObject.Find(mapName))
        {
            DestroyImmediate(GameObject.Find(mapName));
        }
        GameObject map = new GameObject(mapName);
        return map;
    }

    public Cell GetNeinbourInDirection(int index, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return grid[index + 1];
            case Direction.Down:
                return grid[index - 1];
            case Direction.Left:
                return grid[index - gridHeight];
            case Direction.Right:
                return grid[index + gridHeight];
            default:
                Debug.LogWarning("No tile in given direction");
                return null;
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
    public Cell GetCellWithLowestEntropy()
    {
        Cell lowestEntropyCell = grid[0];

        foreach (Cell cell in grid)
        {
            if(cell.GetEntropy() < lowestEntropyCell.GetEntropy() && !cell.Collapsed)
            {
                lowestEntropyCell = cell;
            }
        }

        if(lowestEntropyCell == grid[0])
        {
            List<Cell> lowestEntropyCells = new List<Cell>();

            foreach (Cell cell in grid)
            {
                if (cell.GetEntropy() == grid[0].GetEntropy())
                {
                    lowestEntropyCells.Add(cell);
                }
            }

            if(lowestEntropyCells.Count > 1)
            {
                Debug.Log("No lowest entropy cell found, returning random cell");
                return SelectRandomCell(grid);
            }
        }
        Debug.Log("Returning cell with loweset entropy");
        return lowestEntropyCell;
    }

    void Update()
    {
        
    }
}
