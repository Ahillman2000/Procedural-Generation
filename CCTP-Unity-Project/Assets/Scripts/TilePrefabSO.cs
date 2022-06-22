using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileset", menuName = "New Tile Prefab List")]
public class TilePrefabSO : ScriptableObject
{
    public List<GameObject> tilePrefabs = new List<GameObject>();
}
