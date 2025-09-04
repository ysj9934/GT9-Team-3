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

    // Stage Setting
    public int stageId;
    public List<Wave_DataTable> stageWaveIdList;
    public int worldCode;
    public int stageCode;
    public bool isHardMode = false;

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

        enemyDataLoader = new Enemy_DataTableLoader();
        waveDataLoader = new Wave_DataTableLoader();
        enemySkillListTableLoader = new EnemySkillListTableLoader();
        enemySkillTableLoader = new EnemySkillTableLoader();

        // Test 
        SelectedStage(203);
    }

    /// <summary>
    /// title: selected Stage
    /// created by : yoons, heechun
    /// created at : 25.08.27
    /// </summary>
    public void SelectedStage(int stageId)
    {
        // valid check
        if (IsValidate(stageId))
        {
            if (stageId == 401)
                isHardMode = true;
            else 
                isHardMode = false;

            this.stageId = stageId;
            SetStageWaveList(this.stageId);
            this.worldCode = stageId / 100;
            this.stageCode = stageId % 10;
        }
        
        Debug.Log("selected Stage_ID: " + stageId);
        //if (GameManager.Instance != null)
        //    GameManager.Instance.ResumeGame();
    }

    public void RestartStage(int stageId)
    {
        // initialize previous stage data
        ClearStageData();

        // valid check
        if (IsValidate(stageId))
        {
            this.stageId = stageId;
            SetStageWaveList(this.stageId);
            this.worldCode = stageId / 100;
            this.stageCode = stageId % 10;
        }

        Debug.Log("selected Stage_ID: " + stageId);

        GameManager.Instance.ReStartStage();
    }

    public StageData SendStageData()
    {
        return new StageData(stageId, worldCode, stageCode, stageWaveIdList, isHardMode);
    }

    private void ClearStageData()
    {
        stageId = 0;
        stageWaveIdList = new List<Wave_DataTable>();
        worldCode = 0;
        stageCode = 0;

        GameManager.Instance.DestroyOfType<Projectile>();
        WaveManager.Instance.ReturnAllEnemies();
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
}

public class StageData
{
    public int stageId;
    public List<Wave_DataTable> stageWaveList;
    public int worldCode;
    public int stageCode;
    public int roundCode;
    public int waveCode;
    public bool isHardMode;

    // DataManager To GameManager
    public StageData(int stageId, int worldCode, int stageCode, List<Wave_DataTable> stageWaveList, bool isHardMode)
    {
        this.stageId = stageId;
        this.worldCode = worldCode;
        this.stageCode = stageCode;
        this.stageWaveList = stageWaveList;
        this.isHardMode = isHardMode;
    }

    // GameManager To HUD
    public StageData(int worldCode, int stageCode, int roundCode, int waveCode, bool isHardMode)
    {
        this.worldCode = worldCode;
        this.stageCode = stageCode;
        this.roundCode = roundCode;
        this.waveCode = waveCode;
        this.isHardMode = isHardMode;
    }

    // GameManager To TileManager
    public StageData(int worldCode)
    {

    }

    // GameManager To WaveManager
    public StageData(List<Wave_DataTable> stageWaveList, bool isHardMode)
    {
        this.stageWaveList = stageWaveList;
        this.isHardMode = isHardMode;
    }

    public StageData(int waveCode, int roundCode)
    {
        this.waveCode = waveCode;
        this.roundCode = roundCode;
    }
}
