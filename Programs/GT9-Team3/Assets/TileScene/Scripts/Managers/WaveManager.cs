using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

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
    public bool isHardMode;

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
            this.isHardMode = stageData.isHardMode;
            ResourceManager.Instance.resources[ResourceType.Tilepiece] = stageWaveList[0].StageStartTilePiece;
            HUDCanvas.Instance._hudResource.ShowTilePiece();
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

        // 타일 기믹 적용
        ActivateWorldEffect(stageData.key);

        // UI 갱신
        _gameManager._hudCanvas._hudWaveInfo.ResetEnemyCount();
    }

    // send wave and round data
    public void UpdateWaveLevel(int waveNum)
    {
        this.currentWaveLevel = waveNum;
        this.currentRoundLevel = ((waveNum - 1) / 3) + 1;

        SendWaveData();

        // 라운드 변경
        if (waveNum % 3 == 1)
        {
            _gameManager._hudCanvas.TurnOnPathfinder();
            _gameManager._hudCanvas.TurnOffStartWave();
            _gameManager._tileManager.isUIActive = true;
            _gameManager._tileManager.isMoveActive = true;

            path.Clear();
            // Wave Progress UI 초기화
            _gameManager._hudCanvas._hudWaveInfo.ResetWavePoint();
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
        _gameManager = GameManager.Instance;

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
        rewardGold -= stageWaveList[waveIndex - 1].RewardGoldAmount;

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

            if (index < 7)
                index++;
            else
                break;
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

    
    /// <summary>
    /// 적 유닛 생성
    /// </summary>
    public void SpawnEnemy(EnemyConfig config)
    {
        GameObject enemyObj = _poolManager.GetEnemy();
        if (enemyObj != null)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy._enemyStat.Setup(config);
            EnemyEnhanced(enemy);
            enemy._enemyMovement.pathPoint(path);

            // 남은 적 유닛 수 체크
            aliveEnemyCount++;
            enemy._enemyHealthHandler.OnDeath += HandleEnemyDeath;
            _gameManager._hudCanvas._hudWaveInfo.UpdateWaveCount();

            activeEnemies.Add(enemyObj);
        }
    }

    private void EnemyEnhanced(Enemy enemy)
    {
        enemy._enemyStat.enemyMaxHP *= Mathf.Pow(1.02f, (float) waveIndex);
        enemy._enemyHealthHandler.InitializeHealth();
    }

    private void HandleEnemyDeath()
    { 
        aliveEnemyCount--;
        // UI 업데이트
        _gameManager._hudCanvas._hudWaveInfo.UpdateEnemyCount();

        if (aliveEnemyCount <= 0 && waveRoutine == null)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        isWaveRoutine = false;
        aliveEnemyCount = 0;

        if (!isHardMode)
        {
            if (waveIndex < 9)
            {
                SetWaveSystem(stageWaveList[waveIndex]);
            }
            else
            {
                // HUD에서 승리 panel
                Debug.Log("Victory");

                _gameManager.PauseGame();
                HUDCanvas.Instance._hudResultPanel._gameResultPanel.OpenWindow(true);
            }
        }
        else
        {
            Debug.Log("Victory");
            _gameManager.PauseGame();
            HUDCanvas.Instance._hudResultPanel._gameResultPanel.OpenWindow(true);
        }
    }

    private void ActivateWorldEffect(int key)
    {
        int numberOfLocks = 0;

        switch (key)
        {
            // World 1
            case 10402:
            case 10405:
            case 10502:
            case 10505:
                // 타일 봉쇄
                numberOfLocks = 1;
                while (numberOfLocks > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(1, path.Count - 1);
                    TileInfo tileInfo = path[randomIndex].gameObject.GetComponent<TileInfo>();
                    if (!tileInfo.isTileLocked)
                    {
                        tileInfo.WorldTileGimmic(true, false, false);
                        numberOfLocks--;
                    }
                }

                HUDCanvas.Instance._hudMessageUI.FloatingUIShow(
                    "[분노]",
                    "[월드보스]쌍두 사냥개가 `타일봉쇄`를 사용하였습니다.\n" +
                    "`타일봉쇄`를 당한 타일은 해당 스테이지동안 회전하거나 옮길 수 없습니다.",
                    Color.white,
                    5);

                break;
            case 20302:
            case 20305:
            case 20402:
            case 20405:
            case 20502:
            case 20505:
                // 타일 봉쇄 & 전장 개조
                numberOfLocks = 1;
                while (numberOfLocks > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(1, path.Count - 1);
                    TileInfo tileInfo = path[randomIndex].gameObject.GetComponent<TileInfo>();
                    if (!tileInfo.isTileLocked)
                    {
                        tileInfo.WorldTileGimmic(true, true, false);
                        numberOfLocks--;
                    }
                }

                HUDCanvas.Instance._hudMessageUI.FloatingUIShow(
                   "[분노]",
                   "[월드보스]강철 괴수가 `타일봉쇄`, `전장개조`를 사용하였습니다.\n" +
                   "`타일봉쇄`를 당한 타일은 해당 스테이지동안 회전하거나 옮길 수 없습니다.\n" +
                   "`전장개조`를 당한 타일 위에서 적의 이동속도가 증가합니다. ",
                   Color.white,
                   5);

                break;
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
