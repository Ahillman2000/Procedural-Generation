using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class Tile : MonoBehaviour
{
    public Socket[] sockets = new Socket[4];

    [SerializeField] private TilesetSO tileSet;

    [HideInInspector] public int prefabIndex;

    [Serializable]
    public class NeigboursInDirection
    {
        public List<GameObject> neighbours;
    }

    public List<NeigboursInDirection> neighbourList;

    /// <summary>
    /// runs when the script is added as component, sets socket values and neighour list to default
    /// </summary>
    private void Reset()
    {
        for (int i = 0; i < sockets.Length; i++)
        {
            sockets[i] = new Socket((Direction)i, 0);
        }

        neighbourList = new List<NeigboursInDirection>();

        for (int i = 0; i < 4; i++)
        {
            neighbourList.Add(null);
        }
    }

    /// <summary>
    /// sets valid tiles for each directional socket
    /// </summary>
    public void SetValidTiles()
    {
        int i = 0;
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            foreach (GameObject prefab in tileSet.prefabs)
            {
                Socket thisSocket = sockets[(int)direction];
                Socket otherSocket = prefab.GetComponent<Tile>().sockets[(int)direction.GetOppositeDirection()];

                if (otherSocket.value == thisSocket.value && !neighbourList[i].neighbours.Contains(prefab))
                {
                    neighbourList[i].neighbours.Add(prefab);
                }
            }
            i++;
        }
    }
}