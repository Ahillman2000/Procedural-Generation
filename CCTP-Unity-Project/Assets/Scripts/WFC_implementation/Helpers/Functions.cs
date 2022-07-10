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
        /// <summary>
        /// Check if a neighbour is valid in a given direction
        /// </summary>
        /// <param name="i"> The index of the cell being checked from  </param>
        /// <param name="gridHeight"> The height of the given grid to act as a offset</param>
        /// <param name="gridWidth"> The width of the given grid to determine the x index </param>
        /// <param name="direction"> The direction in which to check the neighbouring cell </param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts a 2 dimensional array to 1 dimensional space
        /// </summary>
        /// <param name="x"> The x coordinate in 2 dimensional space </param>
        /// <param name="y"> The y coordinate in 2 dimensional space </param>
        /// <param name="gridWidth"> The width of the given grid </param>
        /// <returns></returns>
        public static int ConvertTo1dArray(int x , int y, int gridWidth)
        {
            return (x * gridWidth) + y;
        }

        /// <summary>
        /// Converts a 1 dimensional array into 2 dimension space
        /// </summary>
        /// <param name="i"> The 1d index position </param>
        /// <param name="gridWidth"> The width of the given grid </param>
        /// <returns></returns>
        public static Vector2Int ConvertTo2dArray(int i, int gridWidth)
        {
            Vector2Int returnVector = new Vector2Int(i / gridWidth, i % gridWidth);

            return returnVector;
        }


        /// <summary>
        /// Determines if given number is perfect square
        /// </summary>
        /// <param name="n"> The number to check against </param>
        /// https://www.geeksforgeeks.org/check-if-given-number-is-perfect-square-in-cpp/
        /// <returns></returns>
        private static bool IsPerfectSquare(int n)
        {

            // Find floating point value of
            // square root of x.
            if (n >= 0)
            {
                int root = (int)Math.Sqrt(n);

                // if product of square root
                // is equal, then
                // return T/F
                return (root * root == n);
            }
            // else return false if n<0
            return false;
        }

        /// <summary>
        /// Gets the next largest perfect square of a given number
        /// </summary>
        /// <param name="n"> The number to check against </param>
        /// https://www.geeksforgeeks.org/find-the-next-perfect-square-greater-than-a-given-number/
        /// <returns></returns>
        private static int GetNextPerfectSquare(int n)
        {
            double root = Math.Sqrt(n);
            int nextN = (int)Math.Floor(root) + 1;
            int nextPerfectSquare = (int)Math.Pow(nextN, 2);
            return nextPerfectSquare;
        }

        /// <summary>
        /// Gets the perfect square of a given number
        /// </summary>
        /// <param name="n"> The number to cehck against </param>
        /// <returns></returns>
        public static int GetPerfectSquare(int n)
        {
            if (IsPerfectSquare(n))
            {
                return n;
            }
            else
            {
                return GetNextPerfectSquare(n);
            }
        }
    }
}


