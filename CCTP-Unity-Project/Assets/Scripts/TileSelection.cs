using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using System.Linq;
using System;

public class TileSelection : MonoBehaviour
{
    GridGenerator gridGenerator;
    Solver solver;

    private void Start()
    {
        gridGenerator = GridGenerator.Instance;
        solver = Solver.Instance;
    }

    /// <summary>
    /// instantiates the posibles tiles as gameobjects around the cell
    /// </summary>
    /// <param name="cell"> The given cell </param>
    /*public void ShowPossibleTilesetinCell(Cell cell)
    {
        int numberOfCells = cell.possibleTiles.Count;
        int gridDimension = (int)Mathf.Sqrt(HelperFunctions.GetPerfectSquare(numberOfCells));

        //instancesInCell.Add(default);
        instancesInCell = new List<InstancesInCell>();

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

                    tileInstance.transform.parent = gridGenerator.debugSpheres[cell.CellIndex].transform;
                    tileInstance.transform.localScale /= HelperFunctions.GetPerfectSquare(numberOfCells) / 2;

                    instancesInCell[cell.CellIndex].instances.Add(tileInstance);
                }
                else
                {
                    GameObject debugSphere = Instantiate(gridGenerator.spherePrefab, tilePosition, Quaternion.identity);
                    debugSphere.transform.parent = gridGenerator.debugSpheres[cell.CellIndex].transform;
                    debugSphere.name = "Sphere (" + row + " , " + col + ")";
                }
            }
        }
    }*/

    /// Runs the tile selector when left mouse button clicked
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ManuallyCollapseCellToTile();
        }
    }

    /// <summary>
    /// If the user clicks on a tile of cell then this is tile that the cell is collapsed to
    /// </summary>
    public void ManuallyCollapseCellToTile()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPosition = hit.point;
            GameObject hitObject = hit.transform.gameObject;

            Vector3 hitCellPos = HitToCellPos(hitPosition, gridGenerator.sizeOfTiles);
            Vector2Int hitCellCoords2D = new Vector2Int((int)(hitCellPos.x + gridGenerator.sizeOfTiles), (int)(hitCellPos.z + gridGenerator.sizeOfTiles));
            int hitCellIndex = (int)(HelperFunctions.ConvertTo1dArray(hitCellCoords2D.x, hitCellCoords2D.y, gridGenerator.gridDimension) / gridGenerator.sizeOfTiles);

            Tile tile = hitObject.GetComponentInParent(typeof(Tile)) as Tile;
            if (tile != null)
            {
                //Debug.Log("Hit tile belonging to cell: " + hitCellIndex);
                //Debug.Log("Assigning tile: " + tile.prefab);
                
                gridGenerator.grid[hitCellIndex].SetTile(tile.prefab);
                solver.Propagate(gridGenerator.grid[hitCellIndex]);
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
}
