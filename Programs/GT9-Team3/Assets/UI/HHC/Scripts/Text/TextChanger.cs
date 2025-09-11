using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [Header("癰궰野껋?釉?TextMeshProUGUI")]
    public TextMeshProUGUI targetText;

    [Header("揶???ㅺ섯????뽯뻻????용뮞??")]
    public string[] panelTexts; // ??ㅺ섯癰귢쑬以???뽯뻻????용뮞??

    void Awake()
    {
        //panelTexts = new string[] { "1 - ??롫뮎??", "2 - 獄쏅뗀???", "3 - ??밸릊??" };
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
            Debug.LogWarning("panelIndex揶쎛 panelTexts 甕곕뗄?욅몴?甕곗щ선?????덈뼄.");
        }
    }
}