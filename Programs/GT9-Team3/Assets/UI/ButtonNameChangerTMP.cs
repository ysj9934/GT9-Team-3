using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonNameChangerTMP : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        // ?덉떆 ?곗씠??
        string playerItemName = SaveManager.Instance.data.gold.ToString(); // ?닿굔 ?숈쟻?쇰줈 諛붾뚮뒗 ?곗씠?곕씪怨?媛??

        // 踰꾪듉 ?덉쓽 TextMeshProUGUI 而댄룷?뚰듃 媛?몄삤湲?
        TextMeshProUGUI buttonText = myButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = playerItemName; // 3踰??댁슜 ?곸슜
    }
}