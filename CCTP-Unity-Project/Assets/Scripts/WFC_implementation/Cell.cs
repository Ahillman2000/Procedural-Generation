using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

[Serializable]
public class Cell
{
    public int CellIndex { get; set; } = 0;
    public Vector3 position = Vector3.zero;
    public List<GameObject> possibleTiles;

    public bool Collapsed { get; set; } = false;

    [SerializeField] private GameObject tile;
    readonly GameObject parentObj;

    /// <summary>
    /// Creates a new cell with the following params
    /// </summary>
    /// <param name="parentObj"> The parent object that the final tile with be a child of </param>
    /// <param name="cellIndex"> The index of the cell within a grid </param>
    /// <param name="position">  The world space of the object </param>
    /// <param name="possibleTiles"> The possible tiles that this cell could be </param>
    public Cell(GameObject parentObj, int cellIndex, Vector3 position, List<GameObject> possibleTiles)
    {
        this.CellIndex = cellIndex;
        this.position = position;
        this.possibleTiles = new List<GameObject>(possibleTiles);
        this.parentObj = parentObj;
    }

    /// <summary>
    /// Sets the cells tile, called when collapsed to a single possible tile value. Also creates an object of that tile
    /// </summary>
    /// <param name="tile"> The tile value to be set to </param>
    public void SetTile(GameObject tile)
    {
        this.tile = tile;
        GameObject tileObj = GameObject.Instantiate(tile, position, Quaternion.identity, parentObj.transform);
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
        }
        else
        {
            return;
        }

        if (possibleTiles.Count == 1)
        {
            Collapsed = true;
            SetTile(possibleTiles[0]);
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
    public GameObject GetTile()
    {
        if(tile != null)
        {
            return tile;
        }
        return null;
    }
}
