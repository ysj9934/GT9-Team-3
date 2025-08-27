using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Wave_DataTable_WaveSpawnTable
{
    public int key;

    public int SpawnSquence_01;
    public float SpawnStartTime_01;
    public int SpawnerID_01;
    public int EnemyID_01;
    public int SpawnBatchSize_01;
    public int SpawnRepeat_01;
    public float SpawnintervalSec_01;

    public int SpawnSquence_02;
    public float SpawnStartTime_02;
    public int SpawnerID_02;
    public int EnemyID_02;
    public int SpawnBatchSize_02;
    public int SpawnRepeat_02;
    public float SpawnintervalSec_02;

    public int SpawnSquence_03;
    public float SpawnStartTime_03;
    public int SpawnerID_03;
    public int EnemyID_03;
    public int SpawnBatchSize_03;
    public int SpawnRepeat_03;
    public float SpawnintervalSec_03;

    public int SpawnSquence_04;
    public float SpawnStartTime_04;
    public int SpawnerID_04;
    public int EnemyID_04;
    public int SpawnBatchSize_04;
    public int SpawnRepeat_04;
    public float SpawnintervalSec_04;

    public int SpawnSquence_05;
    public float SpawnStartTime_05;
    public int SpawnerID_05;
    public int EnemyID_05;
    public int SpawnBatchSize_05;
    public int SpawnRepeat_05;
    public float SpawnintervalSec_05;

    public int GetSpawnSquence(int index) => index switch
    {
        1 => SpawnSquence_01,
        2 => SpawnSquence_02,
        3 => SpawnSquence_03,
        4 => SpawnSquence_04,
        5 => SpawnSquence_05,
        _ => -1
    };

    public float GetSpawnStartTime(int index) => index switch
    {
        1 => SpawnStartTime_01,
        2 => SpawnStartTime_02,
        3 => SpawnStartTime_03,
        4 => SpawnStartTime_04,
        5 => SpawnStartTime_05,
        _ => -1f
    };

    public int GetSpawnerID(int index) => index switch
    {
        1 => SpawnerID_01,
        2 => SpawnerID_02,
        3 => SpawnerID_03,
        4 => SpawnerID_04,
        5 => SpawnerID_05,
        _ => -1
    };

    public int GetEnemyID(int index)
    {
        return index switch
        {
            1 => EnemyID_01,
            2 => EnemyID_02,
            3 => EnemyID_03,
            4 => EnemyID_04,
            5 => EnemyID_05,
            _ => -1
        };
    }

    public int GetSpawnBatchSize(int index) => index switch
    {
        1 => SpawnBatchSize_01,
        2 => SpawnBatchSize_02,
        3 => SpawnBatchSize_03,
        4 => SpawnBatchSize_04,
        5 => SpawnBatchSize_05,
        _ => -1
    };

    public int GetSpawnRepeat(int index) => index switch
    {
        1 => SpawnRepeat_01,
        2 => SpawnRepeat_02,
        3 => SpawnRepeat_03,
        4 => SpawnRepeat_04,
        5 => SpawnRepeat_05,
        _ => -1
    };

    public float GetSpawnIntervalSec(int index) => index switch
    {
        1 => SpawnintervalSec_01,
        2 => SpawnintervalSec_02,
        3 => SpawnintervalSec_03,
        4 => SpawnintervalSec_04,
        5 => SpawnintervalSec_05,
        _ => -1f
    };
}

public class Wave_DataTable_WaveSpawnTableLoader
{
    public List<Wave_DataTable_WaveSpawnTable> ItemsList { get; private set; }
    public Dictionary<int, Wave_DataTable_WaveSpawnTable> ItemsDict { get; private set; }

    public Wave_DataTable_WaveSpawnTableLoader(string path = "JSON/Wave/Wave_DataTable_WaveSpawnTable")
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(path);
        if (jsonAsset == null)
        {
            Debug.LogError($"WaveSpawnTable JSON not found at path: {path}");
            ItemsList = new List<Wave_DataTable_WaveSpawnTable>();
            ItemsDict = new Dictionary<int, Wave_DataTable_WaveSpawnTable>();
            return;
        }

        string jsonData = jsonAsset.text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, Wave_DataTable_WaveSpawnTable>();
        if (ItemsList != null)
        {
            foreach (var item in ItemsList)
            {
                ItemsDict.Add(item.key, item);
            }
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<Wave_DataTable_WaveSpawnTable> Items;
    }

    public Wave_DataTable_WaveSpawnTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public Wave_DataTable_WaveSpawnTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
