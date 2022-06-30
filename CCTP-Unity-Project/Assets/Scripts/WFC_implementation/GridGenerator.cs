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

        //ShowPossibleTilesetinCell(grid[0]);

        foreach (Cell cell in grid)
        {
            ShowPossibleTilesetinCell(cell);
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
    /// Determines if given number is perfect square
    /// </summary>
    /// <param name="n"> The number to check against </param>
    /// https://www.geeksforgeeks.org/check-if-given-number-is-perfect-square-in-cpp/
    /// <returns></returns>
    static bool IsPerfectSquare(int n)
    {

        // Find floating point value of
        // square root of x.
        if (n >= 0)
        {
            int root = (int)Math.Sqrt(n);

            // if product of square root
            // is equal, then
            // return T/F
            return (root * root == n);
        }
        // else return false if n<0
        return false;
    }

    /// <summary>
    /// Gets the next largest perfect square of a given number
    /// </summary>
    /// <param name="n"> The number to check against </param>
    /// https://www.geeksforgeeks.org/find-the-next-perfect-square-greater-than-a-given-number/
    /// <returns></returns>
    private int GetNextPerfectSquare(int n)
    {
        double root = Math.Sqrt(n);
        int nextN = (int)Math.Floor(root) + 1;
        int nextPerfectSquare = (int)Math.Pow(nextN, 2);
        return nextPerfectSquare;
    }

    /// <summary>
    /// Gets the perfect square of a given number
    /// </summary>
    /// <param name="n"> The number to cehck against </param>
    /// <returns></returns>
    private int GetPerfectSquare(int n)
    {
        if(IsPerfectSquare(n))
        {
            return n;
        }
        else
        {
            return GetNextPerfectSquare(n);
        }
    }

    /// <summary>
    /// instantiates the posibles tiles as gameobjects around the cell
    /// </summary>
    /// <param name="cell"> The given cell </param>
    public void ShowPossibleTilesetinCell(Cell cell)
    {
        int numberOfCells = cell.possibleTiles.Count;
        int gridDimension = (int)Mathf.Sqrt(GetPerfectSquare(numberOfCells));

        for (int row = 0; row < gridDimension; row++)
        {
            for (int col = 0; col < gridDimension; col++)
            {
                Vector3 tilePositionOffset = new Vector3(-gridDimension * gridDimension / 2, 0, -gridDimension * gridDimension / 2);
                Vector3 tilePositioning = new Vector3(row * gridDimension, 0, col * gridDimension) + new Vector3(gridDimension / 2, 0, gridDimension / 2);
                Vector3 tilePosition = cell.position + tilePositionOffset + tilePositioning;

                int index = HelperFunctions.ConvertTo1dArray(row, col, gridDimension);
                if (index < numberOfCells)
                {
                    GameObject tileInstance = Instantiate(cell.possibleTiles[index], tilePosition, Quaternion.identity);
                    tileInstance.transform.parent = debugSpheres[cell.CellIndex].transform;
                    tileInstance.transform.localScale /= GetPerfectSquare(numberOfCells)/2;
                    tileInstance.AddComponent<PossibleTileInstance>();
                    tileInstance.GetComponent<PossibleTileInstance>().prefab = cell.possibleTiles[index];
                    //debugSphere.name = "Sphere (" + row + " , " + col + ")";
                }
                else
                {
                    GameObject debugSphere = Instantiate(spherePrefab, tilePosition, Quaternion.identity);
                    debugSphere.transform.parent = debugSpheres[cell.CellIndex].transform;
                    debugSphere.name = "Sphere (" + row + " , " + col + ")";
                }
            }
        }
    }

    /*
    //TODO: extract out into own script
    public void ManuallyAssignTileToCell()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPosition = hit.point;
            GameObject hitObject = hit.transform.gameObject;

            Vector3 hitCellPos = HitToCellPos(hitPosition, tileOffset);
            Vector2Int hitCellCoords2D = new Vector2Int((int)(hitCellPos.x + tileOffset), (int)(hitCellPos.z + tileOffset));
            int hitCellIndex = (int)(HelperFunctions.ConvertTo1dArray(hitCellCoords2D.x, hitCellCoords2D.y, gridDimension) / tileOffset);

            if (hitObject.transform.parent != null && hitObject.transform.parent.GetComponent<Tile>() != null)
            {
                Debug.Log("Hit " + hitObject.transform.parent.gameObject);
                Debug.Log("Hit tile belonging to cell: " + hitCellIndex);
                //grid[hitCellIndex].SetTile(hitObject.transform.parent.gameObject);

                // can only be used in editor
                PrefabUtility.GetCorrespondingObjectFromOriginalSource(hitObject);
            }
        }
    }

    /// <summary>
    /// https://www.reddit.com/r/Unity3D/comments/38bvns/round_to_even_number_or_nearest_5_in_c/
    /// </summary>
    public Vector3 HitToCellPos(Vector3 input, float factor)
    {
        if (factor <= 0f)
            throw new UnityException("factor argument must be above 0");

        float x = Mathf.Round(input.x / factor) * factor;
        float y = Mathf.Round(input.y / factor) * factor;
        float z = Mathf.Round(input.z / factor) * factor;

        return new Vector3(x, y, z);
    }
    */
}
