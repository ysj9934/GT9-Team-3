using UnityEngine;

public class PopUp : MonoBehaviour
{
    public GameObject popupPanel; // 단일 팝업

    [Header("패널 배열 (Panel1~PanelN)")]
    public GameObject[] popupPanels; // 6개 이상 패널 가능

    void Start()
    {
        if (popupPanel != null) popupPanel.SetActive(false);

        // 시작 시 모든 패널 숨기기
        if (popupPanels != null)
        {
            for (int i = 0; i < popupPanels.Length; i++)
            {
                if (popupPanels[i] != null)
                {
                    popupPanels[i].SetActive(false);
                    Debug.Log($"Start() 숨김: popupPanels[{i}] = {popupPanels[i].name}");
                }
                else
                {
                    Debug.LogWarning($"Start() null: popupPanels[{i}]");
                }
            }
        }
        else
        {
            Debug.LogWarning("Start() popupPanels 배열이 null입니다!");
        }
    }

    // 단일 팝업 열기/닫기
    public void ShowSinglePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
            Debug.Log($"popupPanel 열기: {popupPanel.name}");
        }
    }

    public void HideSinglePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            Debug.Log($"popupPanel 닫기: {popupPanel.name}");
        }
    }

    // 버튼 클릭 시 해당 패널만 활성화, 나머지는 비활성화
    public void ShowPanel(int index)
    {
        if (popupPanels == null || popupPanels.Length == 0)
        {
            Debug.LogWarning("ShowPanel() popupPanels 배열이 비어있거나 null입니다!");
            return;
        }

        if (index < 0 || index >= popupPanels.Length)
        {
            Debug.LogWarning($"ShowPanel() 잘못된 index: {index}");
            return;
        }

        Debug.Log($"ShowPanel() 호출: index={index}");

        for (int i = 0; i < popupPanels.Length; i++)
        {
            if (popupPanels[i] != null)
            {
                bool shouldBeActive = (i == index);
                popupPanels[i].SetActive(shouldBeActive);

                Debug.Log(shouldBeActive
                    ? $"패널 활성화: popupPanels[{i}] = {popupPanels[i].name}"
                    : $"패널 비활성화: popupPanels[{i}] = {popupPanels[i].name}");
            }
        }

        // UI 레이아웃 즉시 갱신 (첫 클릭 문제 방지)
        Canvas.ForceUpdateCanvases();
    }

    // 모든 패널 끄기
    public void HideAllPanels()
    {
        if (popupPanels == null) return;

        foreach (var panel in popupPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
                Debug.Log($"HideAllPanels() 닫기: {panel.name}");
            }
        }
    }
}