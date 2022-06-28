using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeigboursInDirection
{
    public List<GameObject> list;
}

[CreateAssetMenu(fileName = "NewTilePrototype", menuName = "Create New Tile Prototype")]
public class TilePrototypeSO : ScriptableObject
{
    public new string name;
    public Mesh meshData;
    [Range(0,3)]
    public int rotation;
    public Socket[] sockets = new Socket[4];

    [SerializeField] private TilesetSO tileSet;

    public List<NeigboursInDirection> validNeighbours = new List<NeigboursInDirection>();
}
