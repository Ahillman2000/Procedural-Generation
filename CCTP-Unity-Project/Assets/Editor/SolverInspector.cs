using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Solver))]
public class SolverInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Solver solver = (Solver)target;

        if (GUILayout.Button("Single WFC Iteration"))
        {
            solver.Iterate();
        }
        // TODO: UNLIKELY TO WORK due to coroutine in editor
        if (GUILayout.Button("Solve"))
        {
            solver.Solve();
        }
    }
}