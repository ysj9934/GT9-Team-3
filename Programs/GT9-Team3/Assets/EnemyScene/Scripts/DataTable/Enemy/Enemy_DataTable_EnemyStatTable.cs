using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Enemy_DataTable_EnemyStatTable
{
    public int key;
    public string Enemy_Inner_Name;
    public int MaxHP;
    public float MovementSpeed;
    public int AttackDamage;
    public float AttackSpeed;
    public float AttackRange;
    public int AttackType;
    public int ProjectileID;
    public int Defense;
    public int TilePieceAmount;
    public string IgnoreDebuff;
    public int Enemy_Skill_ID;
}

public class Enemy_DataTable_EnemyStatTableLoader
{
    public List<Enemy_DataTable_EnemyStatTable> ItemsList { get; private set; }
    public Dictionary<int, Enemy_DataTable_EnemyStatTable> ItemsDict { get; private set; }

    public Enemy_DataTable_EnemyStatTableLoader(string path = "JSON/Enemy/Enemy_DataTable_EnemyStatTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, Enemy_DataTable_EnemyStatTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<Enemy_DataTable_EnemyStatTable> Items;
    }

    public Enemy_DataTable_EnemyStatTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public Enemy_DataTable_EnemyStatTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
