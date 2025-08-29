using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    // Managers
    private GameManager _gameManager;
    public ObjectPoolManager _poolManager;

    public List<Transform> path = new List<Transform>();
    public List<Wave_DataTable> stageWaveList = new List<Wave_DataTable>();
    public List<GameObject> activeEnemies = new List<GameObject>();

    // Wave Info & check
    private int currentWaveLevel;
    private int currentRoundLevel;
    public int waveIndex;
    private bool isWaveReady = false;
    private bool isWaveRoutine = false;
    public int rewardGold = 0;

    // Wave Data
    private List<float> spawnStartTime = new List<float>();
    private List<int> enemyId = new List<int>();
    private List<int> spawnBatchSize = new List<int>();
    private List<int> spawnRepeat = new List<int>();
    private List<float> spawnintervalSec = new List<float>();

    public int aliveEnemyCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _poolManager = ObjectPoolManager.Instance;

        if (IsValidate())
        {
            isWaveReady = false;
            isWaveRoutine = false;
        }

    }

    private bool IsValidate()
    {
        if (_gameManager == null)
        {
            ValidateMessage(_gameManager.name);
            return false;
        }
        else if (_poolManager == null)
        {
            ValidateMessage(_poolManager.name);
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

    public void SetPath(List<Transform> path)
    {
        this.path = path;
    }

    /// <summary>
    /// Wave Setting
    /// </summary>
    public void ReceiveStageData(StageData stageData)
    {
        if (stageData != null)
        {
            this.stageWaveList = stageData.stageWaveList;
            ResourceManager.Instance.resources[ResourceType.Tilepiece] = stageWaveList[0].StageStartTilePiece;
            HUDCanvas.Instance.ShowTilePiece();
            this.rewardGold = 0;
            SetWaveSystem(stageWaveList[0]);
        }
        else
        {
            Debug.LogError("StageData is Null");
        }
    }

    public void SetWaveSystem(Wave_DataTable stageData)
    {
        if (stageData == null)
        {
            Debug.LogError("WaveDataLoader is not initialized.");
        }

        waveIndex = stageData.key % 10;

        UpdateWaveLevel(waveIndex);

        this.spawnStartTime = stageData.SpawnStartTime;
        this.enemyId = stageData.EnemyID;
        this.spawnBatchSize = stageData.SpawnBatchSize;
        this.spawnRepeat = stageData.SpawnRepeat;
        this.spawnintervalSec = stageData.SpawnintervalSec;
        this.rewardGold += stageData.RewardGoldAmount;
    }

    // send wave and round data
    public void UpdateWaveLevel(int waveNum)
    {
        this.currentWaveLevel = waveNum;
        this.currentRoundLevel = ((waveNum - 1) / 3) + 1;

        SendWaveData();

        if (waveNum % 3 == 1)
        {
            _gameManager._hudCanvas.TurnOnPathfinder();
            _gameManager._hudCanvas.TurnOffStartWave();
            _gameManager._tileManager.isUIActive = true;
            _gameManager._tileManager.isMoveActive = true;

            path.Clear();
        }
        else
        {
            _gameManager._hudCanvas.TurnOnStartWave();
            _gameManager._tileManager.isUIActive = false;
            _gameManager._tileManager.isMoveActive = false;

            
        }

        
    }

    public void SendWaveData()
    {
        _gameManager.ReceiveStageDataFromWaveManager(
            new StageData
            (
                currentWaveLevel,
                currentRoundLevel
            )
        );
        
    }

    
    public Coroutine waveRoutine;
    public List<Coroutine> enemySpawnRoutines;

    /// <summary>
    /// start wave
    /// </summary>
    public void StartWave()
    {
        if (path.Count < 1 || path[0] == null)
        {
            Debug.LogError("path is valid");
            return;
        }
        
        // 1. wave start 버튼을 누르기
        if (!isWaveRoutine)
        {
            // wave start 버튼 켜기
            aliveEnemyCount = 0;
            // 2. TileUI (회전 불가하도록 수정)
            _gameManager._tileManager.isUIActive = true;
            _gameManager._tileManager.isMoveActive = true;
            // 3. 웨이브 스폰 시키기
            waveRoutine = StartCoroutine(AwakeWave());
            // 3-1. 웨이브 패배시 Wave재시작 할 수 있도록..
            // 4. 웨이브 종료시 다음 웨이브 Setting
            // 5. Round가 커질 시 tileUI회전 가능하도록
        }
        else
        {
            // wave시작 버튼 끄기
            _gameManager._hudCanvas.TurnOffStartWave();
            _gameManager._tileManager.isUIActive = false;
            _gameManager._tileManager.isMoveActive = false;
        }
    }

    /// <summary>
    /// Game Defeat
    /// 게임 패배후 초기화
    /// </summary>
    public void StopWave()
    { 
        if (waveRoutine != null)
        {
            StopCoroutine(waveRoutine);
            waveRoutine = null;
            isWaveRoutine = false;
        }

        if (enemySpawnRoutines.Count > 0)
        {
            foreach (var r in enemySpawnRoutines)
            { 
                StopCoroutine(r);
            }
            enemySpawnRoutines.Clear();
        }

        path.Clear();
        aliveEnemyCount = 0;
    }

    public void ReturnAllEnemies()
    {
        foreach (var enemy in activeEnemies)
        {
            enemy.GetComponent<EnemyHealthHandler>().OnDeath -= HandleEnemyDeath;
            _poolManager.ReturnEnemy(enemy);
        }
        activeEnemies.Clear();
    }

    IEnumerator AwakeWave()
    {
        int index = 0;
        isWaveRoutine = true;
        _gameManager._tileManager.isUIActive = false;
        _gameManager._tileManager.isMoveActive = false;
        _gameManager._hudCanvas.TurnOffStartWave();
        enemySpawnRoutines = new List<Coroutine>();

        while (spawnStartTime[index] > -1)
        {
            float delay = index == 0 ? spawnStartTime[0] : spawnStartTime[index] - spawnStartTime[index - 1]; 
            yield return new WaitForSeconds(delay);
            
            for (int j = 0; j < spawnRepeat[index]; j++)
            {
                Coroutine spawnroutine = StartCoroutine(SpawnEnemyWithDelay(j * spawnintervalSec[index], enemyId[index]));    
                enemySpawnRoutines.Add(spawnroutine);
            }
            
            index++;
        }

        waveRoutine = null;
    }

    // 적 유닛 생성 시스템
    IEnumerator SpawnEnemyWithDelay(float spawnTime, int monsterID)
    {
        yield return new WaitForSeconds(spawnTime);

        if (!isWaveRoutine) yield break;

        var config = EnemyConfigManager.Instance.GetConfig(monsterID);
        if (config == null)
        {
            Debug.LogError($"EnemyConfig 생성 실패: monsterID {monsterID}");
            yield break;
        }

        SpawnEnemy(config);
    }

    

    public void SpawnEnemy(EnemyConfig config)
    {
        GameObject enemyObj = _poolManager.GetEnemy();
        if (enemyObj != null)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy._enemyStat.Setup(config);
            EnemyEnhanced(enemy);
            enemy._enemyMovement.pathPoint(path);

            aliveEnemyCount++;
            enemy._enemyHealthHandler.OnDeath += HandleEnemyDeath;

            activeEnemies.Add(enemyObj);
        }
    }

    private void EnemyEnhanced(Enemy enemy)
    {
        enemy._enemyStat.enemyMaxHP *= 1.01f * waveIndex;
        enemy._enemyHealthHandler.InitializeHealth();
    }

    private void HandleEnemyDeath()
    { 
        aliveEnemyCount--;

        if (aliveEnemyCount <= 0 && waveRoutine == null)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        isWaveRoutine = false;

        Debug.Log("WaveEnd");

        if (waveIndex < 9)
        {
            SetWaveSystem(stageWaveList[waveIndex]);
        }
        else
        {
            // HUD에서 승리 panel
            Debug.Log("Victory");

            _gameManager.PauseGame();
            HUDCanvas.Instance._gameResultPanel.OpenWindow(true);
        }   
    }

    /// <summary>
    /// Test
    /// </summary>

    public void SpawnWaves(int waveID)
    {
        WaveSystem(waveID);

        StartCoroutine(AwakeWave());
    }

    // Test WaveSystem UI에 연동되어 있음.
    public void WaveSystem(int waveID)
    {
        Debug.Log($"WaveSystem 버튼 연동으로 들어왔습니다. : {waveID}");
        var jsonData = _gameManager._dataManager.WaveDataLoader.GetByKey(waveID);

        if (jsonData == null)
        {
            Debug.LogError($"웨이브 ID {waveID}에 대한 JSON 데이터 없음");
        }
        
        this.spawnStartTime = jsonData.SpawnStartTime;
        this.enemyId = jsonData.EnemyID;
        this.spawnBatchSize = jsonData.SpawnBatchSize;
        this.spawnRepeat = jsonData.SpawnRepeat;
        this.spawnintervalSec = jsonData.SpawnintervalSec;
    }
}
