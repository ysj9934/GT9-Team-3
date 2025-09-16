using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ToastMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageTitleText;
    [SerializeField] private TextMeshProUGUI messageDescText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private Vector3 floatOffset = new Vector3(0, 1f, 0);
    [SerializeField] private float floatSpeed = 1f;

    private Action onComplete;

    private void Awake()
    {
        CloseFloatingUI();
    }

    public void FloatingUIShow(string messageTitle, string messageDesc, Color color, Action onFinish = null)
    {
        gameObject.SetActive(true);

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
        gameObject.SetActive(false);
    }

    private void CloseFloatingUI()
    {
        gameObject.SetActive(false);
    }
}
