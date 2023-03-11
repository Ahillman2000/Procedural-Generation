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
            /*EditorUtility.SetDirty(tileset.prefabs);
            tileset.SetPrefabIndexes();
            PrefabUtility.RecordPrefabInstancePropertyModifications(tileset);*/

            for (int i = 0; i < tileset.prefabs.Count; i++)
            {
                EditorUtility.SetDirty(tileset.prefabs[i]);
                tileset.prefabs[i].GetComponent<Tile>().prefabIndex = i;
                PrefabUtility.RecordPrefabInstancePropertyModifications(tileset.prefabs[i]);
            }
        }
    }
}
