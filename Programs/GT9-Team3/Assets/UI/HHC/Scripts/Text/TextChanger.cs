using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [Header("변경할 TextMeshProUGUI")]
    public TextMeshProUGUI targetText;

    [Header("패널별로 표시할 텍스트 목록")]
    public string[] panelTexts;

    private void OnEnable()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.StageChanged += ShowStageInfo;
    }

    private void OnDisable()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.StageChanged -= ShowStageInfo;
    }

    public void UpdateText(int panelIndex)
    {
        if (targetText == null) return;

        if (panelTexts != null && panelIndex >= 0 && panelIndex < panelTexts.Length)
        {
            targetText.text = "World " + panelTexts[panelIndex];
        }
        else
        {
            targetText.text = "";
            Debug.LogWarning("panelIndex가 panelTexts 범위를 벗어났습니다.");
        }
    }

    public void ShowStageInfo()
    {
        if (targetText == null || DataManager.Instance == null) return;

        targetText.text = "월드 " + DataManager.Instance.worldCode +
                          " - 스테이지 " + DataManager.Instance.stageCode;
    }
}