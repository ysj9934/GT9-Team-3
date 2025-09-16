using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmMessage : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private TextMeshProUGUI cancelText;
    private Action onConfirm;
    private Action onCancel;

    private void Awake()
    {
        PopupUIHide();
    }

    public void PopupUIShow(string title, string message, string confirmText = "확인", string cancelText = "취소", Action confirmAction = null, Action cancelAction = null)
    {
        titleText.text = title;
        contentText.text = message;

        this.confirmText.text = confirmText;
        this.cancelText.text = cancelText;

        onConfirm = confirmAction;
        onCancel = cancelAction;

        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke();
            PopupUIHide();
        });

        cancelButton.onClick.AddListener(() =>
        {
            onCancel?.Invoke();
            PopupUIHide();
        });

        gameObject.SetActive(true);
    }

    public void PopupUIHide()
    {
        gameObject.SetActive(false);
    }
}
