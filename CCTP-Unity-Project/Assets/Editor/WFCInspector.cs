using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*[CustomEditor(typeof(WFCTestScript))]
public class WFCInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WFCTestScript WFC = (WFCTestScript)target;
        if(GUILayout.Button("Create Tilemap"))
        {
            WFC.CreateWaveFunctionCollapse();
            WFC.CreateTilemap();
        }
        if(GUILayout.Button("Save Tilemap"))
        {
            WFC.SaveTileMap();
        }
    }
}*/

[CustomEditor(typeof(WFC))]
public class WFCInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WFC WFC = (WFC)target;
        if (GUILayout.Button("Collapse Cells"))
        {
            WFC.CollpaseCells();
        }
    }
}