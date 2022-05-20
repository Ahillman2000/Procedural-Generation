using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helpers
{
    public static class MyCollectionExtensions
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
    }
}


