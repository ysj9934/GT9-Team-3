using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDataLoader
{
    public static ProjectileDataLoader Instance { get; private set; }

    public List<ProjectileDataRow> ItemsList { get; private set; }
    public Dictionary<int, ProjectileDataRow> ItemsDict { get; private set; }

    public ProjectileDataLoader(string path = "JSON/ProjectileDataTable")
    {
        if (Instance != null)
        {
            Debug.LogWarning("ProjectileDataLoader 이미 초기화됨");
            return;
        }

        Instance = this;

        var jsonText = Resources.Load<TextAsset>(path)?.text;
        if (string.IsNullOrEmpty(jsonText))
        {
            Debug.LogError("ProjectileDataTable JSON 파일을 찾을 수 없습니다.");
            return;
        }

        var wrapper = JsonUtility.FromJson<Wrapper>(jsonText);
        ItemsList = wrapper.Items;
        ItemsDict = new Dictionary<int, ProjectileDataRow>();
        foreach (var row in ItemsList)
        {
            ItemsDict[row.Key] = row;
        }

        Debug.Log($"[ProjectileDataLoader] {ItemsDict.Count}개 데이터 로드 완료");
    }

    [Serializable]
    private class Wrapper
    {
        public List<ProjectileDataRow> Items;
    }

    public ProjectileDataRow GetByKey(int key)
    {
        ItemsDict.TryGetValue(key, out var row);
        return row;
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
