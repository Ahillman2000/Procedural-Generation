using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using System;

[Serializable]
public struct Socket
{
    public Direction direction;
    public TileValue value;
    public List<TileValue> validNeighbours;
}
