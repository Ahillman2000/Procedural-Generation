using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WaveFunctionCollapse;

namespace Helpers
{
    public static class HelperFunctions
    {
        public static T CreateJaggedArray<T>(params int[] lengths)
        {
            return (T)IntialiseJaggedArray(typeof(T).GetElementType(), 0, lengths);
        }

        static object IntialiseJaggedArray(Type type, int index, int[] lengths)
        {
            Array array = Array.CreateInstance(type, lengths[index]);
            Type elementType = type.GetElementType();

            if(elementType != null)
            {
                for (int i = 0; i < lengths[index]; i++)
                {
                    array.SetValue(IntialiseJaggedArray(elementType, index + 1, lengths), i);
                }
            }

            return array;
        }

        public static bool CheckJaggedArray2IfIndexIsValid<T>(this T[][] array, int x, int y)
        {
            if(array == null)
            {
                return false;
            }
            return Validate2DCoords(x, y, array[0].Length, array.Length);
        }

        public static bool Validate2DCoords(int x, int y, int width, int height)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            return true;
        }

        public static bool Validate1DCoords(int i, int height, int width)
        {
            throw new NotImplementedException();

            //up
            //i + 1 <= (x + 1) * height - 1;
            //down
            //i - 1 >= x * height; 
            //left
            //i - height >= 0;
            //right
            //i + height < lengthOFGrid;
        }

        public static bool CheckForValidNeighbourInDirection(int i, int gridHeight, int gridWidth, Direction direction)
        {
            int x = HelperFunctions.ConvertTo2dArray(i, gridWidth).x;
            int gridSize = gridHeight * gridWidth;

            switch (direction)
            {
                case Direction.Up:
                    return i + 1 <= (x + 1) * gridHeight - 1;
                case Direction.Down:
                    return i - 1 >= x * gridHeight;
                case Direction.Left:
                    return i - gridHeight >= 0;
                case Direction.Right:
                    return i + gridHeight < gridSize;
                default:
                    return false;
            }
        }

        public static int ConvertTo1dArray(int x , int y, int gridWidth)
        {
            return (x * gridWidth) + y;
        }

        public static Vector2Int ConvertTo2dArray(int i, int gridWidth)
        {
            Vector2Int returnVector = new Vector2Int(i / gridWidth, i % gridWidth);

            return returnVector;
        }
    }
}


