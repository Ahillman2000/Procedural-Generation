using WaveFunctionCollapse;
using System;
using UnityEngine;

/// <summary>
/// Socket related info - direction of the socket, the stored tile, the neighbours that can match this socket
/// </summary>
[Serializable]
public class Socket
{
    /*[UnityEngine.HideInInspector]*/ public Direction direction;
    public TileValue value;

    public Socket(Direction direction, TileValue value)
    {
        this.direction = direction;
        this.value = value;
    }
}