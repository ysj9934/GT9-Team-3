using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDMessageUI : MonoBehaviour
{
    [SerializeField] public GameObject PopupUI;
    [SerializeField] public GameObject FloatingUI;

    // PopupMessage
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI confirmButtonText;
    [SerializeField] private TextMeshProUGUI cancelButtonText;

    private Action onConfirm;
    private Action onCancel;

    // FloatingMessageUI
    [SerializeField] private TextMeshProUGUI messageTitleText;
    [SerializeField] private TextMeshProUGUI messageDescText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private Vector3 floatOffset = new Vector3(0, 1f, 0);
    [SerializeField] private float floatSpeed = 1f;

    private Action onComplete;

    private void Awake()
    {
        PopupUIHide();
        FloatingUI.SetActive(false);
    }

    public void PopupUIShow(string title, string message, string confirmText = "확인", string cancelText = "취소", Action confirmAction = null, Action cancelAction = null)
    {
        titleText.text = title;
        bodyText.text = message;

        confirmButtonText.text = confirmText;
        cancelButtonText.text = cancelText;

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

        PopupUI.SetActive(true);
    }

    public void PopupUIHide()
    {
        PopupUI.SetActive(false);
    }

    public void FloatingUIShow(string messageTitle, string messageDesc, Color color, Action onFinish = null)
    {
        FloatingUI.SetActive(true);

        messageTitleText.text = messageTitle;
        messageDescText.text = messageDesc;
        messageDescText.color = color;
        canvasGroup.alpha = 1f;
        onComplete = onFinish;

        StartCoroutine(FloatAndFade());
    }

    private IEnumerator FloatAndFade()
    {
        float visibleDuration = 2f;     // 보여주는 시간
        float fadeDuration = 0.5f;        // 사라지는 시간
        float timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + floatOffset;

        // 1️⃣ 메시지 보여주는 단계 (3초)
        while (timer < visibleDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timer / visibleDuration);
            canvasGroup.alpha = 1f;
            timer += Time.deltaTime;
            yield return null;
        }

        // 2️⃣ 메시지 사라지는 단계 (1초 페이드 아웃)
        timer = 0f;
        Vector3 fadeStartPos = transform.position;
        Vector3 fadeEndPos = fadeStartPos + floatOffset * 0.5f;

        while (timer < fadeDuration)
        {
            transform.position = Vector3.Lerp(fadeStartPos, fadeEndPos, timer / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        onComplete?.Invoke();
        FloatingUI.SetActive(false);

    }


}
