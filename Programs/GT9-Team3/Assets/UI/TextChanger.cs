using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [Header("변경할 TextMeshProUGUI")]
    public TextMeshProUGUI targetText;

    [Header("패널별 텍스트")]
    public string[] panelTexts; // 각 패널에 대응되는 텍스트

    public void UpdateText(int panelIndex)
    {
        if (targetText == null) return;

        if (panelTexts != null && panelIndex >= 0 && panelIndex < panelTexts.Length)
        {
            targetText.text = "World " + panelTexts[panelIndex];

            switch(panelIndex)
            {
                case 0:
                    targetText.text += " - 하늘섬";
                    break;
                case 1:
                targetText.text += " - 바다섬";
                    break;
                case 2:
                targetText.text += " - 용암섬";
                    break;
            }    
        }
        else
        {
            targetText.text = "";
            Debug.LogWarning("panelIndex가 잘못되었거나 panelTexts가 설정되지 않았습니다.");
        }
    }
}