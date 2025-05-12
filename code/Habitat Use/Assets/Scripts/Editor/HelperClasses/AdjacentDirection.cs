using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Numerical representation of the direction of adjacent cells
/// </summary>
public enum AdjacentDirection : int
{
    LEFT = 0,
    TOPLEFT = 1,
    TOPRIGHT = 2,
    RIGHT = 3,
    BOTTOMRIGHT = 4,
    BOTTOMLEFT = 5
}

/// <summary>
/// Returns the coordinates of the desired adjacent cell 
/// </summary>
public class AdjacentCellVectors
{
    public Vector2Int GetLeft(int x, int y)
    {
        return new Vector2Int(x - 1, y);
    }

    public Vector2Int GetTopLeft(int x, int y)
    {
        if (y % 2 == 0)
            return new Vector2Int(x, y + 1);
        else
            return new Vector2Int(x - 1, y + 1);
    }

    public Vector2Int GetTopRight(int x, int y)
    {
        if (y % 2 == 0)
            return new Vector2Int(x + 1, y + 1);
        else
            return new Vector2Int(x, y + 1);
    }

    public Vector2Int GetRight(int x, int y)
    {
        return new Vector2Int(x + 1, y);
    }

    public Vector2Int GetBottomRight(int x, int y)
    {
        if (y % 2 == 0)
            return new Vector2Int(x + 1, y - 1);
        else
            return new Vector2Int(x, y - 1);
    }

    public Vector2Int GetBottomLeft(int x, int y)
    {
        if (y % 2 == 0)
            return new Vector2Int(x, y - 1);
        else
            return new Vector2Int(x - 1, y - 1);
    }
}
