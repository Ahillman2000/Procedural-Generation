using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class NeighbourStrategyFactory
    {
        Dictionary<string, Type> strategies;

        public NeighbourStrategyFactory()
        {
            LoadTypesIFindNeighboursStrategy();
        }

        private void LoadTypesIFindNeighboursStrategy()
        {
            strategies = new Dictionary<string, Type>();
            Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in typesInThisAssembly)
            {
                if(type.GetInterface(typeof(IFindNeighboursStrategy).ToString()) != null)
                {
                    strategies.Add(type.Name.ToLower(), type);
                }
            }
        }

        internal IFindNeighboursStrategy CreateInstance(string nameOfStrategy)
        {
            Type t = GetTypeToCreate(nameOfStrategy);

            if(t == null)
            {
                t = GetTypeToCreate("more");
            }
            return Activator.CreateInstance(t) as IFindNeighboursStrategy;
        }

        private Type GetTypeToCreate(string nameOfStrategy)
        {
            foreach (var possibleStrategy in strategies)
            {
                if(possibleStrategy.Key.Contains(nameOfStrategy))
                {
                    return strategies[possibleStrategy.Key];
                    //return possibleStrategy.Value;
                }
            }
            return null;
        }
    }
}

