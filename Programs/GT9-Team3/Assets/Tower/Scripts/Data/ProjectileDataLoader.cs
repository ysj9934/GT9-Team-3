using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDataLoader
{
    public List<ProjectileDataRow> ItemsList { get; private set; }
    public Dictionary<int, ProjectileDataRow> ItemsDict { get; private set; }

    public ProjectileDataLoader(string resourcePath = "JSON/ProjectileDataTable")
    {
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
        if (textAsset == null)
        {
            Debug.LogError("[발사체 데이터] JSON 파일을 찾을 수 없음");
            return;
        }

        ProjectileDataWrapper wrapper = JsonUtility.FromJson<ProjectileDataWrapper>(textAsset.text);
        ItemsList = wrapper.Items;
        ItemsDict = new Dictionary<int, ProjectileDataRow>();
        foreach (var item in ItemsList)
        {
            ItemsDict[item.key] = item;
        }

    }

    public ProjectileDataRow GetByKey(int key)
    {
        return ItemsDict.TryGetValue(key, out var row) ? row : null;
    }
}
