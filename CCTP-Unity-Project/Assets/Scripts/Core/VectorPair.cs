using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class VectorPair
    {
        public Vector2Int BaseCellPosition { get; set; }
        public Vector2Int CellToPropergatePosition { get; set; }

        public Vector2Int PreviousCellPosition { get; set; }

        public Direction DirectionFromBase { get; set; }

        public VectorPair(Vector2Int baseCellPositon, Vector2Int cellToPropergatePostion, Vector2Int previousCellPositon, 
            Direction directionFromBase)
        {
            BaseCellPosition = baseCellPositon;
            CellToPropergatePosition = cellToPropergatePostion;
            PreviousCellPosition = previousCellPositon;
            DirectionFromBase = directionFromBase;
        }

        public bool CheckingPreviousCell()
        {
            return PreviousCellPosition == CellToPropergatePosition;
        }
    }
}