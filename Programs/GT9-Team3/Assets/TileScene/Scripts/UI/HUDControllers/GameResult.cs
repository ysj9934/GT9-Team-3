using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AdsManager;

public class GameResult : MonoBehaviour
{
    private HUDCanvas _hudCanvas;

    [SerializeField] GameObject resultVictory;
    [SerializeField] GameObject resultDefeat;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI staminaAmountText;
    [SerializeField] GameObject hudStaminaGo;
    [SerializeField] Button staminaShopBtn;
    [SerializeField] TextMeshProUGUI worldStageText;
    [SerializeField] TextMeshProUGUI rewardAmountText;
    [SerializeField] Button exitGameBtn;
    [SerializeField] Button restartGameBtn;
    [SerializeField] Button rewardADvBtn;
    private int gameWorldLevel;
    private int gameStageLevel;
    private int rewardGold;


    private void Awake()
    {
        gameObject.SetActive(false);
        ViewHoldingStamina();
        UpdateWorldStageText();
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);

        // [사운드효과]: 버튼 클릭
        Debug.LogWarning("[Sound]: Button Click Sound");
    }

    public void OpenWindow(bool isWin)
    {
        gameWorldLevel = DataManager.Instance.stageId / 100;
        gameStageLevel = DataManager.Instance.stageId % 10;
        rewardGold = GameManager.Instance._waveController.rewardGold;
        ResultText(isWin);
        rewardAmountText.text = $"{rewardGold}";
        
        gameObject.SetActive(true);

        // 게임 데이터 저장
    }

    public void ResultText(bool isWin)
    {
        if (isWin)
        {
            resultVictory.SetActive(true);
            resultDefeat.SetActive(false);
            restartGameBtn.gameObject.SetActive(false);
            hudStaminaGo.SetActive(false);
        }
        else
        {
            resultVictory.SetActive(false);
            resultDefeat.SetActive(true);
            restartGameBtn.gameObject.SetActive(true);
            hudStaminaGo.SetActive(true);
        }
    }

    private void ViewHoldingStamina()
    {
        staminaAmountText.text = $"{ResourceManager.Instance.GetAmount(ResourceType.Mana).ToString()} / 99";
    }

    public void GoShop()
    {
        Debug.Log("Go Shop");

        // [사운드효과]: 버튼 클릭
        Debug.LogWarning("[Sound]: Button Click Sound");

    }

    public void UpdateWorldStageText()
    {
        worldStageText.text = $"월드 {gameWorldLevel} - 스테이지 {gameStageLevel}";
    }

    public void GameExitButton()
    {
        Debug.Log("GameExit to go MapUI");

        // [사운드효과]: 버튼 클릭
        Debug.LogWarning("[Sound]: Button Click Sound");

        CloseWindow();
        ResourceManager.Instance.Earn(ResourceType.Gold, rewardGold);
        Debug.Log($"{ResourceManager.Instance.GetAmount(ResourceType.Gold)}");
        SceneLoader.Instance.LoadSceneByName("Map UI");
        _hudCanvas.SetGameSpeed3x();
    }

    public void GameRetry()
    {

        Debug.Log("GameRetry");

        // [사운드효과]: 버튼 클릭
        Debug.LogWarning("[Sound]: Button Click Sound");

        if (ResourceManager.Instance.CanAfford(ResourceType.Mana, 5))
        {
            restartGameBtn.enabled = false;
            ResourceManager.Instance.Spend(ResourceType.Mana, 5);
            

            Debug.Log("게임 재시작");
            ResourceManager.Instance.Earn(ResourceType.Gold, rewardGold);
            Debug.Log($"{ResourceManager.Instance.GetAmount(ResourceType.Gold)}");
            DataManager.Instance.RestartStage(DataManager.Instance.stageId);
            _hudCanvas.SetGameSpeed3x();
            CloseWindow();
            restartGameBtn.enabled = true;
        }
        else 
        {
            Debug.Log("not enough stamina");
        }
    }

    public void GameReward2x()
    {
        Debug.Log("Reward2x to go MapUI");

        // [사운드효과]: 버튼 클릭
        Debug.LogWarning("[Sound]: Button Click Sound");

        // 광고 시청
        AdsManager.Instance.ShowRewardedAd(RewardAdType.Result2x, () =>
        {
            CloseWindow();

            // 광고 시청 성공 시 2배 보상
            ResourceManager.Instance.Earn(ResourceType.Gold, rewardGold * 2);
            Debug.Log($"{ResourceManager.Instance.GetAmount(ResourceType.Gold)}");

        },
        () => {
            GameManager.Instance.ResumeGame();

            SceneLoader.Instance.LoadSceneByName("Map UI");
        });
    }
}
