using GoogleMobileAds.Api;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private Dictionary<string, RewardedAd> rewardedAds = new();
    private Dictionary<string, Action> rewardCallbacks = new();

    private RewardAdType currentAdType;

    private readonly Dictionary<RewardAdType, string> adUnitIds = new()
    {
        { RewardAdType.Result2x, "ca-app-pub-9623407653018480/3840444484" },
        { RewardAdType.SpeedBoost, "ca-app-pub-9623407653018480/4443365128" },
        { RewardAdType.Retry, "ca-app-pub-9623407653018480/6230804106" }
    };

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MobileAds.Initialize(initStatus =>
        {
            foreach (var kvp in adUnitIds)
                LoadRewardedAd(kvp.Key);
        });
    }

    public void LoadRewardedAd(RewardAdType type)
    {
        string adUnitId = adUnitIds[type];

        var request = new AdRequest();
        RewardedAd.Load(adUnitId, request, (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogError($"[Ad:{type}] 로드 실패 - {error.GetMessage()}");
                return;
            }

            rewardedAds[type.ToString()] = ad;
            Debug.Log($"[Ad:{type}] 로드 완료");

            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log($"[Ad:{type}] 닫힘 → 재로드");
                LoadRewardedAd(type);
            };
        });
    }

    public void ShowRewardedAd(RewardAdType type, Action onRewarded, Action onAdClosed = null)
    {
        string key = type.ToString();

        if (rewardedAds.TryGetValue(key, out var ad) && ad.CanShowAd())
        {
            currentAdType = type;
            rewardCallbacks[key] = onRewarded;

            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log($"[Ad:{type}] 광고 닫힘");
                onAdClosed?.Invoke();
            };

            ad.Show(reward =>
            {
                Debug.Log($"[Ad:{type}] 보상 지급됨: {reward.Amount} {reward.Type}");
                rewardCallbacks[key]?.Invoke();
                
            });
        }
        else
        {
            Debug.LogWarning($"[Ad:{type}] 광고 준비 안됨");
        }
    }

    public enum RewardAdType
    {
        Result2x,
        SpeedBoost,
        Retry
    }

}
