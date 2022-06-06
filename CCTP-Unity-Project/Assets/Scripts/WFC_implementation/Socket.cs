using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using System;

[Serializable]
public struct Socket
{
    public Direction direction;
    public int Value;
    public List<int> validNeighbours;
}
