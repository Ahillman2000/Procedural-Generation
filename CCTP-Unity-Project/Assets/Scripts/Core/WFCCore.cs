using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class WFCCore
    {
        OutputGrid outputgrid;
        PatternManager patternManager;

        private int maxIterations = 0;

        public WFCCore(int outputWidth, int outputHeight, int _maxIterations, PatternManager _patternManager)
        {
            outputgrid = new OutputGrid(outputWidth, outputHeight, _patternManager.GetNumberOfPatterns());
            patternManager = _patternManager;
            maxIterations = _maxIterations;
        }

        public int[][] CreateOutputGrid()
        {
            int i = -1;
            while(i <= maxIterations)
            {
                CoreSolver solver = new CoreSolver(outputgrid, patternManager);

                int j = 100;
                while(!solver.CheckForConflicts() && !solver.CheckIfSolved())
                {
                    Vector2Int position = solver.GetLowestEntropyCell();
                    solver.CollapseCell(position);
                    solver.Propagate();
                    j--;

                    if(j <= 0)
                    {
                        Debug.Log("Propagation takes too long");
                        return new int[0][];
                    }
                }
                if(solver.CheckForConflicts())
                {
                    Debug.Log("\n A conflict has occured within iteration; " + i);
                    i++;
                    outputgrid.ResetAllPossibilities();
                    solver = new CoreSolver(outputgrid, patternManager);
                }
                else
                {
                    Debug.Log("solved on iteration: " + i);
                    outputgrid.PrintResultsToConsole();
                    break;
                }
            }
            if(i >= maxIterations)
            {
                Debug.Log("Unable to Solve tilemap");
            }
            return outputgrid.GetSolvedOutputGrid();
        }
    }
}

