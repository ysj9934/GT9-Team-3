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
    public GameManager _gameManager;

    // ??? ?????? ????
    // tile size setting
    public readonly float[] tileSize = { 1.4475f, 0.84f };
    public int tileLength = 5;

    public int worldLevel = 1;

    // ??? ?????? ????
    [SerializeField] public GameObject[] tilePrefabs;
    [SerializeField] public GameObject tileGridPrefab;
    [SerializeField] public GameObject tileCastlePrefab;
    [SerializeField] public GameObject tileSpawnPrefab;

    public TileData[,] tileMap;

    public List<TileGrid1> tileGridList;
    public List<TileInfo1> tileInfoList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        InitializeTileMap(tileLength);
        SetTileGrid(tileLength);
        SetTileRoad(tileLength);
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
                    GameObject go = Instantiate(tilePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length)],
                        pos,
                        Quaternion.identity);
                    go.transform.SetParent(transform);
                    TileInfo1 tileInfo = go.GetComponent<TileInfo1>();
                    tileInfo.Initialize(pos);


                    tileInfoList.Add(tileInfo);
                }
                else {
                    Vector2 pos = new Vector2(col * tileSize[0] - row * tileSize[0], (col + row) * -tileSize[1] + tileSize[1] * 2);
                    GameObject go = Instantiate(tileCastlePrefab,
                        pos,
                        Quaternion.identity);
                    go.transform.SetParent(transform);
                    TileCastle tileCastle = go.GetComponent<TileCastle>();
                    tileCastle.Initialize(pos);
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
}
