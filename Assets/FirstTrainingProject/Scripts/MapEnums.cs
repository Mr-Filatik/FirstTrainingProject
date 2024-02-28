using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstTrainingProject
{
    public class MapEnums
    {

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
        public int SizeH;
        public int SizeW;
    }

    public enum DirectionForInstance // Rotation amount
    {
        Left = 0, // Zero PI
        Up = 90, // One fourth PI
        Right = 180, // Half PI
        Down = 270 // Three quarters PI
    }

    public enum DirectionForRemovePillars // Удаление столбов на карте
    {
        LeftUp,
        UpRight,
        RightDown,
        DownLeft
    }

    //
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