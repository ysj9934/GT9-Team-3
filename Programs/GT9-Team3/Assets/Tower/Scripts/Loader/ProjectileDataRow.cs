using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ProjectileDataRow
{
    /// <summary>
    /// Projectile_ID
    /// </summary>
    public int Key;

    /// <summary>
    /// Inner_Name
    /// </summary>
    public string Inner_Name;

    /// <summary>
    /// Projectile_Grade
    /// </summary>
    public int Projectile_Grade;

    /// <summary>
    /// Damage
    /// </summary>
    public float Damage;

    /// <summary>
    /// Projectile_Speed
    /// </summary>
    public float Projectile_Speed;

    /// <summary>
    /// Attack_Radius
    /// </summary>
    public float Attack_Radius;

    /// <summary>
    /// Target_Count
    /// </summary>
    public float Target_Count;

    /// <summary>
    /// Slow_Effect
    /// </summary>
    public float Slow_Effect;

    /// <summary>
    /// Slow_Time
    /// </summary>
    public float Slow_Time;

    /// <summary>
    /// Stun_Time
    /// </summary>
    public float Stun_Time;

    /// <summary>
    /// Knockback_Tile_Count
    /// </summary>
    public float Knockback_Tile_Count;

    /// <summary>
    /// Damege_Increase
    /// </summary>
    public float Damege_Increase;

    /// <summary>
    /// Attack_Speed_Increase
    /// </summary>
    public float Attack_Speed_Increase;

    /// <summary>
    /// Buff_Tower_Count
    /// </summary>
    public int Buff_Tower_Count;

}
public class ProjectileDataTableLoader
{
    public static ProjectileDataTableLoader Instance { get; private set; }
    public List<ProjectileDataRow> ItemsList { get; private set; }
    public Dictionary<int, ProjectileDataRow> ItemsDict { get; private set; }

    public ProjectileDataTableLoader(string path = "JSON/ProjectileDataTable")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, ProjectileDataRow>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.Key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<ProjectileDataRow> Items;
    }

    public ProjectileDataRow GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public ProjectileDataRow GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
