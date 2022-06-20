using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTilePrototype", menuName = "Create New Tile Prototype")]
public class TilePrototypeSO : ScriptableObject
{
    public new string name;
    public Mesh meshData;
    public Vector3 rotation;
    public Socket[] sockets = new Socket[4];

    [SerializeField] private TilePrefabSO tileSet;

    public class NeigboursInDirection
    {
        public List<GameObject> list;
    }

    public List<NeigboursInDirection> neighbourList = new List<NeigboursInDirection>();
}
