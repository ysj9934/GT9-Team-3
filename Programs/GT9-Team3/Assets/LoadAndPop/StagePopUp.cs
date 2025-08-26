using UnityEngine;

public class StagePopUp : MonoBehaviour
{
    public GameObject popupPanel; // Panel�� �ν����Ϳ��� ����

    void Start()
    {
        // ���� �� �ڵ����� �����
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    // �˾� ����
    public void ShowPopup()
    {
        popupPanel.SetActive(true);
    }

    // �˾� �ݱ�
    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}