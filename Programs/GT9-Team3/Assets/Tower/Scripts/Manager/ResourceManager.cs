using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public Dictionary<ResourceType, float> resources = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        // Initialize resources
        resources[ResourceType.Gold] = 120;
        resources[ResourceType.Tilepiece] = 5000;
    }

    public bool CanAfford(ResourceType type, float cost)
    {
        return resources[type] >= cost;
    }

    public void Spend(ResourceType type, float amount)
    {
        if (CanAfford(type, amount))
            resources[type] -= amount;
    }

    public void Add(ResourceType type, float amount)
    {
        resources[type] += amount;
    }

    public float GetAmount(ResourceType type)
    {
        return resources[type];
    }

    public void Earn(ResourceType type, float amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;
        Debug.Log($"[자원] {type} +{amount} 획득, 현재: {resources[type]}");
    }

    public float ShowTilePiece()
    {
        return resources.ContainsKey(ResourceType.Tilepiece) ? resources[ResourceType.Tilepiece] : 0;
    }

    public float showGold()
    {
        return resources.ContainsKey(ResourceType.Gold) ? resources[ResourceType.Gold] : 0;
    }
}
