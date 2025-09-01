using GoogleMobileAds.Api;
using UnityEngine;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private RewardedAd rewardedAd;
    private Action onAdRewarded;

    [SerializeField] private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917"; // 테스트 ID

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MobileAds.Initialize(initStatus => { LoadRewardedAd(); });
    }

    public void LoadRewardedAd()
    {
        var request = new AdRequest();
        RewardedAd.Load(rewardedAdUnitId, request, (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogError("RewardedAd Load Failed: " + error.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("RewardedAd Loaded");

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("RewardedAd Closed");
                LoadRewardedAd(); // 자동 재로딩
            };
        });
    }

    public void ShowRewardedAd(Action onReward)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            onAdRewarded = onReward;
            rewardedAd.Show(reward =>
            {
                Debug.Log($"User rewarded: {reward.Amount} {reward.Type}");
                onAdRewarded?.Invoke();
            });
        }
        else
        {
            Debug.Log("Ad not ready");
        }
    }
}

