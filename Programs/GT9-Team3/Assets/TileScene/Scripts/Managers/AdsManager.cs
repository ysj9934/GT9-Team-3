using GoogleMobileAds.Api;
using UnityEngine;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private RewardedAd rewardedAd;
    private Action onAdRewarded;

    [SerializeField] private string rewardedAdUnitId = "ca-app-pub-9623407653018480~7943626435";

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
                Debug.LogError("보상형 광고 로드 실패: " + error.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("보상형 광고 로드 완료");

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("광고 창이 닫혔습니다");
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
                Debug.Log($"사용자가 보상을 획득했습니다: {reward.Amount} {reward.Type}");
                onAdRewarded?.Invoke();
            });
        }
        else
        {
            Debug.Log("광고가 아직 준비되지 않았습니다");
        }
    }
}
