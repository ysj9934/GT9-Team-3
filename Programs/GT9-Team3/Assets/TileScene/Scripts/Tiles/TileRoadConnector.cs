using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRoadConnector : MonoBehaviour
{
    private TileRoad _tile;

    private void Awake()
    {
        _tile = GetComponent<TileRoad>();
    }

    // 이웃한 타일과의 연결 여부 확인
    public bool isConnectedTo(TileRoad neighbor, TileDir tileDir)
    {
        return tileDir switch
        {
            TileDir.Up => _tile.connectedUp && neighbor != null && neighbor.connectedDown,
            TileDir.Down => _tile.connectedDown && neighbor != null && neighbor.connectedUp,
            TileDir.Left => _tile.connectedLeft && neighbor != null && neighbor.connectedRight,
            TileDir.Right => _tile.connectedRight && neighbor != null && neighbor.connectedLeft,
            _ => false
        };
    }

    
    public List<(TileRoad, TileDir)> GetConnectedNeighbors()
    {
        var neighbors = new List<(TileRoad, TileDir)>();

        if (isConnectedTo(_tile.up, TileDir.Up)) neighbors.Add((_tile.up, TileDir.Up));
        if (isConnectedTo(_tile.down, TileDir.Down)) neighbors.Add((_tile.down, TileDir.Down));
        if (isConnectedTo(_tile.left, TileDir.Left)) neighbors.Add((_tile.left, TileDir.Left));
        if (isConnectedTo(_tile.right, TileDir.Right)) neighbors.Add((_tile.right, TileDir.Right));
        
        return neighbors;
    }
}
