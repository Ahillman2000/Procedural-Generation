using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCAlgorithm : MonoBehaviour
{
    public static WFCAlgorithm Instance { get; private set; }
    
    [HideInInspector] public GridGenerator GridGenerator;
    [HideInInspector] public Solver Solver;

    [Header("Map Settings")]
    [SerializeField] private bool newMapAtRuntime = false;
    public string MapName = "===== MAP =====";
    [Range(2, 20)] public int GridDimension = 5;
    //public int gridWidth;
    //public int gridLength;

    [Header("Tile Settings")]
    public float SizeOfTiles = 30f;
    public TilesetSO Tileset;

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
        GridGenerator = new GridGenerator(this);
        Solver = new Solver(this);

        GridGenerator.GenerateGrid();
        Solver.Solve();
        //return gridGenerator.Map;
    }
}
