using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataLoader
{
    public List<TowerDataRow> ItemsList { get; private set; }
    public Dictionary<int, TowerDataRow> ItemsDict { get; private set; }

    public TowerDataLoader(string resourcePath = "JSON/TowerDataTable")
    {
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
        if (textAsset == null)
        {
            Debug.LogError("[타워 데이터] JSON 파일을 찾을 수 없음");
            return;
        }

        TowerDataWrapper wrapper = JsonUtility.FromJson<TowerDataWrapper>(textAsset.text);
        ItemsList = wrapper.Items;
        ItemsDict = new Dictionary<int, TowerDataRow>();
        foreach (var item in ItemsList)
        {
            ItemsDict[item.key] = item;
        }

    }

    public TowerDataRow GetByKey(int key)
    {
        return ItemsDict.TryGetValue(key, out var row) ? row : null;
    }
}
