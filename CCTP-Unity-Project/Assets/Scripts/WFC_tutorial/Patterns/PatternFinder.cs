using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using Helpers;

namespace WaveFunctionCollapse
{
    public static class PatternFinder
    {
        internal static PatternDataResults GetPatternDataFromGrid<T>(ValuesManager<T> valuemanager, int patternSize, bool equalWeights)
        {
            Dictionary<string, PatternData> patternHashCodeDictionary = new Dictionary<string, PatternData>();
            Dictionary<int, PatternData>    patternIndexDictionary = new Dictionary<int, PatternData>();

            Vector2 sizeOfGrid = valuemanager.getGridSize();

            int patternGridSizeX = 0;
            int patternGridSizeY = 0;
            int rowMin = -1, colMin = -1, rowMax = -1, colMax = -1;

            if(patternSize < 3)
            {
                patternGridSizeX = (int)sizeOfGrid.x + 3 - patternSize;
                patternGridSizeY = (int)sizeOfGrid.y + 3 - patternSize;

                rowMax = patternGridSizeY - 1;
                colMax = patternGridSizeX - 1;
            }
            else
            {
                patternGridSizeX = (int)sizeOfGrid.x + patternSize - 1;
                patternGridSizeY = (int)sizeOfGrid.y + patternSize - 1;

                rowMin = 1 - patternSize;
                colMin = 1 - patternSize;

                rowMax = (int)sizeOfGrid.y;
                colMax = (int)sizeOfGrid.x;
            }

            int[][] patternIndicesGrid = HelperFunctions.CreateJaggedArray<int[][]>(patternGridSizeY, patternGridSizeX);

            int totalFrequency = 0;
            int patternIndex   = 0;

            for (int row = rowMin; row < rowMax; row++)
            {
                for (int col = colMin; col < colMax; col++)
                {
                    int[][] gridValues = valuemanager.GetPatternValuesFromGridAt(col, row, patternSize);
                    string hashValue = HashCodeCalculator.CalculateHashCode(gridValues);

                    if(patternHashCodeDictionary.ContainsKey(hashValue) == false)
                    {
                        Pattern pattern = new Pattern(gridValues, hashValue, patternIndex);
                        patternIndex++;
                        AddNewPattern(patternHashCodeDictionary, patternIndexDictionary, hashValue, pattern);
                    }
                    else
                    {
                        if(equalWeights == false)
                        {
                            patternIndexDictionary[patternHashCodeDictionary[hashValue].Pattern.Index].AddToFrequency();
                        }
                    }

                    totalFrequency++;
                    if (patternSize < 3)
                    {
                        patternIndicesGrid[row + 1][col + 1] = patternHashCodeDictionary[hashValue].Pattern.Index;
                    }
                    else
                    {
                        patternIndicesGrid[row + patternSize - 1][col + patternSize - 1] = patternHashCodeDictionary[hashValue].Pattern.Index;
                    }
                }
            }

            CalculateReativeFrequency(patternIndexDictionary, totalFrequency);
            return new PatternDataResults(patternIndicesGrid, patternIndexDictionary);
        }

        private static void CalculateReativeFrequency(Dictionary<int, PatternData> patternIndexDictionary, int totalFrequency)
        {
            foreach (var item in patternIndexDictionary.Values)
            {
                item.CalculateRelativeFrequency(totalFrequency);
            }
        }

        private static void AddNewPattern(Dictionary<string, PatternData> patternHashCodeDictionary, Dictionary<int, PatternData> patternIndexDictionary, string hashValue, Pattern pattern)
        {
            PatternData data = new PatternData(pattern);
            patternHashCodeDictionary.Add(hashValue, data);
            patternIndexDictionary.Add(pattern.Index, data); 
        }

        internal static Dictionary<int, PatternNeighbours> FindPossibleNeighboursForAllPatterns(IFindNeighboursStrategy strategy, PatternDataResults patterFinderResult)
        {
            return strategy.FindNeighbours(patterFinderResult);
        }

        public static PatternNeighbours CheckNeighboursInEachDirection(int x, int y, PatternDataResults patternDataResults)
        {
            PatternNeighbours patternNeighbours = new PatternNeighbours();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                int possiblePatternIndex = patternDataResults.GetNeighbourInDirection(x, y, direction);
                if(possiblePatternIndex > 0)
                {
                    patternNeighbours.AddPatternToDictionary(direction, possiblePatternIndex);
                }
            }
            return patternNeighbours;
        }

        public static void AddNeighboursToDictionary(Dictionary<int, PatternNeighbours> dictionary, int patternIndex, PatternNeighbours neighbours)
        {
            if(dictionary.ContainsKey(patternIndex) == false)
            {
                dictionary.Add(patternIndex, neighbours);
            }
            dictionary[patternIndex].AddNeighbours(neighbours);
        }
    }

}
