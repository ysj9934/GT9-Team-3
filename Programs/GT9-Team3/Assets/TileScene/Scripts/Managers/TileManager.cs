using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cinemachine;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


/// <summary>
/// TileManager
/// created by: yoon
/// description:
/// 타일에 관한 전체적인 관리를 담당합니다.
/// </summary>
/// <remark>
/// created at: 2025.08.18
/// created at: 2025.08.28
/// TileManager refactoring
/// </remark>
public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }
    
    // Managers
    public GameManager _gameManager;
    public TowerBuildUI towerUIdnjswls;
    public TowerSellUI towerSellUI;

    // Manager Info
    public readonly float[] tileSize = { 1.4475f, 0.84f };
    public int tileLength = 5;
    public int gameWorldLevel;
    public int gameRoundLevel;
    public int mapExtendLevel = 1;

    // Tile Prefabs
    [SerializeField] public GameObject[] tilePrefabs;
    [SerializeField] public GameObject[] tileHardModePrefabs;
    [SerializeField] public GameObject tileGridPrefab;
    [SerializeField] public GameObject tileCastlePrefab;
    [SerializeField] public GameObject tileSpawnPrefab;

    // Tile Info
    public TileData[,] tileMap;
    public Vector2 spawnTransform;
    public TileData startTile;
    public TileData endTile;
    public List<GameObject> tileAllCategoryList;
    public List<TileGrid> tileGridList;
    public List<TileInfo> tileInfoList;
    public bool isHardMode;

    // Path Info
    [SerializeField] private GameObject pathfinderPrefab;
    public List<TileData> path;

    // Tile UI Active
    public bool isUIActive = false;
    public bool isMoveActive = false;

    // Tile Create (Inventory / shop)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.LogError("TileManager Instance Error");

        tileAllCategoryList = new List<GameObject>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        towerSellUI = TowerSellUI.Instance;
        towerUIdnjswls = FindObjectOfType<TowerBuildUI>(true);
    }

    // 데이터가 2번 호출되므로 초기화를 진행해도 된다.
    public void ReceiveStageData(StageData stageData)
    {
        if (stageData != null)
        {
            this.gameWorldLevel = stageData.worldCode;
            this.gameRoundLevel = stageData.roundCode;
            this.isHardMode = stageData.isHardMode;

            //Debug.Log("gameRoundLevel" + gameRoundLevel);
            if (!isHardMode)
            {
                if (stageData.waveCode < 7)
                {
                    mapExtendLevel = 1;
                }
                else
                {
                    mapExtendLevel = 2;
                }

                tileLength = 3 + (2 * mapExtendLevel);


                switch (mapExtendLevel)
                {
                    case 1:
                        Initialize();
                        break;
                    case 2:
                        InitializeMapResize();
                        break;
                    default:
                        Debug.LogWarning("Invalid mapExtendLevel");
                        break;
                }
            }
            else
            {
                mapExtendLevel = 2;
                tileLength = 3 + (2 * mapExtendLevel);

                Initialize();
            }
            
        }
        else
        {
            Debug.LogError("StageData is Null");
        }
    }

    /// <summary>
    /// Initialize
    /// 초기화
    /// </summary>
    public void Initialize()
    {
        tileAllCategoryList = new List<GameObject>();
        path = new List<TileData>();
        isUIActive = true;
        isMoveActive = true;
        SetMapResize(tileLength);
        SetTileGrid(tileLength);
        SetTileRoad(tileLength);
        SetSpawnerPosition();
        SetWorldTile(gameWorldLevel);
    }


    /// <summary>
    /// Resize Map
    /// 맵 크기 재조정
    /// </summary>
    public void InitializeMapResize()
    {
        SetMapResize(tileLength);
        SetTileGrid(tileLength);
        SetSpawnerPosition();

        foreach (var tileInfo in tileInfoList)
        {
            tileInfo.MapExtend(tileInfo.transform.position);
        }

        TileCastle tileCastle = endTile as TileCastle;
        if (tileCastle != null)
        {
            tileCastle.InitializeTemp(tileCastle.transform.position);
        }
        else
        {
            Debug.LogError("startTile is not a TileCastle");
        }
    }

    /// <summary>
    /// Set Tile World Version
    /// 월드 변경시 타일 변경
    /// </summary>
    /// <param name="level">gameWorldLevel</param>
    private void SetWorldTile(int level)
    {
        if (isHardMode)
            level = 1;

        foreach (var tile in tileAllCategoryList)
        {
            TileGrid tileGrid = tile.GetComponent<TileGrid>();
            TileData tileData = tile.GetComponent<TileData>();

            if (tileGrid != null)
            {
                tileGrid.UpdateWorldLevel(level);
            }
            else if (tileData != null)
            {
                tileData.UpdateWorldLevel(level);
            }
        }
    }

    /// <summary>
    /// Tile Map 크기 재조정
    /// Map resize dimensions
    /// </summary>
    /// <param name="tileLength">타일 종횡 길이</param>
    private void SetMapResize(int tileLength)
    {
        tileMap = new TileData[tileLength, tileLength];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (EventSystem.current.IsPointerOverGameObject()) return;
#elif UNITY_ANDROID || UNITY_IOS
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
#endif

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

            TileInfo fallbackTile = null;
            Tower1 fallbackTower = null;

            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    // Block Click Event
                    BlockInfo blockInfo = hit.collider.GetComponent<BlockInfo>();
                    if (blockInfo != null)
                    {
                        blockInfo.OpenTowerInstallerUI();
                        CloseTileUI(null);
                        CloseTowerInfoUI();
                        return;
                    }

                    if (fallbackTile == null)
                    {
                        TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
                        if (tileInfo != null)
                        {
                            fallbackTile = tileInfo;
                        }
                    }

                    if (fallbackTower == null)
                    {
                        Tower1 tower = hit.collider.GetComponent<Tower1>();
                        if (tower != null)
                        {
                            fallbackTower = tower;
                        }
                    }
                }
            }

            if (fallbackTower != null)
            {
                CloseTowerInstallUI();
                // 타워 선택시 event
                bool isAlreadyOpen = TowerSellUI.Instance.IsOpenFor(fallbackTower);

                if (isAlreadyOpen)
                {
                    TowerSellUI.Instance.Hide();

                    if (fallbackTower.rangeVisual != null)
                        fallbackTower.rangeVisual.SetActive(false);

                    return;
                }

                // 다른 타워거나 처음 열리는 경우 기존 UI 닫고 새로 열기
                TowerSellUI.Instance.Show(fallbackTower);

                if (fallbackTower.rangeVisual != null)
                    fallbackTower.rangeVisual.SetActive(fallbackTower);

                // 업그레이드 정보
                TowerUpgradeUI ui = FindObjectOfType<TowerUpgradeUI>();
                if (ui != null)
                {
                    ui.SetTargetTower(fallbackTower);
                }

            }
            else if (fallbackTile != null)
            {
                CloseTowerInstallUI();

                if (isUIActive)
                {
                    CloseTileUI(fallbackTile._tileUI);
                    _gameManager._hudCanvas.TurnOffStartWave();
                    fallbackTile._tileUI.tileUI.SetActive(!fallbackTile._tileUI.tileUI.activeSelf);
                }

                if (isMoveActive)
                {
                    TileMovePress(fallbackTile);
                }
            }
            else
            {
                // 타일이 아닌 곳을 찍을 경우 UI 닫기
                CloseTileUI(null);
                CloseTowerInstallUI();
                CloseTowerInfoUI();
            }
        }

        TileUICollider(isUIActive);
    }

    private void TileUICollider(bool isUIActive)
    { 
        foreach (var tileInfo in tileInfoList)
        {
            tileInfo.collider2D.enabled = isUIActive;
        }
    }

    /// <summary>
    /// Save Tile Data 
    /// 타일을 클릭하여 타일 정보 저장
    /// </summary>
    /// <param name="tileInfo">선택된 타일</param>
    private void TileMovePress(TileInfo tileInfo)
    {
        tileInfo._tileMove.TileMovePress();
    }

    /// <summary>
    /// Set Tile Grid
    /// 그리드 타일 설치
    /// </summary>
    /// <param name="tileLength">타일 길이</param>
    private void SetTileGrid(int tileLength)
    {
        _gameManager = GameManager.Instance;
        tileGridList = new List<TileGrid>();
        _gameManager.DestroyOfType<TileGrid>();

        for (int row = 0; row < tileLength; row++)
        {
            for (int col = 0; col < tileLength; col++)
            {
                Vector2 pos = new Vector2(
                    col * tileSize[0] - row * tileSize[0], 
                    (col + row) * -tileSize[1] + tileSize[1] * 2 + (tileSize[1] * 2 * mapExtendLevel));
                GameObject go = Instantiate(
                    tileGridPrefab, 
                    pos, 
                    Quaternion.identity);
                go.transform.SetParent(transform);
                TileGrid tileGrid = go.GetComponent<TileGrid>();
                tileGrid.Initialize(pos);

                tileGridList.Add(tileGrid);
                tileAllCategoryList.Add(go);
            }
        }
    }

    /// <summary>
    /// Set Tile Road
    /// 길 타일 설치 + 성 타일 설치
    /// </summary>
    /// <param name="tileLength">타일 길이</param>

    private void SetTileRoad(int tileLength)
    {
        tileInfoList = new List<TileInfo>();
        _gameManager.DestroyOfType<TileData>();
        int baseNumber = 0;

        if (!isHardMode)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    // Set Tile Road
                    if (baseNumber != 4)
                    {
                        Vector2 pos = new Vector2(
                            col * tileSize[0] - row * tileSize[0],
                            (col + row) * -tileSize[1] + tileSize[1] * 2);
                        GameObject go = Instantiate(
                            tilePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length)],
                            pos,
                            Quaternion.identity);
                        go.transform.SetParent(transform);
                        TileInfo tileInfo = go.GetComponent<TileInfo>();
                        tileInfo.Initialize(pos);

                        tileInfoList.Add(tileInfo);
                        tileAllCategoryList.Add(go);
                    }
                    // Set Tile Castle with Castle
                    else
                    {
                        Vector2 pos = new Vector2(
                            col * tileSize[0] - row * tileSize[0],
                            (col + row) * -tileSize[1] + tileSize[1] * 2);
                        GameObject go = Instantiate(
                            tileCastlePrefab,
                            pos,
                            Quaternion.identity);
                        go.transform.SetParent(transform);
                        TileCastle tileCastle = go.GetComponent<TileCastle>();
                        tileCastle.Initialize(pos);
                        _gameManager.baseTransform = tileCastle.gameObject.transform;

                        endTile = tileCastle;
                        tileAllCategoryList.Add(go);
                    }

                    baseNumber++;
                }
            }
        }
        else
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    // Set Tile Road
                    if (baseNumber != 12)
                    {
                        Vector2 pos = new Vector2(
                            col * tileSize[0] - row * tileSize[0],
                            (col + row) * -tileSize[1] + tileSize[1] * 2 + (tileSize[1] * mapExtendLevel));
                        GameObject go = Instantiate(
                            tileHardModePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length)],
                            pos,
                            Quaternion.identity);
                        go.transform.SetParent(transform);
                        TileInfo tileInfo = go.GetComponent<TileInfo>();
                        tileInfo.Initialize(pos);

                        tileInfoList.Add(tileInfo);
                        tileAllCategoryList.Add(go);
                    }
                    // Set Tile Castle with Castle
                    else
                    {
                        Vector2 pos = new Vector2(
                            col * tileSize[0] - row * tileSize[0],
                            (col + row) * -tileSize[1] + tileSize[1] * 2 + (tileSize[1] * mapExtendLevel));
                        GameObject go = Instantiate(
                            tileCastlePrefab,
                            pos,
                            Quaternion.identity);
                        go.transform.SetParent(transform);
                        TileCastle tileCastle = go.GetComponent<TileCastle>();
                        tileCastle.Initialize(pos);
                        _gameManager.baseTransform = tileCastle.gameObject.transform;

                        endTile = tileCastle;
                        tileAllCategoryList.Add(go);
                    }

                    baseNumber++;
                }
            }
        }

        
    }

    /// <summary>
    /// Set Spawner Tile
    /// 스포너 타일 지정
    /// </summary>
    private void SetSpawnerPosition()
    {
        spawnTransform = new Vector2();
        ClearSpawner();

        switch (mapExtendLevel)
        {
            case 1:
                spawnTransform = tileGridList[4].transform.position;
                break;
            case 2:
                spawnTransform = tileGridList[6].transform.position;
                break;
        }

        CreateSpawner();
    }

    /// <summary>
    /// Remove Spawner Tile
    /// 스포너 타일 제거
    /// </summary>
    private void ClearSpawner()
    {
        _gameManager.DestroyOfType<TileSpawner>();
    }

    /// <summary>
    /// Create Spawner Tile
    /// 스포너 타일 생성
    /// </summary>
    private void CreateSpawner()
    {
        if (spawnTransform != null)
        {
            Vector2 pos = spawnTransform;
            GameObject go = Instantiate(
                tileSpawnPrefab,
                pos,
                Quaternion.identity);
            go.transform.SetParent(transform);
            TileSpawner tileSpawner = go.GetComponent<TileSpawner>();
            tileSpawner.Initialize(pos);

            startTile = tileSpawner;
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Set Neignbor Tiles
    /// 이웃한 타일 정보 보기
    /// </summary>
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

    /// <summary>
    /// Show Connected Pathroute
    /// 연결된 길 보기
    /// </summary>
    public void ShowConnectedPath()
    {
        SetNeighbors();
        CloseTileUI(null);

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

            if (path != null && path.Count > 0)
            {
                SetPathfinder(path);
            }
            else
            {
                Debug.LogWarning("No length has been allocated for this array");
            }
        }
    }
    
    /// <summary>
    /// Find Conntected Path
    /// 연결된 길 찾기
    /// </summary>
    /// <param name="startTile">스포너 타일</param>
    /// <param name="endTile">성 타일</param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Set Pathfinder
    /// 패스파인더 설치
    /// </summary>
    public void SetPathfinder(List<TileData> path)
    {
        GameObject go = Instantiate
            (
                pathfinderPrefab, 
                startTile.transform
            );
        Pathfinder pathfinder = go.GetComponent<Pathfinder>();
        pathfinder.Initialize(path);

        List<Transform> pathTrans = new List<Transform>();
        foreach (var tile in path)
        {
            pathTrans.Add(tile.transform);
        }

        _gameManager._waveManager.SetPath(pathTrans);
        _gameManager._hudCanvas.TurnOnStartWave();
    }

    /// <summary>
    /// Close Tile UI
    /// 타일에 게시된 UI 모두 닫기
    /// </summary>
    /// <param name="exceptUI">선택된 TileUI 제외</param>
    public void CloseTileUI(TileUI exceptUI)
    {
        foreach (var tileInfo in tileInfoList)
        {
            if (tileInfo._tileUI != exceptUI)
                tileInfo._tileUI.CloseUI();
        }
    }

    /// <summary>
    /// Close Tower Installer UI
    /// 타워 설치 UI 닫기
    /// </summary>
    public void CloseTowerInstallUI()
    {
        towerUIdnjswls.Hide();
    }

    /// <summary>
    /// Close Tower Info UI
    /// 타워 정보 UI 닫기
    /// </summary>
    public void CloseTowerInfoUI()
    {
        TowerSellUI.Instance.Hide();
    }
}
