using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class TowerDataTable
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
    /// Target Order1
    /// </summary>
    public string Target_Order1;

    /// <summary>
    /// Target Order2
    /// </summary>
    public string Target_Order2;

    /// <summary>
    /// Target Order3
    /// </summary>
    public string Target_Order3;

    /// <summary>
    /// Target Order4
    /// </summary>
    public string Target_Order4;

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
    public List<TowerDataTable> ItemsList { get; private set; }
    public Dictionary<int, TowerDataTable> ItemsDict { get; private set; }

    public TowerDataTableLoader(string path = "JSON/TowerDataTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, TowerDataTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<TowerDataTable> Items;
    }

    public TowerDataTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public TowerDataTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
