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

        // 투명도 조정
        if (isOn)
        {
            // ON → 불투명
            backgroundImage.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            // OFF → 완전 투명
            backgroundImage.color = new Color(1f, 1f, 1f, 0f);
        }

        // 변경 후 로그
        Debug.Log($"스프라이트 변경: {(backgroundImage.sprite != null ? backgroundImage.sprite.name : "null")} / 알파값: {backgroundImage.color.a}");

        // Canvas 강제 업데이트
        Canvas.ForceUpdateCanvases();
    }

    void OnDestroy()
    {
        // 메모리 누수 방지
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}