using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class TowerDataRow
{
    /// <summary>
    /// Tower_ID
    /// </summary>
    public int key;

    /// <summary>
    /// Inner_Name
    /// </summary>
    public string Inner_Name;

    /// <summary>
    /// Tower_Grade
    /// </summary>
    public int Tower_Grade;

    /// <summary>
    /// Use_Projectile
    /// </summary>
    public string Use_Projectile;

    /// <summary>
    /// Attack_type
    /// </summary>
    public string Attack_type;

    /// <summary>
    /// Target_Order1
    /// </summary>
    public string Target_Order1;

    /// <summary>
    /// Target_Order2
    /// </summary>
    public string Target_Order2;

    /// <summary>
    /// Target_Order3
    /// </summary>
    public string Target_Order3;

    /// <summary>
    /// Target_Order4
    /// </summary>
    public string Target_Order4;

    /// <summary>
    /// Attack_Range
    /// </summary>
    public float Attack_Range;

    /// <summary>
    /// Attack_Speed
    /// </summary>
    public float Attack_Speed;

    /// <summary>
    /// Upgrade_Cost
    /// </summary>
    public string Upgrade_Cost;

    /// <summary>
    /// Upgrade_Value
    /// </summary>
    public int Upgrade_Value;

    /// <summary>
    /// Make_Cost
    /// </summary>
    public string Make_Cost;

    /// <summary>
    /// Make_Value
    /// </summary>
    public int Make_Value;

    /// <summary>
    /// Sell_Cost
    /// </summary>
    public string Sell_Cost;

    /// <summary>
    /// Sell_Value
    /// </summary>
    public int Sell_Value;

    /// <summary>
    /// User_Level
    /// </summary>
    public int User_Level;

}
public class TowerDataTableLoader
{
    public static TowerDataTableLoader Instance { get; private set; }
    public List<TowerDataRow> ItemsList { get; private set; }
    public Dictionary<int, TowerDataRow> ItemsDict { get; private set; }

    public TowerDataTableLoader(string path = "JSON/TowerDataTable")
    {
        Instance = this;

        string jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, TowerDataRow>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<TowerDataRow> Items;
    }

    public TowerDataRow GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public TowerDataRow GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
