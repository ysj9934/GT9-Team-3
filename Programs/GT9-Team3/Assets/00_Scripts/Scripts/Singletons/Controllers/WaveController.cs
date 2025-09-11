using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    // Object Manager
    private GameManager _gameManager;

    // CommonScript
    private CommonScript commonScript;

    public List<Transform> path = new List<Transform>();
    public List<Wave_DataTable> stageWaveList = new List<Wave_DataTable>();
    public List<GameObject> activeEnemies = new List<GameObject>();

    // Wave Info & check
    private int currentWaveLevel;
    private int currentRoundLevel;
    public int waveIndex;
    private bool isWaveRoutine = false;
    public int rewardGold = 0;
    public bool isHardMode;

    // Wave Data
    private List<float> spawnStartTime = new List<float>();
    private List<int> enemyId = new List<int>();
    private List<int> spawnRepeat = new List<int>();
    private List<float> spawnintervalSec = new List<float>();

    public int aliveEnemyCount = 0;

    private void Awake()
    {
        _gameManager = GetComponentInParent<GameManager>();

        commonScript = new CommonScript();
    }

    // ==================== 웨이브 세팅 ==================== // 

    // [미확인]
    public void SetPath(List<Transform> path)
    {
        this.path = path;
    }

    /// <summary>
    /// 스테이지 정보 습득
    /// </summary>
    public void ReceiveStageData(StageData stageData)
    {
        if (stageData != null)
        {
            this.stageWaveList  = stageData.stageWaveList;
            this.isHardMode     = stageData.isHardMode;

            SetWaveData(stageWaveList[0]);

            ResourceManager.Instance.resources[ResourceType.Tilepiece] = stageWaveList[0].StageStartTilePiece;
            HUDCanvas.Instance._hudResource.ShowTilePiece();
            this.rewardGold = 0;
        }
        else
        {
            Debug.LogError("StageData is Null");
        }
    }


    /// <summary>
    /// 웨이브 정보 세팅
    /// </summary>
    public void SetWaveData(Wave_DataTable stageData)
    {
        if (_gameManager.isGameOver) return;

        if (stageData != null)
        {
            waveIndex = commonScript.getDigit(stageData.key, 1);

            this.spawnStartTime = stageData.SpawnStartTime;
            this.enemyId = stageData.EnemyID;
            this.spawnRepeat = stageData.SpawnRepeat;
            this.spawnintervalSec = stageData.SpawnintervalSec;
            this.rewardGold += stageData.RewardGoldAmount;

            UpdateWaveLevel(waveIndex);

            // 타일 기믹 적용
            ActivateWorldEffect(stageData.key);



            // UI 갱신
            // 남은 웨이브 유닛 초기화
            int enemySpawnCount = 0;
            foreach (int enemyCount in spawnRepeat)
            {
                if (enemyCount > 0)
                    enemySpawnCount += enemyCount;
            }

            aliveEnemyCount = 0;
            HUDCanvas.Instance._hudWaveInfo.ResetWavePoint(enemySpawnCount);
            HUDCanvas.Instance._hudWaveInfo.ResetEnemyCount(enemySpawnCount);

            // [사운드효과]: 다음 웨이브로 이동 사운드
            SoundManager.Instance.Play("medieval-opener-270568", SoundType.BGM, 0.05f);

            Debug.LogWarning("[Sound]: Next Wave Sound");
        }
        else
        {
            Debug.LogError("WaveDataLoader is not initialized.");
        }
    }

    // send wave and round data
    public void UpdateWaveLevel(int waveIndex)
    {
        this.currentWaveLevel = waveIndex;
        this.currentRoundLevel = ((waveIndex - 1) / 3) + 1;

        SendWaveData();

        // 라운드 변경
        if (waveIndex % 3 == 1)
        {
            // 1. 길찾기 지우기
            path.Clear();
            // 2. 타일 획득
            GetTileEndRound();
            // 3. 타일 움직이기 가능
            _gameManager._tileController.isUIActive = true;
            _gameManager._tileController.isMoveActive = true;
            // 4. UI 버튼
            HUDCanvas.Instance.TurnOnPathfinder();
            HUDCanvas.Instance.TurnOffStartWave();

            // Wave Progress UI 초기화
            HUDCanvas.Instance._hudWaveInfo.ResetAllWavePoint();

            // 5. 플로팅 메세지
            if (waveIndex != 1)
            {
                HUDCanvas.Instance._hudMessageUI.FloatingUIShow(
                    $"[{this.currentRoundLevel} 라운드 종료]",
                    "라운드가 종료되어 타일을 획득하였습니다.\n" +
                    "길찾기가 초기화되고, 이제 타일을 움직일 수 있습니다.",
                    Color.white
                    );
            }
            
        }
        else
        {
            _gameManager._tileController.isUIActive = false;
            _gameManager._tileController.isMoveActive = false;

            HUDCanvas.Instance.TurnOnStartWave();
        }
    }

    public void SendWaveData()
    {
        _gameManager.ReceiveStageDataFromWaveManager(
            new StageData
            (
                0,
                0,
                0,
                currentRoundLevel,
                currentWaveLevel,
                null,
                isHardMode
            )
        );
        
    }


    // ==================== 웨이브 시작 ==================== // 
    public Coroutine waveRoutine;
    public List<Coroutine> enemySpawnRoutines;

    /// <summary>
    /// 웨이브 시작 버튼
    /// </summary>
    public void StartWave()
    {
        TileController _tileController = _gameManager._tileController;

        if (path.Count < 1 || path[0] == null)
        {
            Debug.LogError("path is valid");
            HUDCanvas.Instance._hudMessageUI.FloatingUIShow(
                "[오류]",
                "정상적인 길을 배정받지 못하였습니다. \n" +
                "패스파인더를 다시 확인해주세요.",
                Color.red
                );


            return;
        }
        
        // 1. wave start 버튼을 누르기
        if (!isWaveRoutine)
        {
            // wave start 버튼 켜기
            //aliveEnemyCount = 0;
            _tileController.isUIActive = true;
            _tileController.isMoveActive = true;
            // 3. 웨이브 스폰 시키기
            waveRoutine = StartCoroutine(AwakeWave());
        }
        else
        {
            // wave시작 버튼 끄기
            HUDCanvas.Instance.TurnOffStartWave();
            _tileController.isUIActive = false;
            _tileController.isMoveActive = false;
        }
    }

    /// <summary>
    /// 웨이브 시작 코루틴
    /// </summary>
    IEnumerator AwakeWave()
    {
        TileController _tileController = _gameManager._tileController;

        int index = 0;
        isWaveRoutine = true;

        _tileController.isUIActive = false;
        _tileController.isMoveActive = false;
        HUDCanvas.Instance.TurnOffStartWave();

        // [사운드효과]: 웨이브 시작
        SoundManager.Instance.Play("fallen-in-battle-261253 (2)", SoundType.BGM, 0.2f);
        Debug.LogWarning("[Sound] Wave Start Sound");

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

    /// <summary>
    /// 적 유닛 생성 코루틴
    /// </summary>
    IEnumerator SpawnEnemyWithDelay(float spawnTime, int monsterID)
    {
        yield return new WaitForSeconds(spawnTime);

        if (!isWaveRoutine) yield break;

        var config = EnemyManager.Instance.enemyConfigController.GetConfig(monsterID);
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
        GameObject enemyObj = EnemyManager.Instance.GetEnemy();
        if (enemyObj != null)
        {
            // 적 유닛 세팅
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy._enemyStat.Setup(config);
            EnemyEnhanced(enemy);
            enemy._enemyMovement.pathPoint(path);

            
            enemy._enemyHealthHandler.OnDeath += HandleEnemyDeath;
            aliveEnemyCount++;
            int index = waveIndex % 3 == 0 ? 3 : waveIndex % 3;
            HUDCanvas.Instance._hudWaveInfo.UpdateWaveCount(index);

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
        HUDCanvas.Instance._hudWaveInfo.UpdateEnemyCount();

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
                SetWaveData(stageWaveList[waveIndex]);
            }
            else
            {
                _gameManager.GameVictory();

                // HUD에서 승리 panel
                
            }
        }
        else
        {
            _gameManager.GameVictory();
        }
    }

    // ==================== 게임 초기화 ==================== //

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

        rewardGold -= stageWaveList[waveIndex - 1].RewardGoldAmount;
        path.Clear();

        // 유닛 제거
        ReturnAllEnemies();
        aliveEnemyCount = 0;
    }

    public void ReturnAllEnemies()
    {
        EnemyManager _enemyManager = EnemyManager.Instance;

        foreach (var enemy in activeEnemies)
        {
            enemy.GetComponent<EnemyHealthHandler>().OnDeath -= HandleEnemyDeath;
            _enemyManager.ReturnEnemy(enemy);
        }

        activeEnemies.Clear();
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
                    Color.white);

                break;
            // World 2
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
                   Color.white);

                break;
            // World 3 - 2
            case 30202:
            case 30302:
            case 30402:
            case 30502:
                // 타일 봉쇄 & 전장 개조 & 느린 타워
                numberOfLocks = 1;
                while (numberOfLocks > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(1, path.Count - 1);
                    TileInfo tileInfo = path[randomIndex].gameObject.GetComponent<TileInfo>();
                    if (!tileInfo.isTileLocked)
                    {
                        tileInfo.WorldTileGimmic(true, true, true);
                        numberOfLocks--;
                    }
                }

                HUDCanvas.Instance._hudMessageUI.FloatingUIShow(
                   "[분노]",
                   "[월드보스]죽음의 심판자가 `타일봉쇄`, `전장개조`를 사용하였습니다.\n" +
                   "`타일봉쇄`를 당한 타일은 해당 스테이지동안 회전하거나 옮길 수 없습니다.\n" +
                   "`전장개조`를 당한 타일 위에서 적의 이동속도가 증가합니다. \n" +
                   "`느린타워`를 당한 타워는 공격속도가 감소합니다.",
                   Color.white);
                break;
            // World 3 - 3
            case 30205:
            case 30305:
            case 30405:
            case 30505:
                // 타일 봉쇄 & 전장 개조 & 느린 타워
                numberOfLocks = 2;
                while (numberOfLocks > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(1, path.Count - 1);
                    TileInfo tileInfo = path[randomIndex].gameObject.GetComponent<TileInfo>();
                    if (!tileInfo.isTileLocked)
                    {
                        tileInfo.WorldTileGimmic(true, true, true);
                        numberOfLocks--;
                    }
                }

                HUDCanvas.Instance._hudMessageUI.FloatingUIShow(
                   "[분노]",
                   "[월드보스]죽음의 심판자가 `타일봉쇄`, `전장개조`, `느린 타워`를 사용하였습니다.\n" +
                   "`타일봉쇄`를 당한 타일은 해당 스테이지동안 회전하거나 옮길 수 없습니다.\n" +
                   "`전장개조`를 당한 타일 위에서 적의 이동속도가 증가합니다.\n" +
                   "`느린타워`를 당한 타워는 공격속도가 감소합니다.",
                   Color.white);
                break;

        }
    }

    // 라운드 시작시 타일 획득
    private void GetTileEndRound()
    {
        ShopController _shopController = _gameManager._shopController;

        // 타일 1 ~ 3 개 획득 배율은 25 50 25
        float[] weights = { 0.25f, 0.50f, 0.25f };

        int tileCount = CalculateRandomIndex(weights) + 1;

        while (tileCount > 0)
        {
            int tileIndex = UnityEngine.Random.Range(0, 4);

            switch (tileIndex)
            {
                case 0:
                    _shopController.CreateCrossTile();
                    break;
                case 1:
                   _shopController.CreateStraightTile();
                    break;
                case 2:
                    _shopController.CreateTShapeTile();
                    break;
                case 3:
                    _shopController.CreateCrossTile();
                    break;
            }

            tileCount--;
        }


        if (waveIndex != 1)
        {
            HUDCanvas.Instance._hudMessageUI.FloatingUIShow(
                "[습득]",
                "타일을 습득하셨습니다.",
                Color.white
                );
        }
        
    }

    private int CalculateRandomIndex(float[] weights)
    {
        float total = weights.Sum();
        float randomValue = UnityEngine.Random.Range(0, total);

        int selectedIndex = 0;
        float cumulative = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (randomValue <= cumulative)
            {
                selectedIndex = i;
                break;
            }
        }

        return selectedIndex;
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
        DataManager _dataManager= DataManager.Instance;

        Debug.Log($"WaveSystem 버튼 연동으로 들어왔습니다. : {waveID}");
        var jsonData = _dataManager.WaveDataLoader.GetByKey(waveID);

        if (jsonData == null)
        {
            Debug.LogError($"웨이브 ID {waveID}에 대한 JSON 데이터 없음");
        }
        
        this.spawnStartTime = jsonData.SpawnStartTime;
        this.enemyId = jsonData.EnemyID;
        this.spawnRepeat = jsonData.SpawnRepeat;
        this.spawnintervalSec = jsonData.SpawnintervalSec;

    }
}
