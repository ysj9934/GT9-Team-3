using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUICanvas : MonoBehaviour
{
    public ResourceManager _resourceManager;

    //[SerializeField] private Button Startbutton;
    //[SerializeField] private TextMeshProUGUI worldStageInfoText;
    [SerializeField] private TextMeshProUGUI staminaHoldingAmountText;
    [SerializeField] private TextMeshProUGUI goldHoldingAmountText;
    [SerializeField] private TextMeshProUGUI diaHoldingAmountText;
    [SerializeField] private Button stamainAddButton;
    [SerializeField] private Button goldAddButton;
    [SerializeField] private Button diaAddButton;

    private void Awake()
    {
        //_resourceManager = ResourceManager.Instance;

        //if (_resourceManager == null)
        //    Debug.LogError("ResourceManager.Instance가 존재하지 않습니다!");
    }

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;

        if (_resourceManager != null)
        {
            UpdateUI(ResourceType.Mana, _resourceManager.GetAmount(ResourceType.Mana));
            UpdateUI(ResourceType.Gold, _resourceManager.GetAmount(ResourceType.Gold));
            UpdateUI(ResourceType.Crystal, _resourceManager.GetAmount(ResourceType.Crystal));
        }
    }

    private void OnEnable()
    {
        if (_resourceManager != null)
            _resourceManager.OnResourceChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (_resourceManager != null)
            _resourceManager.OnResourceChanged -= UpdateUI;
    }

    private void UpdateUI(ResourceType type, float value)
    {
        switch (type)
        {
            case ResourceType.Mana: ShowStaminaAmount(); break;
            case ResourceType.Gold: ShowGoldAmount(); break;
            case ResourceType.Crystal: ShowDiaAmount(); break;
        }
    }

    public void StageStartButton()
    {
        if (_resourceManager.CanAfford(ResourceType.Mana, 5))
        {
            _resourceManager.Spend(ResourceType.Mana, 5);
            Debug.Log("Mana spent 5");
            SceneLoader.Instance.LoadSceneByIndex(1);
        }
    }

    //public void TextWorldStageInfo(int stageId)
    //{
    //    int worldText = stageId / 100;
    //    int stageText = stageId % 10;
    //    worldStageInfoText.text = $"World {worldText} - Stage {stageText}";
    //}


    public float ShowResourceAmount(ResourceType type)
    {
        return _resourceManager.GetAmount(type);
    }

    public void ShowStaminaAmount()
    {
        staminaHoldingAmountText.text = $"{(int) ShowResourceAmount(ResourceType.Mana)} / 99";
    }

    public void ShowGoldAmount()
    {
        goldHoldingAmountText.text = $"{ShowResourceAmount(ResourceType.Gold)}";
    }

    public void ShowDiaAmount()
    {
        diaHoldingAmountText.text = $"{ShowResourceAmount(ResourceType.Crystal)}";
    }

    public void AddStaminaAmount()
    {
        if (_resourceManager == null) return; // 안전 장치
        Debug.Log("Stamiana Add");

        if (_resourceManager.GetAmount(ResourceType.Mana) < 99)
        {
            Debug.Log("Stamiana earn");
            _resourceManager.Earn(ResourceType.Mana, 1);
        }

        ShowStaminaAmount();
    }

    public void AddGoldAmount()
    {
        Debug.Log("Gold Add");
        ShowGoldAmount();
    }

    public void AddDiaAmount()
    {
        Debug.Log("Diamond Add");
        ShowDiaAmount();
    }

    public void EnterHardMode()
    {
        if (ResourceManager.Instance == null)
        {
            Debug.Log("ResourceManager.Instance is null");
            return;
        }

        float currentMana = ResourceManager.Instance.GetAmount(ResourceType.Mana);

        if (ResourceManager.Instance.CanAfford(ResourceType.Mana, 10))
        {
            ResourceManager.Instance.Spend(ResourceType.Mana, 10);
            DataManager.Instance.SelectedStage(401);
            SceneLoader.Instance.LoadSceneByName("PresentationScene");
        }
        else
        {
            Debug.Log("스태미너 부족"); // 5. else 확인
        }
    }
}
