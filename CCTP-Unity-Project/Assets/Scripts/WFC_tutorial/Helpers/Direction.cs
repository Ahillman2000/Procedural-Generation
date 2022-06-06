using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum TileValue
    {
        Road,
        Sidewalk,
        SidewalkEdge,
        Walkway
    }

    public static class DirectionHelper
    {
        public static Direction GetOppositeDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    return direction;
            }
        }
    }
}
