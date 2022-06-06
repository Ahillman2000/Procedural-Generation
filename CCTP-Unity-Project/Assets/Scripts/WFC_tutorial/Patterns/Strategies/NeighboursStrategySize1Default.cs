using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class NeighboursStrategySize1Default : IFindNeighboursStrategy
    {
        public Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patterFinderResult)
        {
            Dictionary<int, PatternNeighbours> result = new Dictionary<int, PatternNeighbours>();

            FindNeighboursForEachPattern(patterFinderResult, result);
            return result;
        }

        private void FindNeighboursForEachPattern(PatternDataResults patterFinderResult, Dictionary<int, PatternNeighbours> result)
        {
            for (int row = 0; row < patterFinderResult.GetGridLengthY(); row++)
            {
                for (int col = 0; col < patterFinderResult.GetGridLengthX(); col++)
                {
                    PatternNeighbours neighbours = PatternFinder.CheckNeighboursInEachDirection(col, row, patterFinderResult);
                    PatternFinder.AddNeighboursToDictionary(result, patterFinderResult.GetIndexAt(col, row), neighbours);
                }
            }
        }
    }
}

