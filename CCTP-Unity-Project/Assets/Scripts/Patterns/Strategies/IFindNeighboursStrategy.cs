using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public interface IFindNeighboursStrategy
    {
        Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patterFinderResult);
    }
}

