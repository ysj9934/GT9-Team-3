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

    
    /// <summary>
    /// 타워 설치
    /// 2025.08.12
    /// 추후 재작성 필요
    /// </summary>
    public BlockInfo[] blocks = new BlockInfo[9];

    public void ReceiveBlockNumber(int blockNumber)
    {
        BlockInfo[] blockinfo1 = _tileRotate.rotatedPrefabs[0].GetComponentsInChildren<BlockInfo>();
        BlockInfo[] blockinfo2 = _tileRotate.rotatedPrefabs[1].GetComponentsInChildren<BlockInfo>();
        BlockInfo[] blockinfo3 = _tileRotate.rotatedPrefabs[2].GetComponentsInChildren<BlockInfo>();
        BlockInfo[] blockinfo4 = _tileRotate.rotatedPrefabs[3].GetComponentsInChildren<BlockInfo>();
        
        switch (blockNumber)
        {
            case 1:
                blockinfo1[0].PlaceTower();
                blockinfo2[6].PlaceTower();
                blockinfo3[8].PlaceTower();
                blockinfo4[2].PlaceTower();
                break;
            case 2:
                blockinfo1[1].PlaceTower();
                blockinfo2[3].PlaceTower();
                blockinfo3[7].PlaceTower();
                blockinfo4[5].PlaceTower();
                break;
            case 3:
                blockinfo1[2].PlaceTower();
                blockinfo2[0].PlaceTower();
                blockinfo3[6].PlaceTower();
                blockinfo4[8].PlaceTower();
                break;
            case 4:
                blockinfo1[3].PlaceTower();
                blockinfo2[7].PlaceTower();
                blockinfo3[5].PlaceTower();
                blockinfo4[1].PlaceTower();
                break;
            case 6:
                blockinfo1[5].PlaceTower();
                blockinfo2[1].PlaceTower();
                blockinfo3[3].PlaceTower();
                blockinfo4[7].PlaceTower();
                break;
            case 7:
                blockinfo1[6].PlaceTower();
                blockinfo2[8].PlaceTower();
                blockinfo3[2].PlaceTower();
                blockinfo4[0].PlaceTower();
                break;
            case 8:
                blockinfo1[7].PlaceTower();
                blockinfo2[5].PlaceTower();
                blockinfo3[1].PlaceTower();
                blockinfo4[3].PlaceTower();
                break;
            case 9:
                blockinfo1[8].PlaceTower();
                blockinfo2[2].PlaceTower();
                blockinfo3[0].PlaceTower();
                blockinfo4[6].PlaceTower();
                break;
        }
    }

    public BlockInfo GetBlockNumber(int blockNumber)
    {
        return blocks[blockNumber - 1];
    }
    
    


}
