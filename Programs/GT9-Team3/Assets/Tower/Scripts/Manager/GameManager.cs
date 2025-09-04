using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Manager
    public TileManager _tileManager;
    public WaveManager _waveManager;
    public ObjectPoolManager _poolManager;
    public DataManager _dataManager;
    public ResourceManager _resourceManager;

    // MapUICanvas
    public HUDCanvas _hudCanvas;

    public Transform baseTransform;

    // 게임 레벨
    // GameLevel
    public int gameWorldLevel;
    public int gameStageLevel;
    public int gameRoundLevel;
    public int gameWaveLevel;
    public List<Wave_DataTable> stageWaveList = new List<Wave_DataTable>();

    // 게임 일시정지 및 재개
    public bool isGamePaused = false;
    public bool isHardMode;
    public bool isGameOver = false;

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
        // Initialize Manager
        _tileManager = TileManager.Instance;
        _waveManager = WaveManager.Instance;
        _poolManager = ObjectPoolManager.Instance;
        _dataManager = DataManager.Instance;
        _resourceManager = ResourceManager.Instance;

        // Initialize MapUICanvas
        _hudCanvas = HUDCanvas.Instance;

        if (IsValidate())
        {
            // Load Data from DataManager
            ReceiveStageData();
        }
    }

    private bool IsValidate()
    {
        if (_tileManager == null)
        {
            ValidateMessage(_tileManager.name);
            return false;
        }
        else if (_waveManager == null)
        {
            ValidateMessage(_waveManager.name);
            return false;
        }
        else if (_poolManager == null)
        {
            ValidateMessage(_poolManager.name);
            return false;
        }
        else if (_dataManager == null)
        {
            ValidateMessage(_dataManager.name);
            return false;
        }
        else if (_resourceManager == null)
        {
            ValidateMessage(_resourceManager.name);
            return false;
        }
        else if (_hudCanvas == null)
        {
            ValidateMessage(_hudCanvas.name);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ValidateMessage(string obj)
    {
        Debug.LogError($"{obj} is Valid");
    }


    public void DestroyOfType<T>() where T : Component
    {
        T[] objs = GameObject.FindObjectsOfType<T>();

        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }
    }

    // Load data from DataManager
    public void ReceiveStageData()
    {
        StageData stageData = _dataManager.SendStageData();
        if (stageData != null)
        {
            // 0. GameManager에 스테이지 정보 갱신
            // update stage information in GameManager
            gameWorldLevel = stageData.worldCode;
            gameStageLevel = stageData.stageCode;
            stageWaveList = stageData.stageWaveList;
            isHardMode = stageData.isHardMode;

            // send stage data to HUDCanvas
            // 3. WaveManager에 스테이지 정보 전달
            SendStageDataToWaveManager();
            // 2. TileManager에 스테이지 정보 전달
            SendStageDataToTileManager();
            // 1. HUDCanvas에 스테이지 정보 전달
            SendStageDataToHUD();

            // 4. TileManager 세팅
            //_tileManager.Initialize();
        }
        else 
        {
            Debug.LogError("StageData is Null");
        }
    }

    public void ReStartStage()
    {
        // 이전 데이터 초기화하기 
        ClearGameManager();
        _hudCanvas.SetGameSpeed5x();

        ReceiveStageData();
    }

    public void ClearGameManager()
    {
        gameWorldLevel = 0;
        gameStageLevel = 0;
        gameRoundLevel = 0;
        gameWaveLevel = 0;
        stageWaveList = new List<Wave_DataTable>();
    }

    public void SendStageDataToHUD()
    {
        _hudCanvas._hudStageInfo.ReceiveStageData(
            new StageData
                (
                    gameWorldLevel,
                    gameStageLevel,
                    gameRoundLevel,
                    gameWaveLevel,
                    isHardMode
                )
            );
    }

    public void SendStageDataToTileManager()
    {
        _tileManager.ReceiveStageData
            (
                new StageData
                (
                    gameWorldLevel,
                    gameStageLevel,
                    gameRoundLevel,
                    gameWaveLevel,
                    isHardMode
                )
            );
    }

    public void SendStageDataToWaveManager()
    {
        _waveManager.ReceiveStageData(
            new StageData
            (
                stageWaveList,
                isHardMode
            )
        );
        
    }

    public void ReceiveStageDataFromWaveManager(StageData stageData)
    {
        if (stageData != null)
        {
            gameWaveLevel = stageData.waveCode;
            gameRoundLevel = stageData.roundCode;
            SendStageDataToHUD();

            if (gameWaveLevel == 7)
            {
                Debug.Log("MapExtend 01");
                MapExtend();
            }
        }
        else
        {
            Debug.LogError("StageData is Null");
        }
    }

    public void MapExtend()
    {
        SendStageDataToTileManager();
    }

    /// <summary>
    /// Pause Game
    /// 게임 정지
    /// </summary>
    public void PauseGame()
    {
        Debug.Log("PauseGame");
        Time.timeScale = 0;
        isGamePaused = true;
    }

    /// <summary>
    /// Resume Game
    /// 게임 재개
    /// </summary>
    public void ResumeGame()
    {
        Debug.Log("ResumeGame");
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void GameSpeed2x()
    {
        Time.timeScale = 2f;
        isGamePaused = false;
    }

    public void GameSpeed5x()
    {
        Time.timeScale = 5f;
        isGamePaused = false;
    }


#if UNITY_EDITOR
    [ContextMenu("Spawn Test Enemy")]
    void SpawnTestEnemy()
    {
        GameObject enemyObj = _poolManager.GetEnemy();
        if (enemyObj != null)
        {
            var testJson = new Enemy_DataTable_EnemyStatTable
            {
                key = 1000,
                Enemy_Inner_Name = "기어다니는 굼벵이",
                MaxHP = 100,
                MovementSpeed = 3.5f
            };

            //var config = EnemyConfigManager.Instance.CreateConfigFromJson(testJson);
            //var enemy = Instantiate(config.enemyPrefab);
            //enemyObj.GetComponent<Enemy>().Setup(config);
        }
    }
#endif



    // 적 베이스 찾기 (김원진)
    // Find the enemy base transform in the scene
    public Vector3 BasePosition => baseTransform != null ? baseTransform.position : Vector3.zero;

    public void Earn50G()
    {
        _resourceManager.Earn(ResourceType.Tilepiece, 50);
        _hudCanvas._hudResource.ShowTilePiece();
    }

    public void Earn0G(int amount)
    {
        _resourceManager.Earn(ResourceType.Tilepiece, amount);
        _hudCanvas._hudResource.ShowTilePiece();

    }

}

