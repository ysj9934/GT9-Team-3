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
    // Managers
    public TileManager _tileManager;

    // Tile Component
    public TileUI _tileUI;
    public TileMove _tileMove;
    public TileRoadConnector _tileRoadConnector;
    public TileLink _tileLink;

    // Tile Info
    [SerializeField] public TileCategory tileCategory;
    [SerializeField] public TileShape tileShape;
    public int tileIndex;
    public int originTileCol = -1;
    public int originTileRow = -1;
    public int tileCol;
    public int tileRow;
    public bool isInInventory = false;

    // Tile Neighbors
    [Header("Tile Neighbors")]
    public TileData up;
    public TileData down;
    public TileData left;
    public TileData right;
    
    // Tile Connect Road
    public bool connectedUp;
    public bool connectedDown;
    public bool connectedLeft;
    public bool connectedRight;

    protected virtual void Awake()
    {
        _tileManager = TileManager.Instance;
    }

    protected virtual void Start()
    {
        _tileUI = GetComponent<TileUI>();
        _tileMove = GetComponent<TileMove>();
        _tileRoadConnector = GetComponent<TileRoadConnector>();
        _tileLink = GetComponent<TileLink>();
        
        IsValidate();
    }

    protected virtual bool IsValidate()
    {
        if (_tileManager == null)
        {
            ValidateMessage(_tileManager.name);
            return false;
        }
        else if (_tileRoadConnector == null)
        {
            ValidateMessage(_tileRoadConnector.name);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ValidateMessage(string obj)
    {
        Debug.LogError($"{obj} is Valid");
    }

    public virtual void Initialize(Vector2 pos)
    {
        UpdateMapping(pos);
        UpdateTileIndex();
        _tileManager.SetNeighbors();
    }

    /// <summary>
    /// Tile Mapping
    /// Tile의 위치 갱신시 Map정보 수정
    /// </summary>
    /// <param name="pos">타일 위치 정보</param>
    public virtual void UpdateMapping(Vector2 pos)
    {
        float originX = 0f;
        float originY = _tileManager.tileSize[1] * 2 + (_tileManager.tileSize[1] * 2 * _tileManager.mapExtendLevel);

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

        // Castle를 부딪칠 경우
        if (_tileManager.mapExtendLevel == 1 &&
            this.gameObject.GetComponent<TileCastle>() == null)
        {
            if (tileCol == 2 && tileRow == 2)
            {
                transform.position = _tileMove.originalPosition;
                UpdateMapping(transform.position);
            }
            else if (originTileCol == 2 && originTileRow == 2)
            {
                _tileMove.LinkiedUI();
            }
        }
        else if (_tileManager.mapExtendLevel == 2 &&
            this.gameObject.GetComponent<TileCastle>() == null)
        {
            if (tileCol == 3 && tileRow == 3)
            {
                transform.position = _tileMove.originalPosition;
                UpdateMapping(transform.position);
            }
            else if (originTileCol == 3 && originTileRow == 3)
            {
                _tileMove.LinkiedUI();
            }
        }

        // 그리드 밖으로 나갔을 때
        if (!IsValidTilePosition(tileRow, tileCol) &&
            !IsValidTilePosition(originTileRow, originTileCol))
        {
            _tileMove.LinkiedUI();
        }
        else if (!IsValidTilePosition(tileRow, tileCol) &&
                IsValidTilePosition(originTileRow, originTileCol))
        {
            if (_tileManager.tileMap[originTileRow, originTileCol] != null &&
                _tileManager.tileMap[originTileRow, originTileCol] != this)
            {
                _tileMove.LinkiedUI();
            }
            else
            {
                transform.position = _tileMove.originalPosition;
                UpdateMapping(transform.position);
            }
        }
        else if (IsValidTilePosition(tileRow, tileCol) &&
                !IsValidTilePosition(originTileRow, originTileCol))
        {
            if (_tileManager.tileMap[tileRow, tileCol] != null &&
                _tileManager.tileMap[tileRow, tileCol] != this)
            {
                _tileMove.LinkiedUI();
            }
            else
            {
                _tileManager.tileMap[tileRow, tileCol] = this;
            }
        }
        else
        {
            if (_tileManager.tileMap[tileRow, tileCol] != null &&
                _tileManager.tileMap[tileRow, tileCol] != this)
            {
                if (_tileManager.tileMap[originTileRow, originTileCol] != null &&
                _tileManager.tileMap[originTileRow, originTileCol] != this)
                {
                    _tileMove.LinkiedUI();
                }
                else
                {
                    _tileManager.tileMap[originTileRow, originTileCol] = this;
                }
            }
            else
            {
                if (IsValidTilePosition(originTileRow, originTileCol))
                {
                    _tileManager.tileMap[originTileRow, originTileCol] = null;
                }

                _tileManager.tileMap[tileRow, tileCol] = this;
            }
        }
    }
    private bool IsValidTilePosition(int row, int col)
    {
        //Debug.Log($"Tile Position - Row: {row}, Col: {col}");
        return row >= 0 && row < _tileManager.tileMap.GetLength(0) &&
               col >= 0 && col < _tileManager.tileMap.GetLength(1);
    }

    /// <summary>
    /// Tile Index
    /// Tile의 위치 갱신 시 Index번호 수정
    /// </summary>
    /// <returns>타일 인덱스</returns>
    public int UpdateTileIndex()
    {
        return tileIndex = tileCol + tileRow * _tileManager.tileLength + 1;
    }

    /// <summary>
    /// Tile Mapping from neighbors
    /// Tile Mapping을 통하여 타일 주변의 이웃한 타일 정보 찾기
    /// </summary>
    /// <param name="tileMap">TileManager의 TileMap</param>
    /// <param name="maxRow">타일 세로 길이</param>
    /// <param name="maxCol">타일 가로 길이</param>
    public void SetNeighbors(TileData[,] tileMap, int maxRow, int maxCol)
    {
        if (tileRow > 0) up = tileMap[tileRow - 1, tileCol];
        if (tileRow < maxRow - 1) down = tileMap[tileRow + 1, tileCol];
        if (tileCol > 0) left = tileMap[tileRow, tileCol - 1];
        if (tileCol < maxCol - 1) right = tileMap[tileRow, tileCol + 1];
    }

    /// <summary>
    /// Tile World Level
    /// World Level 갱신시 타일 성격 변화
    /// </summary>
    /// <param name="level">월드 레벨</param>
    public virtual void UpdateWorldLevel(int level)
    {}
}
