using UnityEngine;

public class StagePopUp : MonoBehaviour
{
    public GameObject popupPanel; // Panel을 인스펙터에서 연결

    void Start()
    {
        // 시작 시 자동으로 숨기기
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    // 팝업 열기
    public void ShowPopup()
    {
        popupPanel.SetActive(true);
    }

    // 팝업 닫기
    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}