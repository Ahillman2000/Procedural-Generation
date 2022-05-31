using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WaveFunctionCollapse
{
    public class CoreSolver
    {
        PatternManager patterManager;
        OutputGrid outputGrid;
        CoreHelper coreHelper;
        PropagationHelper propagationHelper;

        public CoreSolver(OutputGrid _outputGrid, PatternManager _patternManager)
        {
            outputGrid = _outputGrid;
            patterManager = _patternManager;
            coreHelper = new CoreHelper(patterManager);
            propagationHelper = new PropagationHelper(outputGrid, coreHelper);
        }

        public void Propagate()
        {
            while(propagationHelper.PairsToPropagate.Count > 0)
            {
                var propagationPair = propagationHelper.PairsToPropagate.Dequeue();
                if(propagationHelper.ShouldPairBeProcessed(propagationPair))
                {
                    ProcessCell(propagationPair);
                }
                if(propagationHelper.CheckForConflicts() || outputGrid.IsGridSolved())
                {
                    return;
                }
            }
            if (propagationHelper.CheckForConflicts() && propagationHelper.PairsToPropagate.Count == 0 
                && propagationHelper.LowEntropySet.Count == 0)
            {
                propagationHelper.SetConflictFlag();
            }
        }

        private void ProcessCell(VectorPair propagationPair)
        {
            if(outputGrid.IsCellCollapsed(propagationPair.CellToPropergatePosition))
            {
                propagationHelper.EnqueueUncollapsedNeighbours(propagationPair);
            }
            else
            {
                PropagateNeighbour(propagationPair);
            }
        }

        private void PropagateNeighbour(VectorPair propagationPair)
        {
            var possibleValuesAtNeighbour = outputGrid.GetPossibleValueForPosition(propagationPair.CellToPropergatePosition);
            int startCount = possibleValuesAtNeighbour.Count;

            RemoveImpossibleNeighbours(propagationPair, possibleValuesAtNeighbour);

            int newPossiblePatternCount = possibleValuesAtNeighbour.Count;
            propagationHelper.AnalysePropagationResults(propagationPair, startCount, newPossiblePatternCount);
        }

        private void RemoveImpossibleNeighbours(VectorPair propagationPair, HashSet<int> possibleValuesAtNeighbour)
        {
            HashSet<int> possibleIndices = new HashSet<int>();

            foreach (var patternIndexAtBase in outputGrid.GetPossibleValueForPosition(propagationPair.BaseCellPosition))
            {
                var possibleNeighboursForBase = patterManager.GetPossibleNeighboursForPatternInDirection(patternIndexAtBase, propagationPair.DirectionFromBase);
                possibleIndices.UnionWith(possibleNeighboursForBase);
            }

            possibleValuesAtNeighbour.IntersectWith(possibleIndices);
        }

        public Vector2Int GetLowestEntropyCell()
        {
            if(propagationHelper.LowEntropySet.Count <= 0)
            {
                return outputGrid.GetRandomCell();
            }
            else
            {
                var lowestEntropyElement = propagationHelper.LowEntropySet.First();
                Vector2Int returnVector = lowestEntropyElement.Position;
                propagationHelper.LowEntropySet.Remove(lowestEntropyElement);
                return returnVector;
            }
        }

        public void CollapseCell(Vector2Int cellCoordinates)
        {
            var possibleValue = outputGrid.GetPossibleValueForPosition(cellCoordinates).ToList();

            if (possibleValue.Count == 0 || possibleValue.Count == 1) return;
            else
            {
                int index = coreHelper.SelectSolutionPatternFromFrequency(possibleValue);
                outputGrid.SetPatternAtPosition(cellCoordinates.x, cellCoordinates.y, possibleValue[index]);
            }

            if (!coreHelper.CheckCellSolutionForCollision(cellCoordinates, outputGrid))
            {
                propagationHelper.AddNewPairsToPropagateQueue(cellCoordinates, cellCoordinates);
            }
            else
            {
                propagationHelper.SetConflictFlag();
            }
        }

        public bool CheckIfSolved()
        {
            return outputGrid.IsGridSolved();
        }

        public bool CheckForConflicts()
        {
            return propagationHelper.CheckForConflicts();
        }
    }
}

