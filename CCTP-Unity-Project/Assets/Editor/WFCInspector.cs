using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WFCAlgorithm))]
public class WFCInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WFCAlgorithm wfc = (WFCAlgorithm)target;

        if (GUILayout.Button("WFC"))
        {
            wfc.Execute();
        }
    }
}