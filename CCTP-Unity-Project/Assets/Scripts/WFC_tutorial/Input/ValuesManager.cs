using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using System;
using System.Linq;

namespace WaveFunctionCollapse 
{
    public class ValuesManager<T>
    {
        int[][] grid;
        Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
        int index = 0;

        public ValuesManager(IValue<T>[][] gridOfValues)
        {
            CreateGridOfIndices(gridOfValues);
        }

        private void CreateGridOfIndices(IValue<T>[][] _gridOfValues)
        {
            grid = MyCollectionExtensions.CreateJaggedArray<int[][]>(_gridOfValues.Length, _gridOfValues[0].Length);
            for (int row = 0; row < _gridOfValues.Length; row++)
            {
                for (int col = 0; col < _gridOfValues[0].Length; col++)
                {
                    SetIndexToGridPosition(_gridOfValues, row, col);
                }
            }
        }

        internal Vector2 getGridSize()
        {
            if(grid == null)
            {
                return Vector2.zero;
            }
            return new Vector2(grid[0].Length, grid.Length);
        }

        private void SetIndexToGridPosition(IValue<T>[][] gridOfValues, int row, int col)
        {
            if(valueIndexDictionary.ContainsValue(gridOfValues[row][col]))
            {
                var key = valueIndexDictionary.FirstOrDefault(x => x.Value.Equals(gridOfValues[row][col]));
                grid[row][col] = key.Key;
            }
            else
            {
                grid[row][col] = index;
                valueIndexDictionary.Add(grid[row][col], gridOfValues[row][col]);
                index++;
            }
        }

        public int GetGridValue(int x, int y)
        {
            if(x >= grid[0].Length || y >= grid.Length || x < 0 || y < 0)
            {
                throw new System.IndexOutOfRangeException("Grid doesnt contain x:" + x + ", y: " + y + "value");
            }
            return grid[y][x];
        }

        public IValue<T> GetValueFromIndex(int _index)
        {
            if(valueIndexDictionary.ContainsKey(_index))
            {
                return valueIndexDictionary[_index];
            }
            throw new System.Exception("No index: " + _index + " in valueDictionary");
        }

        public int GetGridValuesIncludingOffset(int x, int y)
        {
            int yMax = grid.Length;
            int xMax = grid[0].Length;

            if(x < 0 && y < 0)
            {
                return GetGridValue(xMax + x, yMax + y);
            }
            if(x < 0 && y >= yMax)
            {
                return GetGridValue(xMax + x, y - yMax);
            }
            if(x >= xMax && y < 0)
            {
                return GetGridValue(x - xMax, yMax + y);
            }
            if (x >= xMax && y >= yMax)
            {
                return GetGridValue(x - xMax, y - yMax);
            }
            if (x < 0)
            {
                return GetGridValue(xMax + x, y);
            }
            if(x >= xMax)
            {
                return GetGridValue(x - xMax, y);
            }
            if (y < 0)
            {
                return GetGridValue(x, yMax + y);
            }
            if (y >= yMax)
            {
                return GetGridValue(x, y - yMax);
            }
            return GetGridValue(x, y);
        }

        public int[][] GetPatternValuesFromGridAt(int x, int y , int _patternSize)
        {
            int[][] arrayToReturn = MyCollectionExtensions.CreateJaggedArray<int[][]>(_patternSize, _patternSize);
            for (int row = 0; row < _patternSize; row++)
            {
                for (int col = 0; col < _patternSize; col++)
                {
                    arrayToReturn[row][col] = GetGridValuesIncludingOffset(x + col, y + row);
                }
            }
            return arrayToReturn;
        }
    }
}
