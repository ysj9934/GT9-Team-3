using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRoad : MonoBehaviour
{
    private GameManager _gameManager;
    private TileManager _tileManager;
    public TileRoadConnector _tileRoadConnector;

    // 이웃한 타일 정보 (자동으로 수정)
    [Header("Tile Neighbors")] 
    public TileRoad up;
    public TileRoad down;
    public TileRoad left;
    public TileRoad right;

    [Header("Tile Info")]
    [SerializeField] public float[] cellSize = { 3.6f,  2.1f };    // 타일 사이즈
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
    }

    public void Initialize(int mapLevel, Vector2 pos)
    {
        this.mapLevel = mapLevel;
        GetMapping(GetGridSize(this.mapLevel), pos);
        UpdateTileSerialNumber();
        // this.row = row;
        // this.col = col;
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

    public void GetMapping(int gridSize, Vector2 pos)
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
        
        _tileManager.tileMap[rowIndex, colIndex] = this;
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
    
    // 이웃한 타일 찾기
    // Q. 매번 찾아야하는가? 
    // Q. 길찾기 전에만 찾으면 되는건가?
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












    // void OnMouseDrag()
    // {
    //     Plane plane = new Plane(Vector3.forward, 0);
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     if (plane.Raycast(ray, out float distance))
    //     {
    //         Vector3 point = ray.GetPoint(distance);
    //         transform.position = SnapToGrid(point); // 격자 스냅 함수
    //     }
    // }
    //
    // Vector3 SnapToGrid(Vector3 pos)
    // {
    //     float x = Mathf.Round(pos.x / cellSize[0]) * cellSize[0];
    //     float y = Mathf.Round(pos.y / cellSize[1]) * cellSize[1];
    //     return new Vector3(x, y, pos.z);
    // }

}
