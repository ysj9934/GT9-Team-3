using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid1 : MonoBehaviour
{
    public TileManager1 _tileManager;

    [SerializeField] public TileCategory tileCategory;
    [SerializeField] public TileShape tileShape;

    [Header("Tile Neighbors")]
    public TileData up;
    public TileData down;
    public TileData left;
    public TileData right;
    
    public int tileIndex;
    public int originTileCol = -1;
    public int originTileRow = -1;
    public int tileCol;
    public int tileRow;

    protected virtual void Awake()
    {
        _tileManager = TileManager1.Instance;
    }
    
    public void Initialize(Vector2 pos)
    {
        UpdateMapping(pos);
        tileIndex = UpdateTileIndex();
        UpdateSpriteOrder();
    }

    protected void UpdateMapping(Vector2 pos)
    {
        float originX = 0f;
        float originY = _tileManager.tileSize[1] * 2 + (_tileManager.tileSize[1] * 2);

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
    }
    
    protected int UpdateTileIndex()
    {
        return tileCol + tileRow * _tileManager.tileLength + 1;
    }
    
    private void UpdateSpriteOrder()
    { 
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sortingOrder = sr.sortingOrder * tileIndex - 1000;
        }
    }
}
