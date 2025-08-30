using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Wave_DataTable
{
    /// <summary>
    /// WDSW_ID
    /// </summary>
    public int key;

    /// <summary>
    /// Inner_Name
    /// </summary>
    public string Inner_Name;

    /// <summary>
    /// Stage_ID
    /// </summary>
    public int Stage_ID;

    /// <summary>
    /// StageWaveNo
    /// </summary>
    public int StageWaveNo;

    /// <summary>
    /// RoundIndex
    /// </summary>
    public int RoundIndex;

    /// <summary>
    /// WaveInRound
    /// </summary>
    public int WaveInRound;

    /// <summary>
    /// StageStartTilePiece
    /// </summary>
    public int StageStartTilePiece;

    /// <summary>
    /// RewardGoldAmount
    /// </summary>
    public int RewardGoldAmount;

    /// <summary>
    /// SpawnStartTime
    /// </summary>
    public List<float> SpawnStartTime;

    /// <summary>
    /// EnemyID
    /// </summary>
    public List<int> EnemyID;

    /// <summary>
    /// SpawnBatchSize
    /// </summary>
    public List<int> SpawnBatchSize;

    /// <summary>
    /// SpawnRepeat
    /// </summary>
    public List<int> SpawnRepeat;

    /// <summary>
    /// SpawnintervalSec
    /// </summary>
    public List<float> SpawnintervalSec;

}
public class Wave_DataTableLoader
{
    public List<Wave_DataTable> ItemsList { get; private set; }
    public Dictionary<int, Wave_DataTable> ItemsDict { get; private set; }

    public Wave_DataTableLoader(string path = "JSON/Wave/Wave_DataTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, Wave_DataTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<Wave_DataTable> Items;
    }

    public Wave_DataTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public Wave_DataTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
