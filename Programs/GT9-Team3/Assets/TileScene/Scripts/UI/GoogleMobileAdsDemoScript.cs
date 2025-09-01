using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    private RewardedAd rewardedAd;

    void Awake()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob Initialized");
            LoadRewardedAd();
        });
    }

    void LoadRewardedAd()
    {
        var request = new AdRequest();
        RewardedAd.Load("ca-app-pub-3940256099942544/5224354917", request, (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogError($"RewardedAd load failed: {error}");
                return;
            }
            rewardedAd = ad;
            Debug.Log("RewardedAd loaded");
        });
    }

    public void ShowRewardedAd(System.Action onRewarded)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"User rewarded: {reward.Amount} {reward.Type}");
                onRewarded?.Invoke();
            });

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                rewardedAd.Destroy();
                LoadRewardedAd(); // 재로딩
            };
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready");
        }
    }
}
