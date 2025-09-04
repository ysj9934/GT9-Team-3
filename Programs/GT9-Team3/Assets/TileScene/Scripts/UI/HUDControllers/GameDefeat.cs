using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AdsManager;

public class GameDefeat : MonoBehaviour
{
    private HUDCanvas _hudCanvas;

    [SerializeField] private Button closeWindowBtn;
    [SerializeField] private TextMeshProUGUI holdDiaAmountText;
    [SerializeField] private Button goShopBtn;
    [SerializeField] private Button watchADvReviveBtn;
    [SerializeField] private Button giveCryReviveBtn;

    

    private void Awake()
    {
        _hudCanvas = GetComponentInParent<HUDCanvas>();
    }

    public HUDCanvas Initialize(HUDCanvas hudCanvas)
    {
        gameObject.SetActive(false);
        ViewHoldingCrystal();
        return _hudCanvas = hudCanvas;
    }

    /// <summary>
    /// Close Game Defeat Panel and then Open Game Result Panel
    /// 패배 창을 닫고 결과창 열기
    /// </summary>
    public void CloseWindown()
    { 
        gameObject.SetActive(false);

        // game result 로 이동
        _hudCanvas._hudResultPanel._gameResultPanel.OpenWindow(false);
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Number of crystals currently held
    /// 보유하고 있는 크리스탈 양
    /// </summary>
    private void ViewHoldingCrystal()
    {
        holdDiaAmountText.text = ResourceManager.Instance.GetAmount(ResourceType.Crystal).ToString();
    }

    /// <summary>
    /// Go Shop
    /// 크리스탈 상점 페이지로 이동
    /// </summary>
    public void GoShop()
    {
        Debug.Log("Go Shop");
        // Go Crystal Shop
    }

    /// <summary>
    /// Watch ADv for revive
    /// 광고 시청 후 부활
    /// </summary>
    public void ReviveToADv()
    {
        Debug.Log("Adv revive");

        // 광고 시청
        AdsManager.Instance.ShowRewardedAd(RewardAdType.Retry, () =>
        {
          
            Debug.Log("광고 시청 완료 - 부활 처리");

            // 패널 닫기
            gameObject.SetActive(false);

            // castle 체력 초기화

            // 웨이브 다시 시작?

            // 속도 초기화
            HUDCanvas.Instance.SetGameSpeed3x();

        });
    }

    /// <summary>
    /// Spend Crystal ofr revive
    /// 크리스탈을 소모하여 부활
    /// </summary>
    public void ReviveToCry()
    {
        Debug.Log("Cry revive");

        if (ResourceManager.Instance.CanAfford(ResourceType.Crystal, 50))
        {
            ResourceManager.Instance.Spend(ResourceType.Crystal, 50);
            ViewHoldingCrystal();
            // 재시작
        }
        else
        {
            Debug.Log("Dont have Cry enough");
            CloseWindown();
        }
        
    }
}
