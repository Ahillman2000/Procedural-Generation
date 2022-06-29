using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCAlgorithm : MonoBehaviour
{
    [SerializeField] private CameraHeight camHeight;

    public static WFCAlgorithm Instance { get; set; } = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = (WFCAlgorithm)FindObjectOfType(typeof(WFCAlgorithm));
        else
            Instance = this;
    }

    public void Start()
    {
    }

    /// <summary>
    /// The complete WFC algorithm to create a grid and solve it
    /// </summary>
    public void Execute()
    {
        GridGenerator.Instance.GenerateGrid();
        Solver.Instance.Solve();
        camHeight.SetCameraHeight();
    }
}
