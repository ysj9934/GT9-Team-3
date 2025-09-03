using UnityEngine;

public class VerticalSlide : MonoBehaviour
{
    [Header("슬라이드 설정")]
    public RectTransform slideContainer; // 3개의 Image를 담는 컨테이너
    public int panelCount = 3;           // 이미지(패널) 개수
    public float slideSpeed = 5f;        // 스냅 이동 속도
    public float dragSensitivity = 1f;   // 드래그 감도
    private float swipeThreshold;        // 화면 높이의 몇 % 이상 드래그 시 스냅

    [Header("텍스트 변경용")]
    public TextChanger textChanger; // Tile_Flag_01_Blue 연결

    [Header("슬라이드 화살표")]
    public GameObject arrowUp;   // 위 화살표
    public GameObject arrowDown; // 아래 화살표


    private Vector2 startTouchPos;
    private Vector2 targetPosition;
    private bool isDragging = false;

    void Start()
    {
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        // 스와이프 최소 거리 = 화면 높이의 30%
        swipeThreshold = screenHeight * 0.1f;

        panelCount = slideContainer.childCount;

        for (int i = 0; i < panelCount; i++)
        {
            RectTransform rt = slideContainer.GetChild(i).GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 1);

            rt.sizeDelta = new Vector2(0, screenHeight);
            rt.anchoredPosition = new Vector2(0, -i * screenHeight);
        }

        // 초기 스냅 위치
        targetPosition = slideContainer.anchoredPosition;

        textChanger.UpdateText(0);
        UpdateArrow(0);
    }

    void Update()
    {
        HandleTouchInput();
        HandleMouseInput();

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

            // 현재 패널 인덱스 계산
            int currentIndex = Mathf.RoundToInt(slideContainer.anchoredPosition.y / Screen.height);
            currentIndex = Mathf.Clamp(currentIndex, 0, panelCount - 1);

            // 첫/마지막 패널에서 이동 제한
            if (currentIndex == 0 && deltaY < 0) deltaY = 0;
            else if (currentIndex == panelCount - 1 && deltaY > 0) deltaY = 0;

            slideContainer.anchoredPosition += new Vector2(0, deltaY);
            startTouchPos = currentTouchPos;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isDragging = false;

            float dragDistance = slideContainer.anchoredPosition.y - targetPosition.y;
            int currentIndex = Mathf.RoundToInt(targetPosition.y / Screen.height);
            int nextIndex = currentIndex;

            // 스와이프 임계값에 따라 다음 패널 계산
            if (dragDistance > swipeThreshold) nextIndex = currentIndex + 1;   // 위로 스와이프
            else if (dragDistance < -swipeThreshold) nextIndex = currentIndex - 1; // 아래로 스와이프

            nextIndex = Mathf.Clamp(nextIndex, 0, panelCount - 1);

            // 스냅 위치 업데이트
            targetPosition = new Vector2(0, nextIndex * Screen.height);

            // TMP 텍스트 업데이트
            if (textChanger != null)
                textChanger.UpdateText(nextIndex);

            // 화살표 업데이트
            UpdateArrow(nextIndex);

            Transform currentPanel = slideContainer.GetChild(nextIndex);
            Debug.Log("현재 패널 이름: " + currentPanel.name);
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentPos = (Vector2)Input.mousePosition;
            float deltaY = (currentPos.y - startTouchPos.y) * dragSensitivity;

            int currentIndex = Mathf.RoundToInt(slideContainer.anchoredPosition.y / Screen.height);
            currentIndex = Mathf.Clamp(currentIndex, 0, panelCount - 1);

            // 첫 패널에서 아래 이동 금지, 마지막 패널에서 위 이동 금지
            if (currentIndex == 0 && deltaY < 0) deltaY = 0;
            else if (currentIndex == panelCount - 1 && deltaY > 0) deltaY = 0;

            slideContainer.anchoredPosition += new Vector2(0, deltaY);
            startTouchPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            float dragDistance = slideContainer.anchoredPosition.y - targetPosition.y;

            int currentIndex = Mathf.RoundToInt(targetPosition.y / Screen.height);
            int nextIndex = currentIndex;

            if (dragDistance > swipeThreshold) nextIndex = currentIndex + 1;   // 위로 스와이프
            else if (dragDistance < -swipeThreshold) nextIndex = currentIndex - 1; // 아래로 스와이프

            nextIndex = Mathf.Clamp(nextIndex, 0, panelCount - 1);

            targetPosition = new Vector2(0, nextIndex * Screen.height);

            Transform currentPanel = slideContainer.GetChild(nextIndex);
            Debug.Log("현재 패널 이름: " + currentPanel.name);

            if (textChanger != null)
                textChanger.UpdateText(nextIndex);

                // 화살표 업데이트
                UpdateArrow(nextIndex);
        }
    }

    void UpdateArrow(int panelIndex)
    {
        // 제일 위 패널
        if (panelIndex == 0)
        {
            if (arrowUp != null) arrowUp.SetActive(false);
            if (arrowDown != null) arrowDown.SetActive(true);
        }
        // 제일 아래 패널
        else if (panelIndex == panelCount - 1)
        {
            if (arrowUp != null) arrowUp.SetActive(true);
            if (arrowDown != null) arrowDown.SetActive(false);
        }
        // 가운데 패널
        else
        {
            if (arrowUp != null) arrowUp.SetActive(true);
            if (arrowDown != null) arrowDown.SetActive(true);
        }
    }
}
//void Update()
//{
//    HandleTouchInput();
//    HandleMouseInput(); // PC 디버그용

//    // 부드럽게 이동
//    slideContainer.anchoredPosition = Vector2.Lerp(
//        slideContainer.anchoredPosition,
//        targetPosition,
//        Time.deltaTime * slideSpeed
//    );
//}

//void HandleTouchInput()
//{
//    if (Input.touchCount == 0) return;

//    Touch touch = Input.GetTouch(0);

//    if (touch.phase == TouchPhase.Began)
//    {
//        startTouchPos = touch.position;
//        isDragging = true;
//    }
//    else if (touch.phase == TouchPhase.Moved && isDragging)
//    {
//        Vector2 currentTouchPos = touch.position;
//        float deltaY = (currentTouchPos.y - startTouchPos.y) * dragSensitivity;
//        slideContainer.anchoredPosition += new Vector2(0, deltaY);
//        startTouchPos = currentTouchPos;
//    }
//    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
//    {
//        isDragging = false;
//        SnapToNearestPanel();
//    }
//}

//void HandleMouseInput()
//{
//    // PC 디버그용 마우스 입력
//    if (Input.GetMouseButtonDown(0))
//    {
//        startTouchPos = Input.mousePosition;
//        isDragging = true;
//    }
//    else if (Input.GetMouseButton(0) && isDragging)
//    {
//        Vector2 currentPos = (Vector2)Input.mousePosition;
//        float deltaY = (currentPos.y - startTouchPos.y) * dragSensitivity;
//        slideContainer.anchoredPosition += new Vector2(0, deltaY);
//        startTouchPos = currentPos;
//    }
//    else if (Input.GetMouseButtonUp(0) && isDragging)
//    {
//        isDragging = false;
//        SnapToNearestPanel();
//    }
//}

//void SnapToNearestPanel()
//{
//    float y = slideContainer.anchoredPosition.y;
//    float screenHeight = 1080;

//    int index = Mathf.RoundToInt(y / screenHeight);
//    index = Mathf.Clamp(index, 0, panelCount - 1);

//    targetPosition = new Vector2(0, index * screenHeight);
//}
