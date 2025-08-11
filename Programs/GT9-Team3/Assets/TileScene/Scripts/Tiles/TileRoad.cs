using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileRoad : MonoBehaviour
{
    private GameManager _gameManager;
    private TileManager _tileManager;
    public TileRoadConnector _tileRoadConnector;
    public TileRotate _tileRotate;

    // 이웃한 타일 정보 (자동으로 수정)
    [Header("Tile Neighbors")] 
    public TileRoad up;
    public TileRoad down;
    public TileRoad left;
    public TileRoad right;

    [Header("Tile Info")]
    public float[] cellSize = { 1.4475f,  0.84f };    // 타일 사이즈
    [SerializeField] public int tileSerialNumber;                   // 타일 시리얼번호 (자동으로 수정)
    public int col, row;                                            // 타일 위치 정보 (자동으로 수정)
    public TileRotation currentRotationIndex = TileRotation.Degree_0;
    public bool connectedUp, 
        connectedDown, 
        connectedLeft, 
        connectedRight;

    public int mapLevel = 1;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _tileManager = TileManager.Instance;
        _tileRotate = GetComponent<TileRotate>();
    }

    // tile initialize
    public void Initialize(int mapLevel, Vector2 pos)
    {
        this.mapLevel = mapLevel;
        UpdateMapping(GetGridSize(this.mapLevel), pos);
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

    // Update Mapping
    public void UpdateMapping(int gridSize, Vector2 pos)
    {
        float originX = 0f;
        float originY = cellSize[1] * 2 + (cellSize[1] * 2 * mapLevel);

        float dx = pos.x - originX;
        float dy = originY - pos.y;

        float col = (dx / cellSize[0] + dy / cellSize[1]) / 2f;
        float row = (dy / cellSize[1] - dx / cellSize[0]) / 2f;

        int colIndex = Mathf.RoundToInt(col);
        int rowIndex = Mathf.RoundToInt(row);

        this.col = colIndex;
        this.row = rowIndex;

        Vector2 originPosition = transform.position;
        try
        {
            _tileManager.tileMap[rowIndex, colIndex] = this;
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError($"{e}");
            transform.position = originPosition;
        }
        
    }

    public void UpdateTileSerialNumber()
    {
        tileSerialNumber = col + (row * 5) + 1;

        UpdateTileOrder();
    }

    public void UpdateTileOrder()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sortingOrder = -1000 + (tileSerialNumber * 10) + i;
        }
    }
    
    public void SetNeighbors(TileRoad[,] map, int maxRow, int maxCol)
    {
        ClearNeighbors();
        if (row > 0) up = map[row - 1, col];
        if (row < maxRow - 1) down = map[row + 1, col];
        if (col > 0) left = map[row, col - 1];
        if (col < maxCol - 1) right = map[row, col + 1];
    }

    private void ClearNeighbors()
    {
        up = null;
        down = null;
        left = null;
        right = null;
    }

    public BlockPlaceTower[] blocks = new BlockPlaceTower[9];

    public void ReceiveBlockNumber(int blockNumber)
    {
        BlockInfo[] blockinfo1 = _tileRotate.rotatedPrefabs[1].GetComponentsInChildren<BlockInfo>();
        blockinfo1[0].GetComponent<BlockPlaceTower>().PlaceTower();
        BlockInfo[] blockinfo2 = _tileRotate.rotatedPrefabs[2].GetComponentsInChildren<BlockInfo>();
        blockinfo2[6].GetComponent<BlockPlaceTower>().PlaceTower();
        BlockInfo[] blockinfo3 = _tileRotate.rotatedPrefabs[3].GetComponentsInChildren<BlockInfo>();
        blockinfo3[8]?.GetComponent<BlockPlaceTower>().PlaceTower();
    }

    public BlockPlaceTower GetBlockNumber(int blockNumber)
    {
        return blocks[blockNumber - 1];
    }


}
