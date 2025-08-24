using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Enemy_DataTable
{
    /// <summary>
    /// key
    /// </summary>
    public int key;

    /// <summary>
    /// Enemy_Name
    /// </summary>
    public string Enemy_Name;

    /// <summary>
    /// Enemy_Type
    /// </summary>
    public string Enemy_Type;

    /// <summary>
    /// MaxHP
    /// </summary>
    public int MaxHP;

    /// <summary>
    /// MovementSpeed
    /// </summary>
    public float MovementSpeed;

    /// <summary>
    /// AttackDamage
    /// </summary>
    public int AttackDamage;

    /// <summary>
    /// AttackSpeed
    /// </summary>
    public float AttackSpeed;

    /// <summary>
    /// AttackRange
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// AttackType
    /// </summary>
    public int AttackType;

    /// <summary>
    /// ProjectileID
    /// </summary>
    public int ProjectileID;

    /// <summary>
    /// Defense
    /// </summary>
    public int Defense;

    /// <summary>
    /// TilePieceAmount
    /// </summary>
    public int TilePieceAmount;

    /// <summary>
    /// IgnoreDebuff
    /// </summary>
    public string IgnoreDebuff;

    /// <summary>
    /// Enemy_Skill_ID
    /// </summary>
    public int Enemy_Skill_ID;

    /// <summary>
    /// Prefab_ID
    /// </summary>
    public int Prefab_ID;

    /// <summary>
    /// Enemy_Image
    /// </summary>
    public string Enemy_Image;

    /// <summary>
    /// Enemy_Size
    /// </summary>
    public float Enemy_Size;

    /// <summary>
    /// Enemy_Description
    /// </summary>
    public string Enemy_Description;

}
public class Enemy_DataTableLoader
{
    public List<Enemy_DataTable> ItemsList { get; private set; }
    public Dictionary<int, Enemy_DataTable> ItemsDict { get; private set; }

    public Enemy_DataTableLoader(string path = "JSON/Enemy/Enemy_DataTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, Enemy_DataTable>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<Enemy_DataTable> Items;
    }

    public Enemy_DataTable GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public Enemy_DataTable GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
