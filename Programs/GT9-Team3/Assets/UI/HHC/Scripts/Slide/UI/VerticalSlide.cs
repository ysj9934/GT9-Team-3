using UnityEngine;

public class VerticalSlide : MonoBehaviour
{
    [Header("Slide Settings")]
    public RectTransform slideContainer; // Parent container for all slide panels
    public int panelCount = 3;           // Number of panels
    public float slideSpeed = 5f;        // Sliding speed
    public float dragSensitivity = 1f;   // Drag sensitivity
    private float swipeThreshold;        // Threshold to recognize swipe movement

    [Header("Text Manager")]
    public TextChanger textChanger; // Reference to text changer (e.g., TMP manager)

    [Header("Navigation Arrows")]
    public GameObject arrowUp;   // Up arrow UI
    public GameObject arrowDown; // Down arrow UI

    private Vector2 startTouchPos;
    private Vector2 targetPosition;
    private bool isDragging = false;

    void Start()
    {
        float screenHeight = Screen.height;

        // Swipe threshold = 10% of screen height
        swipeThreshold = screenHeight * 0.1f;

        panelCount = slideContainer.childCount;

        // 이미지 패널 초기화
        for (int i = 0; i < panelCount; i++)
        {
            RectTransform rt = slideContainer.GetChild(i).GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 1);
            rt.sizeDelta = new Vector2(0, screenHeight);
            rt.anchoredPosition = new Vector2(0, -i * screenHeight);
        }

        // 초기 위치 설정
        targetPosition = slideContainer.anchoredPosition;

        if (textChanger != null)
            textChanger.UpdateText(0);

        UpdateArrow(0);
    }

    void Update()
    {
        HandleInput();

        // 이미지 그룹 이동
        slideContainer.anchoredPosition = Vector2.Lerp(
            slideContainer.anchoredPosition,
            targetPosition,
            Time.deltaTime * slideSpeed
        );
    }

    void HandleInput()
    {
        Vector2 currentPos = Vector2.zero;
        bool inputBegan = false;
        bool inputEnded = false;
        bool inputMoved = false;

        // 터치 입력
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            currentPos = touch.position;

            if (touch.phase == TouchPhase.Began) inputBegan = true;
            else if (touch.phase == TouchPhase.Moved) inputMoved = true;
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) inputEnded = true;
        }
        // 마우스 입력
        else
        {
            if (Input.GetMouseButtonDown(0)) { currentPos = Input.mousePosition; inputBegan = true; }
            else if (Input.GetMouseButton(0)) { currentPos = Input.mousePosition; inputMoved = true; }
            else if (Input.GetMouseButtonUp(0)) { currentPos = Input.mousePosition; inputEnded = true; }
        }

        if (inputBegan)
        {
            startTouchPos = currentPos;
            isDragging = true;
        }
        else if (inputMoved && isDragging)
        {
            float deltaY = (currentPos.y - startTouchPos.y) * dragSensitivity;
            slideContainer.anchoredPosition += new Vector2(0, deltaY);
            startTouchPos = currentPos;
        }
        else if (inputEnded && isDragging)
        {
            isDragging = false;
            SnapToPanel();
        }
    }

    void SnapToPanel()
    {
        float dragDistance = slideContainer.anchoredPosition.y - targetPosition.y;
        int currentIndex = Mathf.RoundToInt(targetPosition.y / Screen.height);
        int nextIndex = currentIndex;

        if (dragDistance > swipeThreshold) nextIndex = currentIndex + 1;
        else if (dragDistance < -swipeThreshold) nextIndex = currentIndex - 1;

        nextIndex = Mathf.Clamp(nextIndex, 0, panelCount - 1);
        targetPosition = new Vector2(0, nextIndex * Screen.height);

        if (textChanger != null)
            textChanger.UpdateText(nextIndex);

        UpdateArrow(nextIndex);

        Transform currentPanel = slideContainer.GetChild(nextIndex);
        //Debug.Log("현재 패널 : " + currentPanel.name);
    }

    void UpdateArrow(int panelIndex)
    {
        if (panelIndex == 0)
        {
            if (arrowUp != null) arrowUp.SetActive(false);
            if (arrowDown != null) arrowDown.SetActive(true);
        }
        else if (panelIndex == panelCount - 1)
        {
            if (arrowUp != null) arrowUp.SetActive(true);
            if (arrowDown != null) arrowDown.SetActive(false);
        }
        else
        {
            if (arrowUp != null) arrowUp.SetActive(true);
            if (arrowDown != null) arrowDown.SetActive(true);
        }
    }
}