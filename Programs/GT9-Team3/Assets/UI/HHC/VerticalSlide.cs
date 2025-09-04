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
        float screenWidth = Screen.width;

        // Swipe threshold = 10% of screen height
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

        // Set initial position
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

            // Clamp movement at first and last panel
            int currentIndex = Mathf.RoundToInt(slideContainer.anchoredPosition.y / Screen.height);
            currentIndex = Mathf.Clamp(currentIndex, 0, panelCount - 1);

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

            // Check if swipe exceeded threshold to change panel
            if (dragDistance > swipeThreshold) nextIndex = currentIndex + 1;   // Swipe down
            else if (dragDistance < -swipeThreshold) nextIndex = currentIndex - 1; // Swipe up

            nextIndex = Mathf.Clamp(nextIndex, 0, panelCount - 1);

            // Move to target panel
            targetPosition = new Vector2(0, nextIndex * Screen.height);

            // Update text
            if (textChanger != null)
                textChanger.UpdateText(nextIndex);

            // Update arrows
            UpdateArrow(nextIndex);

            Transform currentPanel = slideContainer.GetChild(nextIndex);
            Debug.Log("Current panel: " + currentPanel.name);
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

            // Prevent dragging beyond first and last panels
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

            if (dragDistance > swipeThreshold) nextIndex = currentIndex + 1;   // Swipe down
            else if (dragDistance < -swipeThreshold) nextIndex = currentIndex - 1; // Swipe up

            nextIndex = Mathf.Clamp(nextIndex, 0, panelCount - 1);

            targetPosition = new Vector2(0, nextIndex * Screen.height);

            Transform currentPanel = slideContainer.GetChild(nextIndex);
            Debug.Log("Current panel: " + currentPanel.name);

            if (textChanger != null)
                textChanger.UpdateText(nextIndex);

            // Update arrows
            UpdateArrow(nextIndex);
        }
    }

    void UpdateArrow(int panelIndex)
    {
        // First panel
        if (panelIndex == 0)
        {
            if (arrowUp != null) arrowUp.SetActive(false);
            if (arrowDown != null) arrowDown.SetActive(true);
        }
        // Last panel
        else if (panelIndex == panelCount - 1)
        {
            if (arrowUp != null) arrowUp.SetActive(true);
            if (arrowDown != null) arrowDown.SetActive(false);
        }
        // Middle panels
        else
        {
            if (arrowUp != null) arrowUp.SetActive(true);
            if (arrowDown != null) arrowDown.SetActive(true);
        }
    }
}