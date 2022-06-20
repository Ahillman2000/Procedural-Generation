using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Tile tile = (Tile)target;

        if (GUILayout.Button("Set valid neighbours"))
        {
            tile.SetValidTiles();
        }
    }
}
