using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WaveFunctionCollapse
{
    public class CoreHelper
    {
        private float totalFrequency = 0;
        private float totalLogFrequency = 0;

        PatternManager patternManager;


        public CoreHelper(PatternManager _patternManager)
        {
            patternManager = _patternManager;

            //for (int i = 0; i < patternManager.GetNumberOfPatterns(); i++)
            //{
            //    totalFrequency += patternManager.GetPatternFrequency(i);
            //}
            //totalLogFrequency = Mathf.Log(totalFrequency, 2);
        }

        public int SelectSolutionPatternFromFrequency(List<int> possibleValues)
        {
            List<float> valueFrequenciesFractions = GetListOfWeightsFromIndices(possibleValues);
            float randomValue = UnityEngine.Random.Range(0, valueFrequenciesFractions.Sum());
            
            float sum = 0;
            int index = 0;

            foreach (var item in valueFrequenciesFractions)
            {
                sum += item;
                if(randomValue <= sum)
                {
                    return index;
                }
                index++;
            }
            return index;
        }

        private List<float> GetListOfWeightsFromIndices(List<int> possibleValues)
        {
            var valueFrequencies = possibleValues.Aggregate(new List<float>(), (accumulator, value) =>
            {
                accumulator.Add(patternManager.GetPatternFrequency(value));
                return accumulator;
            },
            acc => acc).ToList();
            return valueFrequencies;
        }

        public List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinates, Vector2Int previousCell)
        {
            List<VectorPair> list = new List<VectorPair>()
            {
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, 1) , previousCell, Direction.Up),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, -1), previousCell, Direction.Down),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(-1, 0), previousCell, Direction.Left),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(1, 0) , previousCell, Direction.Right),
            };
            return list;
        }

        public List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinate)
        {
            return Create4DirectionNeighbours(cellCoordinate, cellCoordinate);
        }

        public float CalculateEntropy(Vector2Int position,OutputGrid outputGrid)
        {
            float sum = 0;
            foreach (var possibleIndex in outputGrid.GetPossibleValueForPosition(position))
            {
                totalFrequency += patternManager.GetPatternFrequency(possibleIndex);
                sum += patternManager.GetPatternLog2Frequency(possibleIndex);
            }
            totalLogFrequency = Mathf.Log(totalFrequency, 2);
            return totalFrequency - (sum / totalFrequency);
        }

        public List<VectorPair> AreNeighboursCollapsed (VectorPair pairToCheck, OutputGrid outputGrid)
        {
            return Create4DirectionNeighbours(pairToCheck.CellToPropergatePosition, pairToCheck.BaseCellPosition)
                .Where(x => outputGrid.IsPositionValid(x.CellToPropergatePosition) & 
                !outputGrid.IsCellCollapsed(x.CellToPropergatePosition)).ToList();
        }

        public bool CheckCellSolutionForCollision(Vector2Int cellCoordinates, OutputGrid outputGrid)
        {
            foreach (var neighbour in Create4DirectionNeighbours(cellCoordinates))
            {
                if (!outputGrid.IsPositionValid(neighbour.CellToPropergatePosition)) continue;

                HashSet<int> possibleIndices = new HashSet<int>();
                foreach (var patternIndexAtNeighbour in outputGrid.GetPossibleValueForPosition(neighbour.CellToPropergatePosition))
                {
                    var possibleNeighboursForBase = patternManager.GetPossibleNeighboursForPatternInDirection(patternIndexAtNeighbour, 
                        neighbour.DirectionFromBase.GetOppositeDirection());
                    possibleIndices.UnionWith(possibleNeighboursForBase);
                }
                if(!possibleIndices.Contains(outputGrid.GetPossibleValueForPosition(cellCoordinates).First()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}