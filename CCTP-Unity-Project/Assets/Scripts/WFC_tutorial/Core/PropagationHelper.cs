using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WaveFunctionCollapse
{
    public class PropagationHelper
    {
        OutputGrid outputGrid;
        CoreHelper coreHelper;

        bool cellWithNoSolutionPresent;
        SortedSet<LowEntropyCell> lowEntropySet = new SortedSet<LowEntropyCell>();

        Queue<VectorPair> pairsToPropagate = new Queue<VectorPair>();

        public SortedSet<LowEntropyCell> LowEntropySet { get => lowEntropySet;}
        public Queue<VectorPair> PairsToPropagate { get => pairsToPropagate;}

        public PropagationHelper(OutputGrid _outputGrid, CoreHelper _coreHelper)
        {
            outputGrid = _outputGrid;
            coreHelper = _coreHelper;
        }

        public bool ShouldPairBeProcessed(VectorPair propagationPair)
        {
            return outputGrid.IsPositionValid(propagationPair.CellToPropergatePosition) && !propagationPair.CheckingPreviousCell();
        }

        public void AnalysePropagationResults(VectorPair propagationPair, int startCount, int newPossiblePatternCount)
        {
            if(newPossiblePatternCount > 1 && startCount > newPossiblePatternCount)
            {
                AddNewPairsToPropagateQueue(propagationPair.CellToPropergatePosition, propagationPair.BaseCellPosition);
                AddToLowEntropySet(propagationPair.CellToPropergatePosition);
            }
            if(newPossiblePatternCount == 0)
            {
                cellWithNoSolutionPresent = true;
            }
            if(newPossiblePatternCount == 1)
            {
                cellWithNoSolutionPresent = coreHelper.CheckCellSolutionForCollision(propagationPair.CellToPropergatePosition, outputGrid);
            }
        }

        public void EnqueueUncollapsedNeighbours(VectorPair propagationPair)
        {
            var uncollapsedNeighbours = coreHelper.AreNeighboursCollapsed(propagationPair, outputGrid);
            foreach (var neighbour in uncollapsedNeighbours)
            {
                pairsToPropagate.Enqueue(neighbour);
            }
        }

        public void AddNewPairsToPropagateQueue(Vector2Int cellToPropergatePosition, Vector2Int baseCellPosition)
        {
            var list = coreHelper.Create4DirectionNeighbours(cellToPropergatePosition, baseCellPosition);
            foreach (var item in list)
            {
                pairsToPropagate.Enqueue(item);
            }
        }

        private void AddToLowEntropySet(Vector2Int cellToPropergatePosition)
        {
            var elementOflowEntropySet = lowEntropySet.Where(x => x.Position == cellToPropergatePosition).FirstOrDefault();
            if(elementOflowEntropySet == null && !outputGrid.IsCellCollapsed(cellToPropergatePosition))
            {
                float entropy = coreHelper.CalculateEntropy(cellToPropergatePosition, outputGrid);
                lowEntropySet.Add(new LowEntropyCell(cellToPropergatePosition, entropy));
            }
            else
            {
                lowEntropySet.Remove(elementOflowEntropySet);
                elementOflowEntropySet.Entropy = coreHelper.CalculateEntropy(cellToPropergatePosition, outputGrid);
                lowEntropySet.Add(elementOflowEntropySet);
            }
        }

        public bool CheckForConflicts()
        {
            return cellWithNoSolutionPresent;
        }

        public void SetConflictFlag()
        {
            cellWithNoSolutionPresent = true;
        }
    }
}