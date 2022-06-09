using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Helpers;

namespace WaveFunctionCollapse
{
    public class PatternManager
    {
        Dictionary<int, PatternData> patternDataIndexDictionary;
        Dictionary<int, PatternNeighbours> patternPossibleNeighboursDictionary;

        int patternSize = -1;
        IFindNeighboursStrategy strategy;

        public PatternManager(int _patternSize)
        {
            patternSize = _patternSize;
        }

        public void ProcessGrid<T>(ValuesManager<T> valuemanager, bool equalWeights, string strategyName = null)
        {
            NeighbourStrategyFactory strategyFactory = new NeighbourStrategyFactory();
            strategy = strategyFactory.CreateInstance(strategyName==null ? patternSize+"" : strategyName);
            CreatePatterns(valuemanager, strategy, equalWeights);
        }

        private void CreatePatterns<T>(ValuesManager<T> valuemanager, IFindNeighboursStrategy strategy, bool equalWeights)
        {
            var patterFinderResult = PatternFinder.GetPatternDataFromGrid(valuemanager, patternSize, equalWeights);
            
            // Debugger to test correct patterns and neighbours
            /*StringBuilder builder = null;
            List<string> list = new List<string>();

            for (int row = 0; row < patterFinderResult.GetGridLengthY(); row++)
            {
                builder = new StringBuilder();
                for (int col = 0; col < patterFinderResult.GetGridLengthX(); col++)
                {
                 builder.Append(patterFinderResult.GetIndexAt(col, row) + " ");
                }
                list.Add(builder.ToString());
            }

            list.Reverse();
            foreach (var item in list)
            {
                Debug.Log(item);
            }*/

            patternDataIndexDictionary = patterFinderResult.PatternIndexDictionary;
            GetPatternNeighbours(patterFinderResult, strategy);
        }

        private void GetPatternNeighbours(PatternDataResults patterFinderResult, IFindNeighboursStrategy strategy)
        {
            patternPossibleNeighboursDictionary = PatternFinder.FindPossibleNeighboursForAllPatterns(strategy, patterFinderResult);
        }

        public PatternData GetPatternDataFromIndex(int index)
        {
            return patternDataIndexDictionary[index];
        }

        public HashSet<int> GetPossibleNeighboursForPatternInDirection(int patternIndex, Direction dir)
        {
            return patternPossibleNeighboursDictionary[patternIndex].GetNeighboursInDirection(dir);
        }

        public float GetPatternFrequency(int index)
        {
            return GetPatternDataFromIndex(index).RelativeFrequency;
        }

        public float GetPatternLog2Frequency(int index)
        {
            return GetPatternDataFromIndex(index).RelativeFrequencyLog2;
        }

        public int GetNumberOfPatterns()
        {
            return patternDataIndexDictionary.Count;
        }

        internal int[][] ConvertPatternToValues<T>(int[][] outputValues)
        {
            int patternOutputWidth  = outputValues[0].Length;
            int patternOutputHeight = outputValues.Length;

            int valueGridWidth  = patternOutputWidth  + patternSize - 1;
            int valueGridHeight = patternOutputHeight + patternSize - 1;

            int[][] valueGrid = HelperFunctions.CreateJaggedArray<int[][]>(valueGridHeight, valueGridWidth);

            for (int row = 0; row < patternOutputHeight; row++)
            {
                for (int col = 0; col < patternOutputWidth; col++)
                {
                    Pattern pattern = GetPatternDataFromIndex(outputValues[row][col]).Pattern;
                    GetPatternValues(patternOutputWidth, patternOutputHeight, valueGrid, row, col, pattern);
                }
            }
            return valueGrid;
        }

        private void GetPatternValues(int patternOutputWidth, int patternOutputHeight, int[][] valueGrid, int row, int col, Pattern pattern)
        {
            if(row == patternOutputHeight - 1 && col == patternOutputWidth - 1)
            {
                for (int row_2 = 0; row_2 < patternSize; row_2++)
                {
                    for (int col_2 = 0; col_2 < patternSize; col_2++)
                    {
                        valueGrid[row + row_2][col + col_2] = pattern.GetGridValue(col_2, row_2);
                    }
                }
            }
            else if (row == patternOutputHeight - 1)
            {
                for (int row_2 = 0; row_2 < patternSize; row_2++)
                {
                    valueGrid[row + row_2][col] = pattern.GetGridValue(0, row_2);
                }
            }
            else if (col == patternOutputWidth - 1)
            {
                for (int col_2 = 0; col_2 < patternSize; col_2++)
                {
                    valueGrid[row][col + col_2] = pattern.GetGridValue(col_2, 0);
                }
            }
            else
            {
                valueGrid[row][col] = pattern.GetGridValue(0, 0);
            }
        }
    }
}