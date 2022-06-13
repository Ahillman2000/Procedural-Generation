using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using Helpers;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] string mapName = "===== MAP =====";

    [Range(0, 10)] public int gridWidth;
    [Range(0, 10)] public int gridHeight;

    [SerializeField] private float tileOffset = 5f;

    [SerializeField] public List<GameObject> tilePrefabs;
    [SerializeField] GameObject spherePrefab;

    public List<Cell> grid = new List<Cell>();

    void Start()
    {
        
    }

    /// <summary>
    /// Selects a random tile from a give list of objects
    /// </summary>
    /// <param name="tiles"> The list of objects to iterate through </param>
    /// <returns> The randomly selected tile </returns>
    public GameObject SelectRandomTile(List<GameObject> tiles)
    {
        return tiles[UnityEngine.Random.Range(0, tiles.Count)];
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
    /// WFC function
    /// </summary>
    public void GenerateTiles()
    {
        GameObject map = GenerateNewMap();
        GenerateNewGrid(map);
    }

    // TODO: should be made public and return grid
    /// <summary>
    /// Creates cells for the new grid
    /// </summary>
    /// <param name="map"> The object that the cells will be parented to </param>
    private void GenerateNewGrid(GameObject map)
    {
        grid.Clear();

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

    /// <summary>
    /// Creates an object to store everything in the hierarchy
    /// </summary>
    /// <returns> A new blank gameObject</returns>
    private GameObject GenerateNewMap()
    {
        if (GameObject.Find(mapName))
        {
            DestroyImmediate(GameObject.Find(mapName));
        }
        GameObject map = new GameObject(mapName);
        return map;
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
        Cell lowestEntropyCell = null;

        // sets the lowestEntropyCell (LEC) to first uncollapsed cell
        foreach (Cell cell in grid)
        { 
            if (!cell.Collapsed)
            {
                lowestEntropyCell = cell;
                break;
            }
        }

        // loops through grid to find and set LEC
        foreach (Cell cell in grid)
        {
            if(cell.GetEntropy() < lowestEntropyCell.GetEntropy() && !cell.Collapsed)
            {
                lowestEntropyCell = cell;
            }
        }

        /* 
         check that the LEC is the only one with this entropy value,
         if other cells with the lowest entropy are found then add them to a list
        */
        List<Cell> lowestEntropyCells = new List<Cell>();
        foreach (Cell cell in grid)
        {
            if (cell.GetEntropy() == lowestEntropyCell.GetEntropy())
            {
                lowestEntropyCells.Add(cell);
            }
        }

        // if there are more than 1 LECs then return a random cell from list of LECs, otherwise return the LEC
        if (lowestEntropyCells.Count > 1)
        {
            Debug.Log("No lowest entropy cell found, returning random cell");

            Cell randomCell = SelectRandomCell(lowestEntropyCells);

            return randomCell;
        }
        else
        {
            Debug.Log("Lowest entropy cell found");
            return lowestEntropyCells[0];
        }
    }
  
    void Update()
    {
        
    }
}
