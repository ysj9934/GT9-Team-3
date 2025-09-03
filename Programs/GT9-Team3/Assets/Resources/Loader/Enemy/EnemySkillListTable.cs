using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EnemySkillListTable
{
    /// <summary>
    /// Skill_List_ID
    /// </summary>
    public int key;

    /// <summary>
    /// Skill_Inner_Name
    /// </summary>
    public string Skill_Inner_Name;

    /// <summary>
    /// Skill_ID
    /// </summary>
    public List<int> Skill_ID;

}
public class EnemySkillListTableLoader
{
    public List<EnemySkillListTable> ItemsList { get; private set; }
    public Dictionary<int, EnemySkillListTable> ItemsDict { get; private set; }

    public EnemySkillListTableLoader(string path = "JSON/Enemy/EnemySkillListTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, EnemySkillListTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<EnemySkillListTable> Items;
    }

    public EnemySkillListTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public EnemySkillListTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
