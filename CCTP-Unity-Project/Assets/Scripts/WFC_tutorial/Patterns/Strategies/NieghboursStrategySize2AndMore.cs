using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class NieghboursStrategySize2AndMore : IFindNeighboursStrategy
    {
        public Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patterFinderResult)
        {
            Dictionary<int, PatternNeighbours> result = new Dictionary<int, PatternNeighbours>();

            foreach (var patternDataToCheck in patterFinderResult.PatternIndexDictionary)
            {
                foreach (var possibleNeighboursForPattern in patterFinderResult.PatternIndexDictionary)
                {
                    FindNeighboursInAllDirection(result, patternDataToCheck, possibleNeighboursForPattern);
                }
            }
            return result;
        }

        private void FindNeighboursInAllDirection(Dictionary<int, PatternNeighbours> result, KeyValuePair<int, PatternData> patternDataToCheck, KeyValuePair<int, PatternData> possibleNeighboursForPattern)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if(patternDataToCheck.Value.CompareGrid(direction, possibleNeighboursForPattern.Value))
                {
                    if(result.ContainsKey(patternDataToCheck.Key) == false)
                    {
                        result.Add(patternDataToCheck.Key, new PatternNeighbours());
                    }
                    result[patternDataToCheck.Key].AddPatternToDictionary(direction, possibleNeighboursForPattern.Key);
                }
            }
        }
    }
}

