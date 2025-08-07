using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    [SerializeField] public float[] cellSize = { 3.6f,  2.1f };
    public int col, row;
    [SerializeField] public int tileSerialNumber;
    private int mapLevel = 1;
    
    public void Initialize(int mapLevel, Vector2 pos)
    {
        this.mapLevel = mapLevel;
        GetMapping(GetGridSize(this.mapLevel), pos);
        UpdateTileSerialNumber();
    }
    
    public int GetGridSize(int mapLevel)
    {
        switch (mapLevel)
        {
            case 1:
                return 5;
            case 2:
                return 7;
            case 3:
                return 9;
            default:
                return 5;
        };
    }
    
    private void GetMapping(int gridSize, Vector2 pos)
    {
        float originX = 0f;
        float originY = 8.4f;

        float dx = pos.x - originX;
        float dy = originY - pos.y;

        float col = (dx / cellSize[0] + dy / cellSize[1]) / 2f;
        float row = (dy / cellSize[1] - dx / cellSize[0]) / 2f;

        int colIndex = Mathf.RoundToInt(col);
        int rowIndex = Mathf.RoundToInt(row);

        this.col = colIndex;
        this.row = rowIndex;
    }

    private void UpdateTileSerialNumber()
    {
        tileSerialNumber = col + (row * 5) + 1;

        UpdateTileOrder();
    }

    private void UpdateTileOrder()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sortingOrder = -1000 + (tileSerialNumber * 10) + i;
        }
    }
}
