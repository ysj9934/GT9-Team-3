using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public ResourceManager _resourceManager;

    [SerializeField] private TextMeshProUGUI staminaHoldingAmountText;
    [SerializeField] private TextMeshProUGUI goldHoldingAmountText;
    [SerializeField] private TextMeshProUGUI diaHoldingAmountText;

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;

        // 리소스 변경 시 UI 업데이트 이벤트 구독
        if (_resourceManager != null)
            _resourceManager.OnResourceChanged += UpdateUI;

        // 초기 UI 업데이트
        UpdateUI(ResourceType.Mana, _resourceManager.GetAmount(ResourceType.Mana));
        UpdateUI(ResourceType.Gold, _resourceManager.GetAmount(ResourceType.Gold));
        UpdateUI(ResourceType.Crystal, _resourceManager.GetAmount(ResourceType.Crystal));
    }

    private void OnDestroy()
    {
        if (_resourceManager != null)
            _resourceManager.OnResourceChanged -= UpdateUI;
    }

    private void UpdateUI(ResourceType type, float value)
    {
        if (_resourceManager == null) return;

        switch (type)
        {
            case ResourceType.Mana:
                if (staminaHoldingAmountText != null)
                    staminaHoldingAmountText.text = $"{(int)value} / 99";
                break;
            case ResourceType.Gold:
                if (goldHoldingAmountText != null)
                    goldHoldingAmountText.text = $"{(int)value}";
                break;
            case ResourceType.Crystal:
                if (diaHoldingAmountText != null)
                    diaHoldingAmountText.text = $"{(int)value}";
                break;
        }
    }
}