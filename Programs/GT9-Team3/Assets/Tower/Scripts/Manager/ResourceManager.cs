using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private Dictionary<ResourceType, int> resources = new();

    void Awake()
    {
        Instance = this;
        resources[ResourceType.Gold] = 10000;
    }

    public bool CanAfford(ResourceType type, int cost)
    {
        return resources[type] >= cost;
    }

    public void Spend(ResourceType type, int amount)
    {
        if (CanAfford(type, amount))
            resources[type] -= amount;
    }

    public void Add(ResourceType type, int amount)
    {
        resources[type] += amount;
    }

    public int GetAmount(ResourceType type)
    {
        return resources[type];
    }

    public void Earn(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;
        Debug.Log($"[ÀÚ¿ø] {type} +{amount} È¹µæ, ÇöÀç: {resources[type]}");
    }
}
