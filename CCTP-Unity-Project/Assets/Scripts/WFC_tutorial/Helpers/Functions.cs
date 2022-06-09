using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            return ValidateCoords(x, y, array[0].Length, array.Length);
        }

        public static bool ValidateCoords(int x, int y, int width, int height)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            return true;
        }

        public static int ConvertTo1dArray(int x , int y, int gridWidth)
        {
            return (x * gridWidth) + y;
        }

        public static Vector2 ConvertTo2dArray(int i, int gridWidth)
        {
            Vector2 returnVector = new Vector2(i / gridWidth, i % gridWidth);

            return returnVector;
        }
    }
}


