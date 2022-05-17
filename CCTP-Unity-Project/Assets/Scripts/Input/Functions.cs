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
    }
}


