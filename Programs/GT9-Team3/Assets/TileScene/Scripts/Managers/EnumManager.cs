using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileDirector
{
    Degree_0,
    Degree_90,
    Degree_180,
    Degree_270,
}

public enum TileCategory
{
    Normal,
    Castle,
    Spawner,
    Grid,
    None,
}

public enum TileShape
{
    Corner,
    Straight,
    TShape,
    Cross,
    None,
}

public enum TileDir
{
    Up,
    Down,
    Left,
    Right,
}

public enum BlockCategory
{
    Ground,
    Road,
    PlaceTower,
    None,
}