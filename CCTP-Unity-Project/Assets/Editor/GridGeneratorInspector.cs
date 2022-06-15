using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        GridGenerator gj = (GridGenerator)target;

        if (GUILayout.Button("GenerateGrid"))
        {
            gj.GenerateGrid();
        }
    }
}
