using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileset", menuName = "New Tile Prefab List")]
public class TilesetSO : ScriptableObject
{
    public List<GameObject> prefabs = new List<GameObject>();
}
