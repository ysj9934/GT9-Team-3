using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


/// <summary>
/// TileManager
/// created by: yoon
/// created at: 2025.08.18
/// description:
/// TileManager refactoring
/// </summary>
public class TileManager1 : MonoBehaviour
{
    public static TileManager1 Instance { get; private set; }
    
    // 참조
    // Reference
    public GameManager12 _gameManager;

    // ??? ?????? ????
    // tile size setting
    public readonly float[] tileSize = { 1.4475f, 0.84f };
    public int tileLength = 5;

    public int worldLevel = 1;
    public int tempLevel = 1;

    // ??? ?????? ????
    [SerializeField] public GameObject[] tilePrefabs;
    [SerializeField] public GameObject tileGridPrefab;
    [SerializeField] public GameObject tileCastlePrefab;
    [SerializeField] public GameObject tileSpawnPrefab;

    public TileData[,] tileMap;
    public TileData startTile;
    public TileData endTile;

    [SerializeField] private GameObject pathfinderPrefab;
    public List<TileData> path;
    public List<TileGrid1> tileGridList;
    public List<TileInfo1> tileInfoList;
    public List<Vector2> spawnTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        
    }

    private void Start()
    {
        _gameManager = GameManager12.Instance;
        
        Initialize();
    }

    public void Initialize()
    {
        InitializeTileMap(tileLength);
        SetTileGrid(tileLength);
        SetTileRoad(tileLength);
        SetSpawnerPosition();
    }

    // tileMap ????
    // Initialize the tile map with the given dimensions
    private void InitializeTileMap(int tileLength)
    {
        tileMap = new TileData[tileLength, tileLength];
    }

    // 월드 변경시 타일 변경
    public void UpdateWorldLevel(int level)
    {
        worldLevel = level;

        foreach (var tileInfo in tileInfoList)
        {
            tileInfo.UpdateWorldLevel(worldLevel);
        }
        
        startTile.UpdateWorldLevel(worldLevel);
        endTile.UpdateWorldLevel(worldLevel);
    }

    public void UpdateTempLevel(int level)
    {
        tempLevel = level;
        tileLength = 3 + (2 * tempLevel);
    }

    public void SetTileGrid(int tileLength)
    {
        tileGridList = new List<TileGrid1>();

        for (int row = 0; row < tileLength; row++)
        {
            for (int col = 0; col < tileLength; col++)
            {
                Vector2 pos = new Vector2(col * tileSize[0] - row * tileSize[0], (col + row) * -tileSize[1] + tileSize[1] * 2 + (tileSize[1] * 2));
                GameObject go = Instantiate(tileGridPrefab, 
                    pos, 
                    Quaternion.identity);
                go.transform.SetParent(transform);
                TileGrid1 tileGrid = go.GetComponent<TileGrid1>();
                tileGrid.Initialize(pos);

                tileGridList.Add(tileGrid);
            }
        }
    }

    public void SetTileRoad(int tileLength)
    {
        tileInfoList = new List<TileInfo1>();
        int baseNumber = 0;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (baseNumber != 4){
                    Vector2 pos = new Vector2(col * tileSize[0] - row * tileSize[0], (col + row) * -tileSize[1] + tileSize[1] * 2);
                    GameObject go = Instantiate(
                        tilePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length)],
                        pos,
                        Quaternion.identity);
                    go.transform.SetParent(transform);
                    TileInfo1 tileInfo = go.GetComponent<TileInfo1>();
                    tileInfo.Initialize(pos);


                    tileInfoList.Add(tileInfo);
                }
                else {
                    Vector2 pos = new Vector2(col * tileSize[0] - row * tileSize[0], (col + row) * -tileSize[1] + tileSize[1] * 2);
                    GameObject go = Instantiate(
                        tileCastlePrefab,
                        pos,
                        Quaternion.identity);
                    go.transform.SetParent(transform);
                    TileCastle tileCastle = go.GetComponent<TileCastle>();
                    tileCastle.Initialize(pos);

                    endTile = tileCastle;
                }
                    
                baseNumber++;
            }
        }
    }

    // 이웃한 타일 정보 보기
    public void SetNeighbors()
    {
        for (int row = 0; row < tileLength; row++)
        {
            for (int col = 0; col < tileLength; col++)
            {
                if (tileMap[row, col] != null)
                {
                    tileMap[row, col].SetNeighbors(tileMap, tileLength, tileLength);
                }
            }
        }
    }

    public void SetSpawnerPosition()
    {
        spawnTransform = new List<Vector2>();
        ClearSpawner();

        switch (tempLevel)
        {
            case 1:
                spawnTransform.Add(tileGridList[4].transform.position);
                break;
            case 2:
                spawnTransform.Add(tileGridList[6].transform.position);
                break;
        }

        CreateSpawner();
    }
    
    public void CreateSpawner()
    {
        if (spawnTransform.Count == 0)
            return;
        
        Vector2 pos = spawnTransform[0];
        GameObject go = Instantiate(
            tileSpawnPrefab, 
            pos, 
            Quaternion.identity);
        go.transform.SetParent(transform);
        TileSpawner tileSpawner = go.GetComponent<TileSpawner>();
        tileSpawner.Initialize(pos);

        startTile = tileSpawner;
    }
    
    
    public void ClearSpawner()
    {
        _gameManager.DestroyOfType<TileSpawner>();
    }

    public void ShowConnectedPath()
    {
        SetNeighbors();

        if (startTile == null)
        {
            Debug.Log("startTileRoad is null");
        }
        else if (endTile == null)
        {
            Debug.Log("endTileRoad is null");
        }
        else
        {
            path = FindConnectedPath(startTile, endTile);
            //path = FindConnectedPathBFS(startTileRoad, endTileRoad);

            if (path != null && path.Count > 0)
                GetPathfinder();
        }
    }
    
    public List<TileData> FindConnectedPath(TileData startTile, TileData endTile)
    {
        Debug.Log("FindConnectedPath");
        var visited = new HashSet<TileData>();
        var path = new List<TileData>();

        if (DFS(startTile, endTile, visited, path))
            return path;

        return new List<TileData>();
    }

    private bool DFS(TileData current, TileData target, HashSet<TileData> visited, List<TileData> path)
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
            Debug.Log($"currentTile : {current.tileCol}, {current.tileCol}");
            if (DFS(neighbor, target, visited, path))
                return true;
        }

        path.Remove(current);
        visited.Remove(current);
        return false;
    }
    
    public void GetPathfinder()
    {
        GameObject go = Instantiate(pathfinderPrefab, startTile.transform);
        Pathfinder1 pathfinder = go.GetComponent<Pathfinder1>();
        pathfinder.Initialize(path);

        // waveStartButton.WakeOnButton();
        // WaveManager.Instance.Initilaize(path, startTile.transform);
    }
}
