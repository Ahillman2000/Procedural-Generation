using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCAlgorithm : MonoBehaviour
{
    public static WFCAlgorithm Instance { get; set; } = null;

    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private Solver solver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"There should only be one instance of {this}");
            Destroy(this.gameObject);
        }
    }

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
        //return gridGenerator.Map;
    }
}
