using UnityEngine;

public class VerticalSlide : MonoBehaviour
{
    [Header("슬라이드 설정")]
    public RectTransform slideContainer; // 3개의 Image를 담는 컨테이너
    public int panelCount = 3;           // 이미지(패널) 개수
    public float slideSpeed = 5f;        // 스냅 이동 속도
    public float dragSensitivity = 1f;   // 드래그 감도

    private Vector2 startTouchPos;
    private Vector2 targetPosition;
    private bool isDragging = false;

    void Start()
    {
        // ✅ 실행 시 자동으로 컨테이너 크기 조정
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        // ✅ 자식 Image 개수 자동 계산
        panelCount = slideContainer.childCount;

        // ✅ 컨테이너 높이 자동 조정
        slideContainer.sizeDelta = new Vector2(
            screenWidth,
            screenHeight * panelCount
        );

        // ✅ 자식 이미지 자동 배치
        for (int i = 0; i < panelCount; i++)
        {
            RectTransform rt = slideContainer.GetChild(i).GetComponent<RectTransform>();

            // Anchor & Pivot 설정 (Top 기준)
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 1);

            // 크기 = 화면 크기
            rt.sizeDelta = new Vector2(0, screenHeight);

            // 위치 = 화면 높이만큼 아래로
            rt.anchoredPosition = new Vector2(0, -i * screenHeight);
        }
    }

    void Update()
    {
        HandleTouchInput();
        HandleMouseInput(); // PC 디버그용

        // 부드럽게 이동
        slideContainer.anchoredPosition = Vector2.Lerp(
            slideContainer.anchoredPosition,
            targetPosition,
            Time.deltaTime * slideSpeed
        );
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startTouchPos = touch.position;
            isDragging = true;
        }
        else if (touch.phase == TouchPhase.Moved && isDragging)
        {
            Vector2 currentTouchPos = touch.position;
            float deltaY = (currentTouchPos.y - startTouchPos.y) * dragSensitivity;
            slideContainer.anchoredPosition += new Vector2(0, deltaY);
            startTouchPos = currentTouchPos;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isDragging = false;
            SnapToNearestPanel();
        }
    }

    void HandleMouseInput()
    {
        // PC 디버그용 마우스 입력
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentPos = (Vector2)Input.mousePosition;
            float deltaY = (currentPos.y - startTouchPos.y) * dragSensitivity;
            slideContainer.anchoredPosition += new Vector2(0, deltaY);
            startTouchPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            SnapToNearestPanel();
        }
    }

    void SnapToNearestPanel()
    {
        float y = slideContainer.anchoredPosition.y;
        float screenHeight = Screen.height;

        int index = Mathf.RoundToInt(-y / screenHeight);
        index = Mathf.Clamp(index, 0, panelCount - 1); // 범위 제한

        targetPosition = new Vector2(0, -index * screenHeight);
    }
}