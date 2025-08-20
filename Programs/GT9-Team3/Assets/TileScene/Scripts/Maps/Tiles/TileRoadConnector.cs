using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRoadConnector : MonoBehaviour
{
    private TileData _tileData;

    private void Awake()
    {
        _tileData = GetComponent<TileData>();
    }

    // 이웃한 타일과의 연결 여부 확인
    public bool isConnectedTo(TileData neighbor, TileDir tileDir)
    {
        return tileDir switch
        {
            TileDir.Up => _tileData.connectedUp && neighbor != null && neighbor.connectedDown,
            TileDir.Down => _tileData.connectedDown && neighbor != null && neighbor.connectedUp,
            TileDir.Left => _tileData.connectedLeft && neighbor != null && neighbor.connectedRight,
            TileDir.Right => _tileData.connectedRight && neighbor != null && neighbor.connectedLeft,
            _ => false
        };
    }

    
    public List<(TileData, TileDir)> GetConnectedNeighbors()
    {
        var neighbors = new List<(TileData, TileDir)>();

        if (isConnectedTo(_tileData.up, TileDir.Up)) neighbors.Add((_tileData.up, TileDir.Up));
        if (isConnectedTo(_tileData.down, TileDir.Down)) neighbors.Add((_tileData.down, TileDir.Down));
        if (isConnectedTo(_tileData.left, TileDir.Left)) neighbors.Add((_tileData.left, TileDir.Left));
        if (isConnectedTo(_tileData.right, TileDir.Right)) neighbors.Add((_tileData.right, TileDir.Right));
        
        return neighbors;
    }
}
