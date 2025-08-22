using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Enemy_DataTable_EnemyMaster_DataTable
{
    public int key;
    public string Enemy_Name;
    public int Prefab_ID;
    public string Enemy_Image;
    public float Enemy_Size;
    public string Enemy_Type;
    public string Enemy_Description;
}
public class Enemy_DataTable_EnemyMaster_DataTableLoader
{
    public List<Enemy_DataTable_EnemyMaster_DataTable> ItemsList { get; private set; }
    public Dictionary<int, Enemy_DataTable_EnemyMaster_DataTable> ItemsDict { get; private set; }

    public Enemy_DataTable_EnemyMaster_DataTableLoader(string path = "JSON/Enemy/Enemy_DataTable_EnemyMaster_DataTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, Enemy_DataTable_EnemyMaster_DataTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<Enemy_DataTable_EnemyMaster_DataTable> Items;
    }

    public Enemy_DataTable_EnemyMaster_DataTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public Enemy_DataTable_EnemyMaster_DataTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
