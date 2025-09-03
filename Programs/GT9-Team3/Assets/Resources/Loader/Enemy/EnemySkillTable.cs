using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EnemySkillTable
{
    /// <summary>
    /// Skill_ID
    /// </summary>
    public int key;

    /// <summary>
    /// Skill_Name
    /// </summary>
    public string Skill_Name;

    /// <summary>
    /// Skill_Trigger
    /// </summary>
    public string Skill_Trigger;

    /// <summary>
    /// TriggerValue
    /// </summary>
    public float TriggerValue;

    /// <summary>
    /// EffectType
    /// </summary>
    public string EffectType;

    /// <summary>
    /// EffectCategory
    /// </summary>
    public string EffectCategory;

    /// <summary>
    /// TargetType
    /// </summary>
    public string TargetType;

    /// <summary>
    /// AreaRadius
    /// </summary>
    public float AreaRadius;

    /// <summary>
    /// CastTime
    /// </summary>
    public float CastTime;

    /// <summary>
    /// Duration
    /// </summary>
    public float Duration;

    /// <summary>
    /// SpawnEnemy_ID
    /// </summary>
    public int SpawnEnemy_ID;

    /// <summary>
    /// EffectValue
    /// </summary>
    public int EffectValue;

    /// <summary>
    /// Cooldown
    /// </summary>
    public float Cooldown;

}
public class EnemySkillTableLoader
{
    public List<EnemySkillTable> ItemsList { get; private set; }
    public Dictionary<int, EnemySkillTable> ItemsDict { get; private set; }

    public EnemySkillTableLoader(string path = "JSON/Enemy/EnemySkillTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, EnemySkillTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<EnemySkillTable> Items;
    }

    public EnemySkillTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public EnemySkillTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
