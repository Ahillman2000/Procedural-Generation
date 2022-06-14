using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using Helpers;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] string mapName = "===== MAP =====";

    [Range(2, 20)] public int gridDimensionSquared = 2;

    [SerializeField] private float tileOffset = 5f;

    [SerializeField] public List<GameObject> tilePrefabs;
    [SerializeField] GameObject spherePrefab;
    GameObject debugSphere;
    List<GameObject> debugSpheres = new List<GameObject>();

    public List<Cell> grid = new List<Cell>();

    private GameObject map;

    void Start()
    {

    }

    void Update()
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
    /// Main WFC function
    /// </summary>
    public void GenerateTiles()
    {
        GenerateNewMap();
        GenerateNewGrid();
    }

    // TODO: should be made public and return grid
    /// <summary>
    /// Creates cells for the new grid
    /// </summary>
    private void GenerateNewGrid()
    {
        grid.Clear();
        debugSpheres.Clear();

        debugSphere = new GameObject("debugSpheres");
        debugSphere.transform.parent = map.transform;

        for (int row = 0; row < gridDimensionSquared; row++)
        {
            for (int col = 0; col < gridDimensionSquared; col++)
            {
                int i = HelperFunctions.ConvertTo1dArray(row, col, gridDimensionSquared);

                Vector3 tilePositionOffset = new Vector3(-gridDimensionSquared * tileOffset / 2, 0, -gridDimensionSquared * tileOffset / 2);
                Vector3 tilePosition = tilePositionOffset + new Vector3(row * tileOffset, 0, col * tileOffset) + new Vector3(tileOffset / 2, 0, tileOffset / 2);

                Cell cell = new Cell(map, i, tilePosition, tilePrefabs);

                GameObject debugSphere = Instantiate(spherePrefab, tilePosition, Quaternion.identity);
                debugSphere.name = "Sphere (" + row + " , " + col + ")";
                debugSpheres.Add(debugSphere);
                debugSphere.transform.parent = this.debugSphere.transform;
                grid.Add(cell);
            }
        }
    }

    /// <summary>
    /// Creates an object to store everything in the hierarchy
    /// </summary>
    /// <returns> A new blank gameObject</returns>
    private void GenerateNewMap()
    {
        if (GameObject.Find(mapName))
        {
            DestroyImmediate(GameObject.Find(mapName));
        }
        map = new GameObject(mapName);
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

    /// <summary>
    /// instantiates the posibles tiles as gameobjects around the cell
    /// </summary>
    /// <param name="cell"> The given cell </param>
    public void ShowPossibleTilesetinCell(Cell cell)
    {
        int numberOfCells = cell.possibleTiles.Count;
        int gridDimension = (int)Mathf.Sqrt(numberOfCells);
        int i = 0;

        foreach (GameObject possibleTile in cell.possibleTiles)
        {
            GameObject tileInstance = Instantiate(possibleTile, cell.position, Quaternion.identity, debugSpheres[cell.CellIndex].transform);
            tileInstance.transform.localScale /= gridDimension;

            Vector3 cellPosition = new Vector3(HelperFunctions.ConvertTo2dArray(i, gridDimension).x * tileOffset * tileInstance.transform.localScale.x, 0, HelperFunctions.ConvertTo2dArray(i, gridDimension).y * tileOffset * tileInstance.transform.localScale.z);
            Vector3 cellPositionOffset = new Vector3(-tileOffset / 2 * tileInstance.transform.localScale.x, 0, -tileOffset / 2 * tileInstance.transform.localScale.z);

            tileInstance.transform.localPosition = cellPosition + cellPositionOffset;
            i++;
        }
    }
}
