using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCAlgorithm : MonoBehaviour
{
    public static WFCAlgorithm Instance { get; private set; }
    
    public GridGenerator GridGenerator;
    public Solver Solver;

    [Header("Map Settings")]
    [SerializeField] private bool newMapAtRuntime = false;
    public string MapName = "===== MAP =====";
    //public int gridWidth;
    //public int gridLength;

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
        if(newMapAtRuntime)
            Execute();
    }

    /// <summary>
    /// The complete WFC algorithm to create a grid and solve it
    /// </summary>
    public void Execute()
    {
        GridGenerator.GenerateGrid();
        Solver.Solve();
        //return gridGenerator.Map;
    }
}
