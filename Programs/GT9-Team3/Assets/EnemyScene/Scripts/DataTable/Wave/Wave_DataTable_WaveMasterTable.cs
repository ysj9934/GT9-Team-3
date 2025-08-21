using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Wave_DataTable_WaveMasterTable
{
    public int key;
    public string Inner_Name;
    public int Stage_ID;
    public int StageWaveNo;
    public int RoundIndex;
    public int WaveInRound;
    public int Reward_ID;
}
public class Wave_DataTable_WaveMasterTableLoader
{
    public List<Wave_DataTable_WaveMasterTable> ItemsList { get; private set; }
    public Dictionary<int, Wave_DataTable_WaveMasterTable> ItemsDict { get; private set; }

    public Wave_DataTable_WaveMasterTableLoader(string path = "JSON/Wave/Wave_DataTable_WaveMasterTable")
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(path);
        if (jsonAsset == null)
        {
            Debug.LogError($"WaveMasterTable JSON not found at path: {path}");
            ItemsList = new List<Wave_DataTable_WaveMasterTable>();
            ItemsDict = new Dictionary<int, Wave_DataTable_WaveMasterTable>();
            return;
        }

        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(jsonAsset.text);
        if (wrapper == null || wrapper.Items == null)
        {
            Debug.LogError($"Failed to parse JSON at path: {path}");
            ItemsList = new List<Wave_DataTable_WaveMasterTable>();
            ItemsDict = new Dictionary<int, Wave_DataTable_WaveMasterTable>();
            return;
        }

        ItemsList = wrapper.Items;
        ItemsDict = new Dictionary<int, Wave_DataTable_WaveMasterTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<Wave_DataTable_WaveMasterTable> Items;
    }

    public Wave_DataTable_WaveMasterTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public Wave_DataTable_WaveMasterTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
