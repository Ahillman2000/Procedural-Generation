using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class InputImageParameters
    {
        Vector2Int? bottomRightTileCoords = null;
        Vector2Int? TopLeftTileCoords     = null;

        BoundsInt inputTileMapsBounds;

        TileBase[] inputTilemapTilesArray;

        Queue<TileContainer> stackOfTiles = new Queue<TileContainer>();

        private int width  = 0;
        private int height = 0;

        private Tilemap inputTilemap;

        public Queue<TileContainer> StackOfTiles { get => stackOfTiles; set => stackOfTiles = value; }
        public int Width { get => width; }
        public int Height { get => height; }


        public InputImageParameters(Tilemap inputTilemap)
        {
            this.inputTilemap = inputTilemap;
            this.inputTileMapsBounds = this.inputTilemap.cellBounds;
            this.inputTilemapTilesArray = this.inputTilemap.GetTilesBlock(this.inputTileMapsBounds);
            ExtractNonEmptyTiles();
            VerifyInputTiles();
        }

        private void VerifyInputTiles()
        {
            //throw new NotImplementedException();
            if(TopLeftTileCoords == null || bottomRightTileCoords == null)
            {
                throw new System.Exception("WFC: Input tilemap is empty");
            }

            int minX = bottomRightTileCoords.Value.x;
            int maxX = TopLeftTileCoords.Value.x;

            int minY = bottomRightTileCoords.Value.y;
            int maxY = TopLeftTileCoords.Value.y;

            width  = Math.Abs(maxX - minX) + 1;
            height = Math.Abs(maxY - minY) + 1;

            int tileCount = width * height;
            if(stackOfTiles.Count != tileCount)
            {
                throw new System.Exception("WFC: tilemap has empty fields");
            }
            if(stackOfTiles.Any(tile => tile.X > maxX || tile.X < minX || tile.Y > maxY || tile.Y < minY))
            {
                throw new System.Exception("WFC: Tilemap image should be a filled rectangle");
            }
        }

        private void ExtractNonEmptyTiles()
        {
            for (int row = 0; row < inputTileMapsBounds.size.y; row++)
            {
                for (int col = 0; col < inputTileMapsBounds.size.x; col++)
                {
                    int index = col + (row * inputTileMapsBounds.size.x);
                    TileBase tile = inputTilemapTilesArray[index];
                    if (bottomRightTileCoords == null && tile != null)
                    {
                        bottomRightTileCoords = new Vector2Int(col, row);
                    }
                    if(tile != null)
                    {
                        stackOfTiles.Enqueue(new TileContainer(tile, col, row));
                        TopLeftTileCoords = new Vector2Int(col, row);
                    }
                }
            }
        }
    }
}
