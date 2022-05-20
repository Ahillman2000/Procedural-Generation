using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using System;

namespace WaveFunctionCollapse
{
    public class Pattern
    {
        private int index;
        private int[][] grid;

        public string HashIndex { get; set; }
        public int Index { get => index; }

        public Pattern(int[][] _grid, string _hashcode, int _index)
        {
            grid = _grid;
            HashIndex = _hashcode;
            index = _index;
        }

        public void SetGridValue(int x, int y, int value)
        {
            grid[y][x] = value;
        }

        public int GetGridValue(int x, int y)
        {
            return grid[y][x];
        }

        public bool CheckValueAtPosition(int x, int y, int index)
        {
            return index.Equals(GetGridValue(x, y));
        }

        public bool ComparePattern(Direction _dir, Pattern _pattern)
        {
            int[][] grid = GetGridValuesInDirection(_dir);
            int[][] otherGrid = _pattern.GetGridValuesInDirection(_dir.GetOppositeDirection());

            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[0].Length; col++)
                {
                    if (grid[row][col] != otherGrid[row][col]) return false;
                }
            }
            return true;
        }

        private int[][] GetGridValuesInDirection(Direction _dir)
        {
            int[][] gridPartToCompare;

            switch (_dir)
            {
                case Direction.Up:
                    gridPartToCompare = MyCollectionExtensions.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 1, grid.Length, gridPartToCompare);
                    break;
                case Direction.Down:
                    gridPartToCompare = MyCollectionExtensions.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 0, grid.Length - 1, gridPartToCompare);
                    break;
                case Direction.Left:
                    gridPartToCompare = MyCollectionExtensions.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.Right:
                    gridPartToCompare = MyCollectionExtensions.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(1, grid.Length, 0, grid.Length, gridPartToCompare);
                    break;
                default:
                    return grid;
            }
            return gridPartToCompare;
        }

        private void CreatePartOfGrid(int xMin, int xMax, int yMin, int yMax, int[][] _gridPartToCompare)
        {
            List<int> tempList = new List<int>();

            for (int row = yMin; row < yMax; row++)
            {
                for (int col = xMin; col < xMax; col++)
                {
                    tempList.Add(grid[row][col]);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                int x = i % _gridPartToCompare.Length;
                int y = i / _gridPartToCompare.Length;

                _gridPartToCompare[x][y] = tempList[i];
            }
        }
    }
}
