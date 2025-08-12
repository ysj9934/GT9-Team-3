using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PortalData
{
    /// <summary>
    /// portalCode
    /// </summary>
    public int key;

    /// <summary>
    /// 씬코드
    /// </summary>
    public int SceneCode;

    /// <summary>
    /// 씬화면명
    /// </summary>
    public string SceneDisplayName;

    /// <summary>
    /// 씬코드명
    /// </summary>
    public string SceneCodeName;

    /// <summary>
    /// 씬타입
    /// </summary>
    public int SceneType;

}
public class PortalDataLoader
{
    public List<PortalData> ItemsList { get; private set; }
    public Dictionary<int, PortalData> ItemsDict { get; private set; }

    public PortalDataLoader(string path = "JSON/PortalData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, PortalData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<PortalData> Items;
    }

    public PortalData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public PortalData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
