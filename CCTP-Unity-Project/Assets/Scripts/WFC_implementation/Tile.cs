using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class Tile : MonoBehaviour
{
    public Socket[] sockets = new Socket[4];

    // make tis list into a SO and then drag reference into scripts instead
    [SerializeField] private TilePrefabSO tileSet;
    public List<GameObject> validNeighbouringTiles = new List<GameObject>();

    public List<List<GameObject>> neighbourList = new List<List<GameObject>>();

    public void SetValidTiles()
    {
        validNeighbouringTiles.Clear();

        foreach (GameObject prefab in tileSet.tilePrefabs)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Socket thisSocket  = sockets[(int)direction];
                Socket otherSocket = prefab.GetComponent<Tile>().sockets[(int)direction.GetOppositeDirection()];

                if(otherSocket.value == thisSocket.value && !validNeighbouringTiles.Contains(prefab))
                {
                    validNeighbouringTiles.Add(prefab);
                }
            }
        }
    }
}
