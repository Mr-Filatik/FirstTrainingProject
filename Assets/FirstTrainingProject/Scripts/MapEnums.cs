using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapController;
using static UnityEngine.Rendering.DebugUI;

namespace FirstTrainingProject
{
    public class MapEnums
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Example (1 - cell, 0 - wall, _ - not used): <br/>
    /// _ 0 _ 0 _ 0 _ 0 _ 0 _ <br/>
    /// 0 1 0 1 0 1 0 1 0 1 0 <br/>
    /// _ 0 _ 0 _ 0 _ 0 _ 0 _ <br/>
    /// 0 1 0 1 0 1 0 1 0 1 0 <br/>
    /// _ 0 _ 0 _ 0 _ 0 _ 0 _ <br/>
    /// 0 1 0 1 0 1 0 1 0 1 0 <br/>
    /// _ 0 _ 0 _ 0 _ 0 _ 0 _ <br/>
    /// 0 1 0 1 0 1 0 1 0 1 0 <br/>
    /// _ 0 _ 0 _ 0 _ 0 _ 0 _ <br/>
    /// 0 1 0 1 0 1 0 1 0 1 0 <br/>
    /// _ 0 _ 0 _ 0 _ 0 _ 0 _ <br/>
    /// </remarks>
    public class MapType
    {
        public int[,] Map { get; set; }
        public int Height => Map.GetLength(0);
        public int Width => Map.GetLength(1);
        public CellPointWithDirection StartPoint { get; set; }
        public CellPointWithDirection EndPoint { get; set; }
        public List<RoomPoint> InternalRooms { get; set; }

        public static int MinHeight => 11;
        public static int MinWidth => 11;

        public MapType(int hIndex, int wIndex)
        {
            Map = new int[hIndex, wIndex];

            for (int h = 0; h < Height; h++)
            {
                for (int w = 0; w < Width; w++)
                {
                    Map[h, w] = (int)CellType.Empty;
                }
            }
            StartPoint = new CellPointWithDirection() { PosH = 1, PosW = 1, Direction = CellDirection.Right };
            Map[StartPoint.PosH, StartPoint.PosW] = (int)CellType.Start;
            EndPoint = new CellPointWithDirection() { PosH = Height - 2, PosW = Width - 2, Direction = CellDirection.Left };
            Map[EndPoint.PosH, EndPoint.PosW] = (int)CellType.End;
            InternalRooms = new List<RoomPoint>();
        }

        public int this[int hIndex, int wIndex]
        {
            get => Map[hIndex, wIndex];
            set => Map[hIndex, wIndex] = value;
        }

        //add clear point condition
        public bool CellInMap(CellPointWithDirection cell)
        {
            if (cell.Direction == MapController.CellDirection.Left)
            {
                return CellInMap((CellPoint)cell) && CellInMap(cell.PosH, cell.PosW - 2);
            }
            if (cell.Direction == MapController.CellDirection.Top)
            {
                return CellInMap((CellPoint)cell) && CellInMap(cell.PosH + 2, cell.PosW);
            }
            if (cell.Direction == MapController.CellDirection.Right)
            {
                return CellInMap((CellPoint)cell) && CellInMap(cell.PosH, cell.PosW + 2);
            }
            if (cell.Direction == MapController.CellDirection.Bottom)
            {
                return CellInMap((CellPoint)cell) && CellInMap(cell.PosH - 2, cell.PosW);
            }
            return false;
        }

        //add clear point condition
        public bool CellInMap(CellPoint cell)
        {
            return CellInMap(cell.PosH, cell.PosW);
        }

        //add clear point condition
        public bool CellInMap(int posH, int posW)
        {
            return posH % 2 == 1 && posW % 2 == 1 && posH > 0 && posH < Height && posW > 0 && posW < Width;
        }
    }

    public class CellPoint
    {
        public int PosH;
        public int PosW;
    }

    public class CellPointWithDirection : CellPoint
    {
        public MapController.CellDirection Direction;
    }

    public class RoomPoint : CellPoint
    {
        private int _sizeH;
        private int _sizeW;
        /// <summary>
        /// 1, 2, 3... cells
        /// </summary>
        public int SizeH { get { return (_sizeH - 1) * 2; } set { _sizeH = value; } }
        /// <summary>
        /// 1, 2, 3... cells
        /// </summary>
        public int SizeW { get { return (_sizeW - 1) * 2; } set { _sizeW = value; } }

        public MapController.CellDirection DoorDirection { get; set; }
        /// <summary>
        /// Clockwise distance
        /// </summary>
        public int DoorDistance { get; set; }
    }

    [Obsolete]
    public enum DirectionForInstance // Rotation amount
    {
        Left = 0, // Zero PI
        Up = 90, // One fourth PI
        Right = 180, // Half PI
        Down = 270 // Three quarters PI
    }

    [Obsolete]
    public enum DirectionForRemovePillars // Удаление столбов на карте
    {
        LeftUp,
        UpRight,
        RightDown,
        DownLeft
    }

