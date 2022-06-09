using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Helpers;
using System.Text;

namespace WaveFunctionCollapse
{
    public class OutputGrid
    {
        Dictionary<int, HashSet<int>> indexPossiblePatternDictionary = new Dictionary<int, HashSet<int>>();
        public int Width { get; } 
        public int Height { get; }

        private int maxNumberOfPatterns = 0;

        public OutputGrid(int _width, int _height, int _numberOfPatterns)
        {
            Width = _width;
            Height = _height;
            maxNumberOfPatterns = _numberOfPatterns;

            ResetAllPossibilities();
        }

        public void ResetAllPossibilities()
        {
            HashSet<int> allPossiblePatternList = new HashSet<int>();
            allPossiblePatternList.UnionWith(Enumerable.Range(0, maxNumberOfPatterns).ToList());

            indexPossiblePatternDictionary.Clear();
            for (int i = 0; i < Height * Width; i++)
            {
                indexPossiblePatternDictionary.Add(i, new HashSet<int>(allPossiblePatternList));
            }
        }

        public bool CheckCellExists(Vector2Int positon)
        {
            int index = GetIndexFromCoordinates(positon);
            return indexPossiblePatternDictionary.ContainsKey(index);
        }

        private int GetIndexFromCoordinates(Vector2Int positon)
        {
            return positon.x + Width * positon.y;
        }

        private Vector2Int GetCoordinatesFromIndex(int randomIndex)
        {
            Vector2Int coordinates = Vector2Int.zero;
            coordinates.x = randomIndex / Width;
            coordinates.y = randomIndex / Height;
            return coordinates;
        }

        internal void PrintResultsToConsole()
        {
            StringBuilder builder = null;
            List<string> list = new List<string>();

            for (int row = 0; row < Height; row++)
            {
                builder = new StringBuilder();
                for (int col = 0; col < Width; col++)
                {
                    var result = GetPossibleValueForPosition(new Vector2Int(col, row));
                    if(result.Count == 1)
                    {
                        builder.Append(result.First() + " ");
                    }
                    else
                    {
                        string newString = "";
                        foreach (var item in result)
                        {
                            newString += item + ",";
                        }
                        builder.Append(newString);
                    }
                }
                list.Add(builder.ToString());
            }

            list.Reverse();
            foreach (var item in list)
            {
                Debug.Log(item);
            }
            Debug.Log("---");
        }

        public bool IsCellCollapsed(Vector2Int posisiton)
        {
            return GetPossibleValueForPosition(posisiton).Count <= 1;
        }

        public HashSet<int> GetPossibleValueForPosition(Vector2Int posisiton)
        {
            int index = GetIndexFromCoordinates(posisiton);
            if(indexPossiblePatternDictionary.ContainsKey(index))
            {
                return indexPossiblePatternDictionary[index];
            }
            return new HashSet<int>();
        }

        public bool IsGridSolved()
        {
            return !indexPossiblePatternDictionary.Any(x => x.Value.Count > 1);
        }

        internal bool IsPositionValid(Vector2Int position)
        {
            return HelperFunctions.ValidateCoords(position.x, position.y, Width, Height);
        }

        public Vector2Int GetRandomCell()
        {
            int randomIndex = UnityEngine.Random.Range(0, indexPossiblePatternDictionary.Count);
            return GetCoordinatesFromIndex(randomIndex);
        }

        public void SetPatternAtPosition(int x, int y, int patternIndex)
        {
            int index = GetIndexFromCoordinates(new Vector2Int(x, y));
            indexPossiblePatternDictionary[index] = new HashSet<int>() { patternIndex };
        }

        public int[][] GetSolvedOutputGrid()
        {
            int[][] returnGrid = HelperFunctions.CreateJaggedArray<int[][]>(Height, Width);
            if(!IsGridSolved())
            {
                return HelperFunctions.CreateJaggedArray<int[][]>(0, 0);
            }
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    int index = GetIndexFromCoordinates(new Vector2Int(col, row));
                    returnGrid[row][col] = indexPossiblePatternDictionary[index].First();
                }
            }
            return returnGrid;
        }
    }
}

