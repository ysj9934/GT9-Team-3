using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private Enemy_DataTableLoader enemyDataLoader;
    public Enemy_DataTableLoader EnemyDataLoader
    {
        get { return enemyDataLoader; }
    }

    private Wave_DataTableLoader waveDataLoader;
    public Wave_DataTableLoader WaveDataLoader
    {
        get { return waveDataLoader; }
    }

    private EnemySkillListTableLoader enemySkillListTableLoader;
    public EnemySkillListTableLoader EnemySkillListTableLoader
    {
        get { return enemySkillListTableLoader; }
    }

    private EnemySkillTableLoader enemySkillTableLoader;
    public EnemySkillTableLoader EnemySkillTableLoader
    {
        get { return enemySkillTableLoader; }
    }

    // CommonScript
    private CommonScript commonScript;

    // Stage Setting
    public int stageId;
    public int worldCode;
    public int stageCode;
    public bool isBossMode = false;
    public List<Wave_DataTable> stageWaveIdList;

    public delegate void OnStageChanged();
    public event OnStageChanged StageChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Data
        enemyDataLoader = new Enemy_DataTableLoader();
        waveDataLoader = new Wave_DataTableLoader();
        enemySkillListTableLoader = new EnemySkillListTableLoader();
        enemySkillTableLoader = new EnemySkillTableLoader();

        // CommonScript
        commonScript = new CommonScript();

        // Test 
        SelectedStage(203);
    }

    /// <summary>
    /// 스테이지 선택
    /// </summary>
    public void SelectedStage(int stageId)
    {
        if (IsValidate(stageId))
        {
            this.stageId = stageId;
            this.worldCode = commonScript.getDigit(stageId, 3);
            this.stageCode = commonScript.getDigit(stageId, 1);

            if (worldCode == 4)
                isBossMode = true;
            else
                isBossMode = false;



            SetStageWaveList(this.stageId);
        }

        StageChanged?.Invoke();
    }

    /// <summary>
    /// 스테이지 재시작
    /// </summary>
    public void RestartStage(int stageId)
    {
        Debug.Log("selected Stage_ID: " + stageId);

        // 스테이지 데이터 초기화
        ClearStageData();

        if (IsValidate(stageId))
        {
            this.stageId = stageId;
            this.worldCode = commonScript.getDigit(stageId, 3);
            this.stageCode = commonScript.getDigit(stageId, 1);

            if (worldCode == 4)
                isBossMode = true;
            else
                isBossMode = false;



            SetStageWaveList(this.stageId);
        }

        // 스테이지 재시작 (GameManager)
        GameManager.Instance.ReStartStage();
    }

    private bool IsValidate(int stageId)
    {
        if (stageId < 100)
        {
            Debug.LogError("Invalid Stage_ID: " + stageId);
            return false;
        }

        if (WaveDataLoader == null)
        {
            Debug.LogError("WaveDataLoader is not initialized.");
            return false;
        }

        return true;
    }

    public StageData SendStageData()
    {
        return new StageData(
            0, 
            worldCode, 
            stageCode,
            0,
            0,
            stageWaveIdList, 
            isBossMode
            );
    }

    // 스테이지 데이터 초기화
    private void ClearStageData()
    {
        // DataManager 초기화
        stageId = 0;
        worldCode = 0;
        stageCode = 0;
        stageWaveIdList = new List<Wave_DataTable>();
        isBossMode = false;

        GameManager.Instance.DestroyOfType<Projectile>();
        GameManager.Instance._waveController.ReturnAllEnemies();
        TowerSellUI.Instance.Hide();
    }

    private void SetStageWaveList(int stageId)
    {
        stageWaveIdList = new List<Wave_DataTable>();

        foreach (var items in waveDataLoader.ItemsList)
        {
            if (items.Stage_ID == this.stageId)
            {
                stageWaveIdList.Add(items);
            }
        }

        if (stageWaveIdList.Count < 1 || stageWaveIdList == null)
        {
            Debug.LogError("Invalid Stage_ID in stageWaveIdList: " + stageId);
            return;
        }
    }

}

public class StageData
{
    public int stageId;
    public int worldCode;
    public int stageCode;
    public int roundCode;
    public int waveCode;
    public List<Wave_DataTable> stageWaveList;
    public bool isHardMode;

    public StageData(int stageId, int worldCode, int stageCode, int roundCode, int waveCode, List<Wave_DataTable> stageWaveList, bool isHardMode)
    {
        this.stageId = stageId;
        this.worldCode = worldCode;
        this.stageCode = stageCode;
        this.roundCode = roundCode;
        this.waveCode = waveCode;
        this.stageWaveList = stageWaveList;
        this.isHardMode = isHardMode;
    }
}
