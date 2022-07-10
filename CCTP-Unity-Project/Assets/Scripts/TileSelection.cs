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
            GameObject hitObject = hit.transform.gameObject;

            Vector2 hitPos2D = new Vector2(hitObject.transform.position.x, hitObject.transform.position.z);
            Vector2 minCellCoords2D = new Vector2(gridGenerator.debugSpheres[0].transform.position.x, gridGenerator.debugSpheres[0].transform.position.z);
            Vector2 newHitPos2D = hitPos2D - minCellCoords2D + new Vector2(gridGenerator.sizeOfTiles / 2, gridGenerator.sizeOfTiles / 2);
            Vector2 hitCellCoords2D = new Vector2(newHitPos2D.x / gridGenerator.sizeOfTiles, newHitPos2D.y / gridGenerator.sizeOfTiles);
            int hitCellIndex = (int)(HelperFunctions.ConvertTo1dArray((int)hitCellCoords2D.x, (int)hitCellCoords2D.y, gridGenerator.gridDimension));

            Tile tile = hitObject.GetComponentInParent(typeof(Tile)) as Tile;
            if (tile != null)
            {
                gridGenerator.grid[hitCellIndex].SetTile(tile.prefab);
                solver.Propagate(gridGenerator.grid[hitCellIndex]);
            }
        }
    } 
}
