using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// TileData
/// create by : yoons
/// create at : 2025.08.19
/// description :
/// 타일 공통 정보
/// </summary>
public class TileData : MonoBehaviour
{
    public TileManager _tileManager;
    public TileRoadConnector _tileRoadConnector;
    public TileUI _tileUI;
    public TileMove _tileMove;

    [SerializeField] public TileCategory tileCategory;
    [SerializeField] public TileShape tileShape;

    [Header("Tile Neighbors")]
    public TileData up;
    public TileData down;
    public TileData left;
    public TileData right;
    
    public bool connectedUp;
    public bool connectedDown;
    public bool connectedLeft;
    public bool connectedRight;
    
    public int tileIndex;
    public int originTileCol = -1;
    public int originTileRow = -1;
    public int tileCol;
    public int tileRow;

    

    protected virtual void Awake()
    {
        _tileManager = TileManager.Instance;
    }

    private void Start()
    {
        _tileRoadConnector = GetComponent<TileRoadConnector>();
        _tileUI = GetComponent<TileUI>();
        _tileMove = GetComponent<TileMove>();
    }

    public virtual void Initialize(Vector2 pos)
    {
        UpdateMapping(pos);
        tileIndex = UpdateTileIndex();
        _tileManager.SetNeighbors();
    }

    public virtual void UpdateMapping(Vector2 pos)
    {
        float originX = 0f;
        float originY = _tileManager.tileSize[1] * 2 + (_tileManager.tileSize[1] * 2 * _tileManager.tempLevel);

        float dx = pos.x - originX;
        float dy = originY - pos.y;

        float col = (dx / _tileManager.tileSize[0] + dy / _tileManager.tileSize[1]) / 2f;
        float row = (dy / _tileManager.tileSize[1] - dx / _tileManager.tileSize[0]) / 2f;

        int colIndex = Mathf.RoundToInt(col);
        int rowIndex = Mathf.RoundToInt(row);

        originTileCol = this.tileCol;
        originTileRow = this.tileRow;

        this.tileCol = colIndex;
        this.tileRow = rowIndex;
        
        _tileManager.tileMap[originTileRow, originTileCol] = null;
        
        //if (tileCol > _tileManager.tileLength &&
        //    tileRow > _tileManager.tileLength &&
        //    tileCol < -1 &&
        //    tileRow < -1)
            _tileManager.tileMap[tileRow, tileCol] = this;
    }
    
    public void SetNeighbors(TileData[,] tileMap, int maxRow, int maxCol)
    {
        if (tileRow > 0) up = tileMap[tileRow - 1, tileCol];
        if (tileRow < maxRow - 1) down = tileMap[tileRow + 1, tileCol];
        if (tileCol > 0) left = tileMap[tileRow, tileCol - 1];
        if (tileCol < maxCol - 1) right = tileMap[tileRow, tileCol + 1];
    }
    
    public virtual void UpdateWorldLevel(int level)
    {}
    
    public int UpdateTileIndex()
    {
        return tileCol + tileRow * _tileManager.tileLength + 1;
    }

    
}
