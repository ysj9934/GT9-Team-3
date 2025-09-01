using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    // Managers
    public TileManager _tileManager;

    // Tile Component
    [SerializeField] public TileCategory tileCategory;
    [SerializeField] public TileShape tileShape;

    // Tile Info
    public int tileIndex;
    public int tileCol;
    public int tileRow;

    // Block Info
    public BlockInfo[] blockInfos;
    public Dictionary<SpriteRenderer, int> originBlockOrder = new Dictionary<SpriteRenderer, int>();
    private bool originOrderInitialized = false;

    private void Awake()
    {
        _tileManager = TileManager.Instance;

        IsValidate();
    }

    private bool IsValidate()
    {
        if (_tileManager == null)
        {
            ValidateMessage(_tileManager.name);
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

    /// <summary>
    /// Tile Initialize
    /// 타일 정보 초기화 
    /// </summary>
    /// <param name="pos">타일 위치</param>
    public void Initialize(Vector2 pos)
    {
        CacheOriginOrders();
        UpdateMapping(pos);
        UpdateTileIndex();
        SetBlockInfos();
        UpdateSpriteOrder();
    }

    /// <summary>
    /// Cache Block Origin Order
    /// 블럭의 최초 위치를 저장
    /// </summary>
    private void CacheOriginOrders()
    {
        if (originOrderInitialized) return;

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        originBlockOrder.Clear();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            originBlockOrder[sr] = sr.sortingOrder;
        }

        originOrderInitialized = true;
    }

    /// <summary>
    /// Tile Mapping
    /// Tile의 위치를 저장
    /// </summary>
    /// <param name="pos">타일 위치</param>
    protected void UpdateMapping(Vector2 pos)
    {
        float originX = 0f;
        float originY = _tileManager.tileSize[1] * 2 + (_tileManager.tileSize[1] * 2 * _tileManager.mapExtendLevel);

        float dx = pos.x - originX;
        float dy = originY - pos.y;

        float col = (dx / _tileManager.tileSize[0] + dy / _tileManager.tileSize[1]) / 2f;
        float row = (dy / _tileManager.tileSize[1] - dx / _tileManager.tileSize[0]) / 2f;

        int colIndex = Mathf.RoundToInt(col);
        int rowIndex = Mathf.RoundToInt(row);

        this.tileCol = colIndex;
        this.tileRow = rowIndex;
    }

    /// <summary>
    /// Tile Index
    /// Tile의 Index번호 작성
    /// </summary>
    /// <returns></returns>
    protected int UpdateTileIndex()
    {
        return tileIndex = tileCol + tileRow * _tileManager.tileLength + 1;
    }


    /// <summary>
    /// Set Block Infos
    /// 소유하고 있는 블럭을 저장
    /// </summary>
    private void SetBlockInfos()
    {
        blockInfos = GetComponentsInChildren<BlockInfo>();
    }

    /// <summary>
    /// sorting order blocks
    /// 블록의 order를 위치값에 맞게 지정
    /// </summary>    
    private void UpdateSpriteOrder()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            int baseOrder = originBlockOrder.ContainsKey(sr) ? originBlockOrder[sr] : 0;
            sr.sortingOrder = baseOrder + (tileIndex * 10) - 1000;
        }
    }

    public void UpdateWorldLevel(int level)
    {
        foreach (var blockInfo in blockInfos)
        {
            blockInfo.UpdateWorldLevel(level);
        }
    }

}
