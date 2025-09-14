using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public Dictionary<ResourceType, float> resources = new Dictionary<ResourceType, float>();

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
        Initialize();

        Add(ResourceType.Tilepiece, 5000);
        //Add(ResourceType.Crystal, 100);
        //Add(ResourceType.Mana, 50);
    }

    void Start()
    {
        if (SaveManager.Instance != null)
        {
            resources[ResourceType.Gold] = SaveManager.Instance.data.gold;
            resources[ResourceType.Mana] = SaveManager.Instance.data.mana;
        }
    }

    private void Initialize()
    {
        resources[ResourceType.Gold] = 0;
        resources[ResourceType.Mana] = 0;
        resources[ResourceType.Crystal] = 0;
        resources[ResourceType.Tilepiece] = 0;
    }

    public bool CanAfford(ResourceType type, float cost)
    {
        return resources[type] >= cost;
    }

    public void Spend(ResourceType type, float amount)
    {
        if (CanAfford(type, amount))
        {
            resources[type] -= amount;

            if (type == ResourceType.Gold)
            {
                SaveManager.Instance.data.gold = (int)resources[type];
                SaveManager.Instance.Save();
            }
            else if (type == ResourceType.Mana)
            {
                SaveManager.Instance.data.mana = (int)resources[type];
                SaveManager.Instance.Save();
            }
        }
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

        if (type == ResourceType.Gold)
        {
            SaveManager.Instance.data.gold += (int)amount;
            SaveManager.Instance.Save();
        }
        else if (type == ResourceType.Mana)
        {
            SaveManager.Instance.data.mana = (int)resources[type]; // 갱신
            SaveManager.Instance.Save();
        }
    }
}
