using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Test_Wave
{
    /// <summary>
    /// 10101
    /// </summary>
    public int key;

    /// <summary>
    /// 1?”ë“œ_ë³´í†µ?œì´??1?¤í…Œ?´ì?_1?¨ì´ë¸?
    /// </summary>
    public string Inner_Name;

    /// <summary>
    /// 101
    /// </summary>
    public int Stage_ID;

    /// <summary>
    /// 1
    /// </summary>
    public int StageWaveNo;

    /// <summary>
    /// 1
    /// </summary>
    public int RoundIndex;

    /// <summary>
    /// 1
    /// </summary>
    public int WaveInRound;

    /// <summary>
    /// 0
    /// </summary>
    public int Reward_ID;

}
public class Test_WaveLoader
{
    public List<Test_Wave> ItemsList { get; private set; }
    public Dictionary<int, Test_Wave> ItemsDict { get; private set; }

    public Test_WaveLoader(string path = "JSON/Test_Wave")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, Test_Wave>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<Test_Wave> Items;
    }

    public Test_Wave GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public Test_Wave GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
