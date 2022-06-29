using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using Helpers;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] string mapName = "===== MAP =====";

    [Range(2, 20)] public int gridDimension = 2;

    [SerializeField] private float sizeOfTiles = 5f;

    public TilesetSO tileset;
    [SerializeField] private GameObject spherePrefab;
    private GameObject debugSphere;
    private List<GameObject> debugSpheres = new List<GameObject>();

    public List<Cell> grid = new List<Cell>();

    private GameObject map;

    public static GridGenerator Instance { get; set; } = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = (GridGenerator)FindObjectOfType(typeof(GridGenerator));
        else
            Instance = this;
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
        GenerateNewMap();
        GenerateNewGrid();

        Camera cam = Camera.main;
        Vector3 camPos = cam.transform.position;
        cam.transform.position = new Vector3(camPos.x, gridDimension * 7, camPos.z);

        /*foreach (Cell cell in grid)
        {
            ShowPossibleTilesetinCell(cell);
        }*/
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

        for (int row = 0; row < gridDimension; row++)
        {
            for (int col = 0; col < gridDimension; col++)
            {
                int i = HelperFunctions.ConvertTo1dArray(row, col, gridDimension);

                Vector3 tilePositionOffset = new Vector3(-gridDimension * sizeOfTiles / 2, 0, -gridDimension * sizeOfTiles / 2);
                Vector3 tilePosition = tilePositionOffset + new Vector3(row * sizeOfTiles, 0, col * sizeOfTiles) + new Vector3(sizeOfTiles / 2, 0, sizeOfTiles / 2);

                Cell cell = new Cell(map, i, tilePosition, tileset.prefabs);

                GameObject debugSphere = Instantiate(spherePrefab, tilePosition, Quaternion.identity);
                debugSphere.name = "Sphere (" + row + " , " + col + ")";
                debugSpheres.Add(debugSphere);
                debugSphere.transform.parent = this.debugSphere.transform;
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

            Vector3 cellPosition = new Vector3(HelperFunctions.ConvertTo2dArray(i, gridDimension).x * sizeOfTiles * tileInstance.transform.localScale.x, -5, HelperFunctions.ConvertTo2dArray(i, gridDimension).y * sizeOfTiles * tileInstance.transform.localScale.z);
            Vector3 cellPositionOffset = new Vector3(-sizeOfTiles / 2 * tileInstance.transform.localScale.x, 0, -sizeOfTiles / 2 * tileInstance.transform.localScale.z);

            tileInstance.transform.localPosition = cellPosition + cellPositionOffset;
            i++;
        }
    }
}
