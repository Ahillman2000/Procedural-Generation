using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCAlgorithm : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private Solver solver;

    public void Start()
    {
        Execute();
    }

    /// <summary>
    /// The complete WFC algorithm to create a grid and solve it
    /// </summary>
    public void Execute()
    {
        gridGenerator.GenerateGrid();
        solver.Solve();
    }
}
