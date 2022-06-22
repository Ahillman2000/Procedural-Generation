using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

// TODO: convert to scriptableObject
public class Tile : MonoBehaviour
{
    public Socket[] sockets = new Socket[4];

    [SerializeField] private TilesetSO tileSet;

    [Serializable]
    public class NeigboursInDirection
    {
        public List<GameObject> list;
    }

    public List<NeigboursInDirection> neighbourList = new List<NeigboursInDirection>();

    public void SetValidTiles()
    {
        int i = 0;
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            foreach (GameObject prefab in tileSet.prefabs)
            {
                Socket thisSocket = sockets[(int)direction];
                Socket otherSocket = prefab.GetComponent<Tile>().sockets[(int)direction.GetOppositeDirection()];

                if (otherSocket.value == thisSocket.value && !neighbourList[i].list.Contains(prefab))
                {
                    neighbourList[i].list.Add(prefab);
                }
            }
            i++;
        }
    }
}