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

    // Stage Setting
    public int stageId;
    public List<Wave_DataTable> stageWaveIdList;
    public int worldCode;
    public int stageCode;

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
    }


    /// <summary>
    /// title: selected Stage
    /// created by : yoons, heechun
    /// created at : 25.08.27
    /// </summary>

    public void SelectedStage(int stageId)
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
    }

    public StageData SendStageData()
    {
        return new StageData(stageId, worldCode, stageCode, stageWaveIdList);
    }

    private void ClearStageData()
    {
        stageId = 0;
        stageWaveIdList = new List<Wave_DataTable>();
        worldCode = 0;
        stageCode = 0;
    }

    private void SetStageWaveList(int stageId)
    {
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
    public List<Wave_DataTable> stageWaveIdList;
    public int worldCode;
    public int stageCode;

    public StageData(int stageId, int worldCode, int stageCode, List<Wave_DataTable> stageWaveIdList)
    {
        this.stageId = stageId;
        this.worldCode = worldCode;
        this.stageCode = stageCode;
        this.stageWaveIdList = stageWaveIdList;
    }
}
