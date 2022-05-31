using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WFCTestScript))]
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
}
