using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnums
{

}

public enum DirectionForInstance // Rotation amount
{
    Left = 0, // Zero PI
    Up = 90, // One fourth PI
    Right = 180, // Half PI
    Down = 270 // Three quarters PI
}

/// <summary>
/// Direction for a dead-end cell
/// </summary>
/// <remarks> ����������� ��� ��������� ������ </remarks>
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
/// <remarks> ����������� ��� ������ ������ </remarks>
public enum StraightDirection
{
    LeftToRight = 21,
    UpToDown = 22
}

/// <summary>
/// Direction for corner cell
/// </summary>
/// <remarks> ����������� ��� ������� ������ </remarks>
public enum AngleDirection
{
    LeftToUp = 23,
    UpToRight = 24,
    RightToDown = 25,
    DownToLeft = 26
}

/// <summary>
/// Direction for branch cell
/// </summary>
/// <remarks> ����������� ��� ��������� ������ </remarks>
public enum BranchDirection
{
    LeftBetweenUpAndDown = 31,
    UpBetweenLeftAndRight = 32,
    RightBetweenDownAndUp = 33,
    DownBetweenRightAndLeft = 34
}

/// <summary>
/// Direction for crossing cell
/// </summary>
/// <remarks> ����������� ��� ������ ����������� </remarks>
public enum CrossingDirection
{
    AllSides = 41
}