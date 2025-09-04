using UnityEngine;
using UnityEngine.UI;

public class ToggleSlider : MonoBehaviour
{
    public Toggle toggle;
    public RectTransform handle;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    public Vector2 onPosition;
    public Vector2 offPosition;
    public float slideSpeed = 10f;

    void Start()
    {
        // Null Checks
        if (toggle == null)
        {
            Debug.LogError("no Toggle!");
            return;
        }

        if (backgroundImage == null)
        {
            Debug.LogError("no Background Image!");
            return;
        }

        UpdateBackgroundSprite(toggle.isOn);

        // handle position based on initial toggle state
        if (handle != null)
        {
            handle.anchoredPosition = toggle.isOn ? onPosition : offPosition;
        }

        // event listener
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void Update()
    {
        if (handle != null && toggle != null)
        {
            // smoothly slide the handle
            Vector2 targetPos = toggle.isOn ? onPosition : offPosition;
            handle.anchoredPosition = Vector2.Lerp(handle.anchoredPosition, targetPos, Time.deltaTime * slideSpeed);
        }
    }

    void OnToggleChanged(bool isOn)
    {
        UpdateBackgroundSprite(isOn);
    }

    private void UpdateBackgroundSprite(bool isOn)
    {
        if (backgroundImage == null)
        {
            Debug.LogError("Background Image is null");
            return;
        }

        Sprite targetSprite = isOn ? onSprite : offSprite;
        backgroundImage.sprite = targetSprite;

        // ?щ챸??議곗젙
        if (isOn)
        {
            // ON ??遺덊닾紐?
            backgroundImage.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            // OFF ???꾩쟾 ?щ챸
            backgroundImage.color = new Color(1f, 1f, 1f, 0f);
        }

        // 蹂寃???濡쒓렇
        //Debug.Log($"?ㅽ봽?쇱씠??蹂寃? {(backgroundImage.sprite != null ? backgroundImage.sprite.name : "null")} / ?뚰뙆媛? {backgroundImage.color.a}");

        // Canvas 媛뺤젣 ?낅뜲?댄듃
        Canvas.ForceUpdateCanvases();
    }

    void OnDestroy()
    {
        // 硫붾え由??꾩닔 諛⑹?
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}