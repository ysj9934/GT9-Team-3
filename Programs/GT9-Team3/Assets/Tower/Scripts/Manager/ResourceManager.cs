using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public Dictionary<ResourceType, float> resources = new Dictionary<ResourceType, float>();
    public event Action<ResourceType, float> OnResourceChanged;

    // 회복 관련
    private const int ManaRecoveryIntervalSeconds = 300; // 5분 = 300초
    private const int ManaRecoveryAmount = 1;
    private bool manaRecoveryRunning = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 모든 ResourceType 초기화
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType))) resources[type] = 0f;

        // 씬 이동에도 한 번만 오프라인 회복 및 코루틴 시작
        if (!manaRecoveryRunning)
        {
            StartCoroutine(ManaRecoveryCoroutine());
            manaRecoveryRunning = true;
        }

        Add(ResourceType.Tilepiece, 5000);
    }

    public void ApplySavedResources()   // SaveManager의 저장된 자원 적용
    {
        if (SaveManager.Instance == null)
        {
            Debug.Log("[ApplySavedResources] SaveManager가 없음");
            return;
        }

        resources[ResourceType.Gold] = SaveManager.Instance.data.gold;
        resources[ResourceType.Mana] = SaveManager.Instance.data.mana;
        resources[ResourceType.Crystal] = SaveManager.Instance.data.crystal;

        Debug.Log($"[ApplySavedResources] Mana: " +
            $"{resources[ResourceType.Mana]}, Gold: {resources[ResourceType.Gold]}, Crystal: {resources[ResourceType.Crystal]}");

        OnResourceChanged?.Invoke(ResourceType.Mana, resources[ResourceType.Mana]);
        OnResourceChanged?.Invoke(ResourceType.Gold, resources[ResourceType.Gold]);
        OnResourceChanged?.Invoke(ResourceType.Crystal, resources[ResourceType.Crystal]);
    }

    public void ApplyOfflineRecovery()
    {
        if (SaveManager.Instance == null) return;

        var lastSave = SaveManager.Instance.data.lastSaveTime;
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long elapsedSeconds = now - lastSave;
        int recoveredMana = (int)(elapsedSeconds / ManaRecoveryIntervalSeconds) * ManaRecoveryAmount;

        // 디버그 출력
        Debug.Log($"[OfflineRecovery] 이전 저장 시각: {lastSave} (UnixTime), 현재 시각: {now} (UnixTime), 경과 시간: {elapsedSeconds}초" +
            $"[OfflineRecovery] 회복 예정 마나: {recoveredMana}");

        if (recoveredMana > 0)
        {
            Earn(ResourceType.Mana, recoveredMana);
            SaveManager.Instance.Save(); // 즉시 저장
        }
    }

    private IEnumerator ManaRecoveryCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(ManaRecoveryIntervalSeconds);

            // 최대 마나 99 제한
            if (resources[ResourceType.Mana] < 99)
            {
                float recoverAmount = Mathf.Min(ManaRecoveryAmount, 99 - resources[ResourceType.Mana]);
                Earn(ResourceType.Mana, recoverAmount);
            }
        }
    }

    public bool CanAfford(ResourceType type, float cost)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0f; // 없으면 0으로 초기화
        return resources[type] >= cost;
    }
 
    public void Spend(ResourceType type, float amount)
    {
        if (!CanAfford(type, amount)) return;
        resources[type] -= amount;
        OnResourceChanged?.Invoke(type, resources[type]);
    }

    public void Add(ResourceType type, float amount)
    {
        resources[type] += amount;
    }

    public float GetAmount(ResourceType type)
    {
        return resources[type];
    }

    // ResourceManager는 저장 책임 제거
    public void Earn(ResourceType type, float amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;

        if (type == ResourceType.Mana && resources[type] > 99)
            resources[type] = 99;

        Debug.Log($"[자원] {type} +{amount} 획득, 현재: {resources[type]}");
        OnResourceChanged?.Invoke(type, resources[type]);
    }
}
