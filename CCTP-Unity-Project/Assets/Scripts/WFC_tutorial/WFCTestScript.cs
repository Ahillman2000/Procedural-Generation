using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse;
using UnityEditor;

public class WFCTestScript : MonoBehaviour
{
    public Tilemap inputTilemap;
    public Tilemap outputTilemap;

    public int patternSize;

    public int maxIterations = 500;

    public int outputWidth = 5;
    public int outputHeight = 5;

    public bool equalWeights = false;

    ValuesManager<TileBase> valueManager;
    PatternManager manager;
    WFCCore core;
    TilemapOutput output;

    void Start()
    {
        CreateWaveFunctionCollapse();
    }

    public void CreateWaveFunctionCollapse()
    {
        InputReader reader = new InputReader(inputTilemap);
        var grid = reader.ReadInputToGrid();

        valueManager = new ValuesManager<TileBase>(grid);
        manager = new PatternManager(2);
        manager.ProcessGrid(valueManager, equalWeights);

        core = new WFCCore(5, 5, 500, manager);
    }

    public void CreateTilemap()
    {
        output = new TilemapOutput(outputTilemap, valueManager);
        var result = core.CreateOutputGrid();
        output.CreateOutput(manager, result, outputWidth, outputHeight);
    }

    public void SaveTileMap()
    {
        if(output.OutputImage != null)
        {
            outputTilemap = output.OutputImage;
            GameObject objectToSave = outputTilemap.gameObject;

            PrefabUtility.SaveAsPrefabAsset(objectToSave, "Assets/Saved/output.prefab");
        }
    }
}
