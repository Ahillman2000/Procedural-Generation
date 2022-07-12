using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilesetSO))]
public class TilesetSOInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TilesetSO tileset = (TilesetSO)target;

        if (GUILayout.Button("Confirm tile set"))
        {
            EditorUtility.SetDirty(tileset);
            tileset.SetPrefabIndexes();
            PrefabUtility.RecordPrefabInstancePropertyModifications(tileset);
        }
    }
}
