using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    [SerializeField] private CinemachineVirtualCamera[] levelCamera;

    public int mapLevel = 1;
    private int cellSize = 5;
    private float cellPos;
    
    public TileRoad[,] tileMap;
    public TileRoad startTileRoad;
    public TileRoad endTileRoad;

    [SerializeField] private GameObject pathfinderPrefab;
    public List<TileRoad> path;
    public List<TileGrid> gridTileList;
    public List<Vector2> spawnTransform;
    
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
        CameraMove();
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

    private void CameraMove()
    {
        foreach (var vcamera in levelCamera)
        {
            vcamera.Priority = 0;
        }

        levelCamera[mapLevel - 1].Priority = 20;
    }

    public void SetGridTile()
    {
        
        gridTileList = new List<TileGrid>();
        _gameManager.DestroyOfType<TileGrid>();
        tileMap = new TileRoad[cellSize, cellSize];
        
        for (int row = 0; row < cellSize; row++)
        {
            for (int col = 0; col < cellSize; col++)
            {
                Vector2 pos = new Vector3(col * 3.6f - (3.6f * row), (row + col) * -2.1f + 4.2f + (4.2f * mapLevel));
                GameObject go = Instantiate(tileGridPrefab, pos, Quaternion.identity);
                go.transform.SetParent(transform);
                TileGrid tileGrid = go.GetComponent<TileGrid>();
                tileGrid.Initialize(mapLevel, pos);
                
                gridTileList.Add(tileGrid);
            }
        }
    }

    public void SetBaseTile()
    {
        _gameManager.DestroyOfType<TileRoad>();
        int baseNumber = 1;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                // 주변 8타일
                if (baseNumber != 5)
                {
                    Vector2 pos = new Vector3(col * 3.6f - (3.6f * row), (row + col) * -2.1f + 4.2f);
                    GameObject go = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)], pos, Quaternion.identity);
                    go.transform.SetParent(transform);
                    TileRoad tileRoad = go.GetComponent<TileRoad>();
                    tileRoad.Initialize(mapLevel, pos);
                    
                    tileMap[row + mapLevel, col + mapLevel] = tileRoad;
                }
                // 기지 타일
                else
                {
                    Vector2 pos = new Vector3(col * 3.6f - (3.6f * row), (row + col) * -2.1f + 4.2f);
                    GameObject go = Instantiate(tileBasePrefab, pos, Quaternion.identity);
                    go.transform.SetParent(transform);
                    TileRoad tileRoad = go.GetComponent<TileRoad>();
                    tileRoad.Initialize(mapLevel, pos);
                    
                    tileMap[row + mapLevel, col + mapLevel] = tileRoad;
                    endTileRoad = tileRoad;
                }

                baseNumber++;
            }
        }
    }

    public void SetNeighbors()
    {
        for (int row = 0; row < cellSize; row++)
        {
            for (int col = 0; col < cellSize; col++)
            {
                Debug.Log($"tileMap[{row},{col}]");
                if (tileMap[row, col] != null)
                {
                    tileMap[row, col].SetNeighbors(tileMap, cellSize, cellSize);
                }
                
                Debug.Log($"tileMap : {tileMap[row, col]}");
            }
        }
    }

    public void SetSpawnerPosition()
    {
        spawnTransform = new List<Vector2>();
        ClearSpawner();
        
        switch (mapLevel)
        {
            case 1:
                
                spawnTransform.Add(gridTileList[2].transform.position);
                spawnTransform.Add(gridTileList[10].transform.position);
                spawnTransform.Add(gridTileList[14].transform.position);
                spawnTransform.Add(gridTileList[22].transform.position);
                break;
            case 2:
                spawnTransform.Add(gridTileList[3].transform.position);
                spawnTransform.Add(gridTileList[21].transform.position);
                spawnTransform.Add(gridTileList[27].transform.position);
                spawnTransform.Add(gridTileList[45].transform.position);
                break;
            case 3:
                spawnTransform.Add(gridTileList[4].transform.position);
                spawnTransform.Add(gridTileList[36].transform.position);
                spawnTransform.Add(gridTileList[44].transform.position);
                spawnTransform.Add(gridTileList[76].transform.position);
                break;
        }

        CreateSpawner();
    }

    public void CreateSpawner()
    {
        if (spawnTransform.Count == 0)
            return;
        
        Vector2 pos = spawnTransform[Random.Range(0, spawnTransform.Count)];
        GameObject go = Instantiate(tileSpawnPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        TileSpawner tileS = go.GetComponent<TileSpawner>();
        TileRoad tile = go.GetComponent<TileRoad>();
        
        tile.Initialize(mapLevel, pos);

        startTileRoad = tile;
    }
    
    public void ClearSpawner()
    {
        _gameManager.DestroyOfType<TileSpawner>();
    }

    public void ShowConnectedPath()
    {
        path = new List<TileRoad>();
        
        SetNeighbors();

        if (startTileRoad == null)
        {
            Debug.Log("startTileRoad is null");
        }
        else if (endTileRoad == null)
        {
            Debug.Log("endTileRoad is null");
        }
        else
        {
            path = FindConnectedPath(startTileRoad, endTileRoad);
            GetPathfinder();
        } 
    }

    public List<TileRoad> FindConnectedPath(TileRoad startTile, TileRoad endTile)
    {
        Debug.Log("FindConnectedPath");
        var visited = new HashSet<TileRoad>();
        var path = new List<TileRoad>();

        if (DFS(startTile, endTile, visited, path))
        {
            Debug.Log("The path is complete");
            return path;    
        }

        return new List<TileRoad>();
    }

    private bool DFS(TileRoad current, TileRoad target, HashSet<TileRoad> visited, List<TileRoad> path)
    {
        Debug.Log("DFS");
        if (current == null || visited.Contains(current))
            return false;
        
        visited.Add(current);
        path.Add(current);

        if (current == target)
            return true;

        foreach (var (neighbor, _) in current._tileRoadConnector.GetConnectedNeighbors())
        {
            if (DFS(neighbor, target, visited, path))
                return true;
        }

        path.Remove(current);
        Debug.LogError("The path is invalid");
        return false;
    }

    public void GetPathfinder()
    {
        GameObject go = Instantiate(pathfinderPrefab, startTileRoad.transform);
        Pathfinder pathfinder = go.GetComponent<Pathfinder>();
        pathfinder.Initialize(path);
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
