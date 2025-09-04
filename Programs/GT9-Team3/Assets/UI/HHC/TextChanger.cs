using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [Header("蹂寃쏀븷 TextMeshProUGUI")]
    public TextMeshProUGUI targetText;

    [Header("媛??⑤꼸???쒖떆???띿뒪??")]
    public string[] panelTexts; // ?⑤꼸蹂꾨줈 ?쒖떆???띿뒪??

    void Awake()
    {
        panelTexts = new string[] { "1 - ?섎뒛??", "2 - 諛붾떎??", "3 - ?⑹븫??" };
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
            Debug.LogWarning("panelIndex媛 panelTexts 踰붿쐞瑜?踰쀬뼱?ъ뒿?덈떎.");
        }
    }
}