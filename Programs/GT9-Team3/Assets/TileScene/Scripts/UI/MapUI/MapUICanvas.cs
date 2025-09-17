using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUICanvas : MonoBehaviour
{
    public ResourceManager _resourceManager;

    [SerializeField] private Button stamainAddButton;
    [SerializeField] private Button goldAddButton;
    [SerializeField] private Button diaAddButton;

    public void StageStartButton()
    {
        Debug.Log("버튼 클릭!");

        if (ResourceManager.Instance == null)
        {
            Debug.LogError("ResourceManager가 없음");
            return;
        }

        Debug.Log($"Mana 현재: {ResourceManager.Instance.GetAmount(ResourceType.Mana)}");

        if (ResourceManager.Instance.CanAfford(ResourceType.Mana, 5))
        {
            Debug.Log("Mana 충분, 5 소비");
            ResourceManager.Instance.Spend(ResourceType.Mana, 5);
        }
        else
        {
            Debug.LogWarning("Mana 부족!");
        }

        Debug.Log("씬 전환 시도");
        SceneLoader.Instance.LoadSceneByIndex(1);
    }
    
    public void AddStaminaAmount()
    {
        if (_resourceManager == null) return; // 안전 장치
        Debug.Log("Stamiana Add");

        if (_resourceManager.GetAmount(ResourceType.Mana) < 99)
        {
            Debug.Log("Stamiana earn");
            _resourceManager.Earn(ResourceType.Mana, 10);
        }

        //ShowStaminaAmount();
    }

    public void AddGoldAmount()
    {
        Debug.Log("Gold Add");
        //ShowGoldAmount();
    }

    public void AddDiaAmount()
    {
        Debug.Log("Diamond Add");
        //ShowDiaAmount();
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

    public void giveStageMode(int stageID)
    {
        if (_resourceManager.CanAfford(ResourceType.Mana, 5))
        {
            DataManager.Instance.SelectedStage(stageID);
        }
        else
        {
            Debug.Log("스태미너 부족"); // 5. else 확인
        }
    }
}
