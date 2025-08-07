using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }
    private GameManager _gameManager;
    
    [Header("Tile Prefabs")]
    [SerializeField] private GameObject[] tilePrefabs;  // 타일_길타입 프리팹
    [SerializeField] private GameObject tileGridPrefab; // 타일_그리드 프리팹
    [SerializeField] private GameObject tileBasePrefab; // 타일_기지 프리펩
    [SerializeField] private GameObject tileSpawnPrefab;// 타일_스포너 프리팹

    private int mapLevel = 1;
    private int cellSize = 5;
    private float cellPos;
    
    public TileRoad[,] tileMap;
    public TileRoad startTileRoad;
    public TileRoad endTileRoad;
    
    public List<TileRoad> path = new List<TileRoad>();
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void Initialize()
    {
        UpdateLevel(mapLevel);
        ClearTiles();
        SetGridTile();
        SetBaseTile();
    }

    // 맵 레벨 업데이트
    public void UpdateLevel(int level)
    {
        this.mapLevel = level;

        switch (mapLevel)
        {
            case 1:
                cellSize = 5;
                break;
            case 2:
                cellSize = 7;
                break;
            case 3:
                cellSize = 9;
                break;
        }

        cellPos = Mathf.Ceil((float)cellSize / 2f);
    }

    public void ClearTiles()
    {
        _gameManager.DestroyOfType<TileRoad>();
    }

    public void SetGridTile()
    {
        
        tileMap = new TileRoad[cellSize, cellSize];
        int tileNumber = 1;
        
        for (int row = 0; row < cellSize; row++)
        {
            for (int col = 0; col < cellSize; col++)
            {
                Vector2 pos = new Vector3(col * 3.6f - (3.6f * row), (row + col) * -2.1f + 8.4f);
                GameObject go = Instantiate(tileGridPrefab, pos, Quaternion.identity);
                go.transform.SetParent(transform);
                TileRoad tileRoad = go.GetComponent<TileRoad>();
                tileRoad.Initialize(mapLevel, pos);
                tileRoad.tileSerialNumber = tileNumber++;
                tileMap[row, col] = tileRoad;
            }
        }
    }

    public void SetBaseTile()
    {
        int baseNumber = 1;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (baseNumber != 5)
                {
                    Vector2 pos = new Vector3(col * 3.6f - (3.6f * row), (row + col) * -2.1f + 4.2f);
                    GameObject go = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)], pos, Quaternion.identity);
                    go.transform.SetParent(transform);
                    TileRoad tileRoad = go.GetComponent<TileRoad>();
                    tileRoad.Initialize(mapLevel, pos);
                    
                    // tileRoad.tileSerialNumber = tileNumber++;
                    tileMap[row + 1, col + 1] = tileRoad;
                }
                else
                {
                    Vector2 pos = new Vector3(col * 3.6f - (3.6f * row), (row + col) * -2.1f + 4.2f);
                    GameObject go = Instantiate(tileBasePrefab, pos, Quaternion.identity);
                    go.transform.SetParent(transform);
                    TileRoad tileRoad = go.GetComponent<TileRoad>();
                    tileRoad.Initialize(mapLevel, pos);
                    // tileRoad.tileSerialNumber = tileNumber++;
                    tileMap[row + 1, col + 1] = tileRoad;
                }

                baseNumber++;
            }
        }
    }




































    // private GameManager gameManager;
    //
    // [SerializeField] private GameObject baseTilePrefab;
    // // [SerializeField] private GameObject[] tilePrefabs;
    //
    // [SerializeField] public Vector2 startPosition = Vector2.zero;
    //
    // [SerializeField] private List<GameObject> tileList = new List<GameObject>();
    //
    //
    // private void Start()
    // {
    //     gameManager = GameManager.Instance;
    //     
    //     InitBaseTile(gameManager.blockCount);
    // }
    //
    // // 타일 초기 세팅
    // public void InitBaseTile(int blockCount)
    // {
    //     // 타일 초기화
    //     ResetTile();
    //     
    //     // base tile setting
    //     CreateBaseTile(blockCount);
    // }
    //
    // // 타일 초기화
    // public void ResetTile()
    // {
    //     gameManager.DestroyOfType<Tile>();
    //     tileList.Clear();
    // }
    //
    // // 베이스 타일 생성
    // private void CreateBaseTile(int blockCount)
    // {
    //     // 타일 위치 초기화
    //     switch (blockCount)
    //     {
    //         case 5:
    //             startPosition = new Vector2(0, 4.2f);
    //             break;
    //         case 7:
    //             startPosition = new Vector2(0, 8.4f);
    //             break;
    //         case 9:
    //             startPosition = new Vector2(0, 12.6f);
    //             break;
    //     }
    //     
    //     // 타일 
    //     int[] blocksPerRow2 = {1, 2, 3, 2, 1};
    //     
    //     for (int row = 0; row < blocksPerRow2.Length; row++)
    //     {
    //         int count = blocksPerRow2[row];
    //         float offset = -(count - 1);
    //         
    //         for (int i = 0; i < count; i++)
    //         {
    //             // base tile
    //             if (row == 2 && i == 1)
    //             {
    //                 Vector2 pos = startPosition + new Vector2(offset * 3.6f + 7.2f * i, -row * 2.1f);
    //                 GameObject go = GetBaseTile(baseTilePrefab, pos);
    //                 SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
    //                 for (int j = 0; j < renderers.Length; j++)
    //                 {
    //                     renderers[j].sortingOrder += row * 10 -500;
    //                 }
    //             
    //                 tileList.Add(go);
    //             }
    //             // basic tile
    //             else
    //             {
    //                 Vector2 pos = startPosition + new Vector2(offset * 3.6f + 7.2f * i, -row * 2.1f);
    //                 GameObject go = GetTile(tilePrefabs, pos);
    //                 SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
    //                 for (int j = 0; j < renderers.Length; j++)
    //                 {
    //                     renderers[j].sortingOrder += row * 10 -500;
    //                 }
    //             
    //                 tileList.Add(go);
    //             }
    //         }
    //     }
    // }
    //
    // // base tile
    // private GameObject GetBaseTile(GameObject prefab, Vector2 pos)
    // {
    //     return Instantiate(prefab, pos, Quaternion.identity);
    // }
    //
    // // basic tile
    // private GameObject GetTile(GameObject[] prefab, Vector2 pos)
    // {
    //     return Instantiate(prefab[Random.Range(0, prefab.Length)], pos, Quaternion.identity);
    // }

}
