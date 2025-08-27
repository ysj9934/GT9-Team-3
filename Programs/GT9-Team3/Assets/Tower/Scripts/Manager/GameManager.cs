using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEditor.EditorTools;
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

    // Canvas
    public HUDCanvas _hudCanvas;


    public Transform baseTransform;

    // 게임 레벨
    // GameLevel
    public int gameWorldLevel;
    public int gameStageLevel;
    public int gameRoundLevel;
    public int gameWaveLevel;
    public List<Wave_DataTable> stageWaveIdList = new List<Wave_DataTable>();
    public int tempLevel;

    // 게임 일시정지 및 재개
    public bool isGamePaused = false;

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
        _poolManager = ObjectPoolManager.Instance;
        _dataManager = DataManager.Instance;
        _resourceManager = ResourceManager.Instance;

        // Initialize Canvas
        _hudCanvas = HUDCanvas.Instance;

        if (IsValidate())
        {
            // Load Data from DataManager
            ReceiveStageData();
        }

        // 추후 삭제 필요
        InitializeTiles();
    }

    private bool IsValidate()
    {
        if (_tileManager == null)
        {
            ValidateMessage(_tileManager.name);
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
    private void ReceiveStageData()
    {
        StageData stageData = _dataManager.SendStageData();
        if (stageData != null)
        {
            // 0. GameManager에 스테이지 정보 갱신
            // update stage information in GameManager
            gameWorldLevel = stageData.worldCode;
            gameStageLevel = stageData.stageCode;

            // send stage data to HUDCanvas

            // 1. HUDCanvas에 스테이지 정보 전달
            SendStageDataToHUD();
            // 2. TileManager에 스테이지 정보 전달
            // 3. WaveManager에 스테이지 정보 전달

            // 4. TileManager 세팅
            //InitializeTiles();
        }
        else 
        {
            Debug.LogError("StageData is Null");
        }
    }


    // 초기 타일 배치
    // Initial tile placement
    public void InitializeTiles()
    {
        // 타일 생성
        _tileManager.Initialize();
        _tileManager.SetSpawnerPosition();
    }

    public void SendStageDataToHUD()
    {
        _hudCanvas.ReceiveStageData(
            new StageDataToHUD
                (
                    gameWorldLevel,
                    gameStageLevel,
                    gameRoundLevel,
                    gameWaveLevel
                )
            );
    }

    public StageDataToTileManager SendStageDataToTileManager()
    {
        return new StageDataToTileManager
            (
                gameWorldLevel,
                gameStageLevel,
                gameRoundLevel,
                gameWaveLevel
            );
    }

    public StageDataToWaveManager SendStageDataToWaveManager()
    {
        return new StageDataToWaveManager
            (
                stageWaveIdList
            );
    }



    public void UpdateWorldLevel(int level)
    {
        this.gameWorldLevel = level;
        _tileManager.UpdateWorldLevel(this.gameWorldLevel);
    }



    public void UpdateTempLevel(int level)
    {
        this.tempLevel = level;

        _tileManager.UpdateTempLevel(this.tempLevel);
    }

    public void WaveStartButton()
    {
        //WaveManager.Instance.StartWave();
    }

    // TEMP
    // 적 스폰
    public void SpanwEnemy(Enemy_DataTable_EnemyStatTable jsonData)
    {
        GameObject enemyObj = _poolManager.GetEnemy();
        if (enemyObj != null)
        {
            //var config = EnemyConfigManager.Instance.CreateConfigFromJson(jsonData);
            //enemyObj.GetComponent<Enemy>().Setup(config);
        }
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
}

public class StageDataToHUD
{
    public int worldCode;
    public int stageCode;
    public int roundCode;
    public int waveCode;

    public StageDataToHUD(int worldCode, int stageCode, int roundCode, int waveCode)
    {
        this.worldCode = worldCode;
        this.stageCode = stageCode;
        this.roundCode = roundCode;
        this.waveCode = waveCode;
    }
}

public class StageDataToTileManager
{
    public int worldCode;
    public int stageCode;
    public int roundCode;
    public int waveCode;

    public StageDataToTileManager(int worldCode, int stageCode, int roundCode, int waveCode)
    {
        this.worldCode = worldCode;
        this.stageCode = stageCode;
        this.roundCode = roundCode;
        this.waveCode = waveCode;
    }
}
public class StageDataToWaveManager
{
    public List<Wave_DataTable> stageWaveList;

    public StageDataToWaveManager(List<Wave_DataTable> stageWaveList)
    {
        this.stageWaveList = stageWaveList;
    }
}