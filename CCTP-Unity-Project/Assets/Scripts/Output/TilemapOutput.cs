using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class TilemapOutput : IOutputCreator<Tilemap>
    {
        private Tilemap outputImage;
        private ValuesManager<TileBase> valueManager;

        public Tilemap OutputImage => outputImage;


        public TilemapOutput(Tilemap _outputImage, ValuesManager<TileBase> _valueManager)
        {
            outputImage = _outputImage;
            valueManager = _valueManager;
        }

        public void CreateOutput(PatternManager manager, int[][] outputValues, int width, int height)
        {
            if (outputValues.Length == 0) return;

            outputImage.ClearAllTiles();

            int[][] valueGrid;
            valueGrid = manager.ConvertPatternToValues<TileBase>(outputValues);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    TileBase tile = (TileBase)valueManager.GetValueFromIndex(valueGrid[row][col]).Value;
                    outputImage.SetTile(new Vector3Int(col, row, 0), tile);
                }
            }
        }
    }
}