using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using System;

/// <summary>
/// Socket related info - direction of the socket, the stored tile, the neighbours that can match this socket
/// </summary>
[Serializable]
public struct Socket
{
    public Direction direction;
    public TileValue value;
    //public List<TileValue> validNeighbours;
}
