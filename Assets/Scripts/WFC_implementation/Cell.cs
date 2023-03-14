using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

[Serializable]
public class Cell
{
    public int CellIndex { get; set; } = 0;
    public Vector3 cellPosition = Vector3.zero;

    public List<GameObject> possibleTiles;
    //public List<GameObject> tileInstances = new List<GameObject>();

    public bool Collapsed { get; set; } = false;

    [SerializeField] private GameObject tile;
    private GameObject parentObj;

    private Solver solver;

    /// <summary>
    /// Creates a new cell with the following params
    /// </summary>
    /// <param name="parentObj"> The parent object that the final tile with be a child of </param>
    /// <param name="cellIndex"> The index of the cell within a grid </param>
    /// <param name="cellPosition">  The world space of the object </param>
    /// <param name="possibleTiles"> The possible tiles that this cell could be </param>
    public Cell(Solver solver, GameObject parentObj, int cellIndex, Vector3 cellPosition, List<GameObject> possibleTiles)
    {
        this.solver = solver;
        this.CellIndex = cellIndex;
        this.cellPosition = cellPosition;
        this.possibleTiles = new List<GameObject>(possibleTiles);
        this.parentObj = parentObj;
    }

    /// <summary>
    /// Selects a random tile from a give list of objects
    /// </summary>
    /// <param name="tiles"> The list of objects to iterate through </param>
    /// <returns> The randomly selected tile </returns>
    public GameObject SelectRandomPossibleTile()
    {
        return possibleTiles[UnityEngine.Random.Range(0, possibleTiles.Count)];
    }

    /// <summary>
    /// Should be used to remove an invalid tile from the possible tileset
    /// </summary>
    /// <param name="tileToRemove"> The tile to be removed from the possible tileset </param>
    public void RemovePossibleTile(GameObject tileToRemove)
    {
        if(possibleTiles.Contains(tileToRemove))
        {
            possibleTiles.Remove(tileToRemove);

            if (possibleTiles.Count == 1)
            {
                SetTile(possibleTiles[0]);
            }
        }
    }

    /// <summary>
    /// Sets the cells tile, called when collapsed to a single possible tile value. Also creates an object of that tile
    /// </summary>
    /// <param name="tile"> The tile value to be set to </param>
    public void SetTile(GameObject tile)
    {
        if(this.tile == null)
        {
            Collapsed = true;
            this.tile = tile;
            GameObject instance = GameObject.Instantiate(tile, cellPosition, Quaternion.identity, parentObj.transform);
            instance.transform.localScale = Vector3.one;
            solver.OnCellCollapse();
            //RemoveAllPossibleTileInstancesinCell();
        }
    }

    /// <summary>
    /// Should be used to add a valid tile to the possible tileset
    /// </summary>
    /// <param name="tileToAdd"> The tile to be added to the possible tileset </param>
    public void AddPossibleTile(GameObject tileToAdd)
    {
        foreach (GameObject possibleTile in possibleTiles)
        {
            if (tileToAdd == possibleTile)
            {
                return;
            }
        }
        possibleTiles.Add(tileToAdd);
    }

    /// <summary>
    /// Get the entropy of the cell
    /// </summary>
    /// <returns> The number of possible tiles remaining for the cell /returns>
    public int GetEntropy()
    {
        return possibleTiles.Count;
    }

    /// <summary>
    /// Get the tile
    /// </summary>
    /// <returns> the tile object of this cell </returns>
    public Tile GetTile()
    {
        if(tile != null)
        {
            if (tile.GetComponent<Tile>() != null)
            {
                return tile.GetComponent<Tile>();
            }
            else
            {
                Debug.LogError("No tile component attached to cell:" + CellIndex);
                return null;
            }
        }
        else
        {
            Debug.LogError("No tile gameObject assigned to cell: " + CellIndex);
            return null;
        }
    }

    /*public void ShowPossibleTileInstancesinCell()
    {
        if (tileInstances.Count > 0)
        {
            RemoveAllPossibleTileInstancesinCell();
        }
        if(Collapsed)
        {
            return;
        }

        int numberOfCells = GridGenerator.Instance.tileset.prefabs.Count;
        int gridDimension = (int)Mathf.Sqrt(HelperFunctions.GetPerfectSquare(numberOfCells));

        if(numberOfCells > 1)
        {
            for (int row = 0; row < gridDimension; row++)
            {
                for (int col = 0; col < gridDimension; col++)
                {
                    Vector3 tilePositionOffset = new Vector3(-gridDimension * gridDimension / 2, 0, -gridDimension * gridDimension / 2) + new Vector3(gridDimension / 2, 0, gridDimension / 2);
                    Vector3 tilePositioning = new Vector3(row * gridDimension, 0, col * gridDimension);
                    Vector3 tilePosition = cellPosition + tilePositionOffset + tilePositioning;

                    int index = HelperFunctions.ConvertTo1dArray(row, col, gridDimension);
                    if (index < possibleTiles.Count)
                    {
                        GameObject tileInstance = GameObject.Instantiate(possibleTiles[index], tilePosition, Quaternion.identity);

                        tileInstance.transform.parent = GridGenerator.Instance.debugSpheres[CellIndex].transform;
                        tileInstance.transform.localScale /= HelperFunctions.GetPerfectSquare(numberOfCells) / 2;

                        tileInstances.Add(tileInstance);
                    }
                    else
                    {
                        GameObject debugSphere = GameObject.Instantiate(GridGenerator.Instance.spherePrefab, tilePosition, Quaternion.identity);
                        debugSphere.transform.parent = GridGenerator.Instance.debugSpheres[CellIndex].transform;
                        debugSphere.name = "Sphere (" + row + " , " + col + ")";

                        tileInstances.Add(debugSphere);
                    }
                }
            }
        }
    }

    public void RemoveAllPossibleTileInstancesinCell()
    {
        for (int i = 0; i < tileInstances.Count; i++)
        {
            GameObject.Destroy(tileInstances[i]);
        }
        tileInstances.Clear();
    }*/
}