    [Obsolete]
    public enum PointForCreateMaze
    {
        /// <summary>
        /// Нет информации
        /// </summary>
        None = 0,
        /// <summary>
        /// Клетка свободна
        /// </summary>
        Free = 1,
        /// <summary>
        /// Клетка уже занята подо что-то
        /// </summary>
        Bysy = 2
    }

    /// <summary>
    /// Direction for a dead-end cell
    /// </summary>
    /// <remarks> Направление для тупиковой ячейки </remarks>
    [Obsolete]
    public enum BlindAlleyDirection
    {
        /// <summary>
        /// Left is empty
        /// </summary>
        Left = 11,
        /// <summary>
        /// Up is empty
        /// </summary>
        Up = 12,
        /// <summary>
        /// Right is empty
        /// </summary>
        Right = 13,
        /// <summary>
        /// Down is empty
        /// </summary>
        Down = 14
    }

    /// <summary>
    /// Direction for straight cell
    /// </summary>
    /// <remarks> Направление для прямой ячейки </remarks>
    [Obsolete]
    public enum StraightDirection
    {
        /// <summary>
        /// Left and right are empty
        /// </summary>
        LeftToRight = 21,
        /// <summary>
        /// Up and down are empty
        /// </summary>
        UpToDown = 22
    }

    /// <summary>
    /// Direction for corner cell
    /// </summary>
    /// <remarks> Направление для угловой ячейки </remarks>
    [Obsolete]
    public enum AngleDirection
    {
        /// <summary>
        /// Left and right are empty
        /// </summary>
        LeftToUp = 23,
        /// <summary>
        /// Left and right are empty
        /// </summary>
        UpToRight = 24,
        /// <summary>
        /// Left and right are empty
        /// </summary>
        RightToDown = 25,
        /// <summary>
        /// Left and right are empty
        /// </summary>
        DownToLeft = 26,
        /// <summary>
        /// Left and right are empty without pillar
        /// </summary>
        LeftToUpWithoutPillar = 231,
        /// <summary>
        /// Left and right are empty without pillar
        /// </summary>
        UpToRightWithoutPillar = 241,
        /// <summary>
        /// Left and right are empty without pillar
        /// </summary>
        RightToDownWithoutPillar = 251,
        /// <summary>
        /// Left and right are empty without pillar
        /// </summary>
        DownToLeftWithoutPillar = 261,
    }

    /// <summary>
    /// Direction for branch cell
    /// </summary>
    /// <remarks> Направление для ветвистой ячейки </remarks>
    [Obsolete]
    public enum BranchDirection
    {
        LeftBetweenUpAndDown = 31,
        UpBetweenLeftAndRight = 32,
        RightBetweenDownAndUp = 33,
        DownBetweenRightAndLeft = 34,
        LeftBetweenUpAndDownWithoutUpPillar = 311,
        LeftBetweenUpAndDownWithoutDownPillar = 312,
        LeftBetweenUpAndDownWithoutBothPillars = 313,
        UpBetweenLeftAndRightWithoutLeftPillar = 321,
        UpBetweenLeftAndRightWithoutRightPillar = 322,
        UpBetweenLeftAndRightWithoutBothPillars = 323,
        RightBetweenDownAndUpWithoutDownPillar = 331,
        RightBetweenDownAndUpWithoutUpPillar = 332,
        RightBetweenDownAndUpWithoutBothPillars = 333,
        DownBetweenRightAndLeftWithoutRightPillar = 341,
        DownBetweenRightAndLeftWithoutLeftPillar = 342,
        DownBetweenRightAndLeftWithoutBothPillars = 343,
    }

    /// <summary>
    /// Direction for crossing cell
    /// </summary>
    /// <remarks> Направление для ячейки пересечения </remarks>
    [Obsolete]
    public enum CrossingDirection
    {
        AllSides = 41,

        AllSidesWithoutOnePillar = 411,

        AllSidesWithoutOnePillarLeftUp = 4111,
        AllSidesWithoutOnePillarUpRight = 4112,
        AllSidesWithoutOnePillarRightDown = 4113,
        AllSidesWithoutOnePillarDownLeft = 4114,

        AllSidesWithoutTwoPillars = 412,

        AllSidesWithoutTwoPillarsNeighbours = 4121,

        AllSidesWithoutTwoPillarsNeighboursLeft = 41211,
        AllSidesWithoutTwoPillarsNeighboursUp = 41212,
        AllSidesWithoutTwoPillarsNeighboursRight = 41213,
        AllSidesWithoutTwoPillarsNeighboursDown = 41214,

        AllSidesWithoutTwoPillarsNotNeighbours = 4122,

        AllSidesWithoutTwoPillarsNotNeighboursLeftUp = 41221,
        AllSidesWithoutTwoPillarsNotNeighboursUpRight = 41222,
        AllSidesWithoutTwoPillarsNotNeighboursRightDown = 41223,
        AllSidesWithoutTwoPillarsNotNeighboursDownLeft = 41224,

        AllSidesWithoutThreePillars = 413,

        AllSidesWithoutThreePillarsLeft = 4131,
        AllSidesWithoutThreePillarsUp = 4132,
        AllSidesWithoutThreePillarsRight = 4133,
        AllSidesWithoutThreePillarsDown = 4134,

        AllSidesWithoutFourPillars = 414
    }
}