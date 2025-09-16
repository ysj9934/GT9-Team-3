using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileUIObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler
{
    // Managers
    private TileController _tileManager;

    // Object Structure
    public TileLink link;
    private CanvasGroup canvasGroup;
    public RectTransform inventoryContent;

    // Object Data
    private bool isHolding = false;
    private float holdTimer = 0f;
    private bool hasSpawned = false;

    // Object UX
    [SerializeField] private Image holdCircle;
    private readonly float holdDuration = 0.5f;

    // Object Functions
    [SerializeField] private Button towerInfoButton;

    private void Awake()
    {
        _tileManager = TileController.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
        link = GetComponent<TileLink>();
    }

    public void Initialize(RectTransform content)
    {
        inventoryContent = content;
    }

    void Update()
    {
        if (!GameManager.Instance._tileController.isUIActive) return;

        if (isHolding)
        {
            // Object UX
            holdTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(holdTimer / holdDuration);
            holdCircle.fillAmount = progress;

            if (!hasSpawned && holdTimer >= holdDuration)
            {
                // Object UX
                holdCircle.gameObject.SetActive(false);

                // Object SpawnWorldObject
                hasSpawned = true;
                SpawnWorldObject();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdTimer = 0f;
        hasSpawned = false;

        // Object UX
        holdCircle.fillAmount = 0f;
        holdCircle.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        holdTimer = 0f;

        // Object UX
        holdCircle.fillAmount = 0f;
        holdCircle.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }


    /// <summary>
    /// WorldObject 생성 또는 링크
    /// </summary>
    private bool isNewTile = false;
    private void SpawnWorldObject()
    {
        if (link.linkedWorldObject == null)
        {
            link.linkedWorldObject = Instantiate(link.tileObjectPrefab);
            link.linkedWorldObject.name = link.tileObjectPrefab.name;
            isNewTile = true;
        }
        link.linkedUIObject = this.gameObject;
        link.linkedWorldObject.SetActive(true);

        TileInfo tileObject = link.linkedWorldObject.GetComponent<TileInfo>();
        _tileManager.tileInfoList.Add(tileObject);
        _tileManager.tileAllCategoryList.Add(tileObject.gameObject);
        if (isNewTile)
            tileObject.Initialize(link);
        tileObject._tileMove.isDragging = true;
        tileObject.UpdateWorldLevel(GameManager.Instance.worldLevel);
        tileObject._tileLink = link;
        tileObject._tileLink.linkedUIObject = link.linkedUIObject;
        tileObject.collider2D.enabled = true;
        tileObject.gameObject.transform.SetParent(_tileManager.transform);

        link.linkedUIObject.SetActive(false);
        tileObject.isInInventory = false;
    }

    public void SendItemData()
    {
        Vector2 mousePosition = Input.mousePosition;

        if (link.linkedWorldObject != null)
        {
            TileInfo tileInfo = link.linkedWorldObject.GetComponent<TileInfo>();
            GameUIManager.Instance.canvasWindow.itemHubUI.SetTowerInfo(tileInfo, mousePosition);
        }
        else
        {
            GameUIManager.Instance.canvasWindow.itemHubUI.SetTowerInfo(null, mousePosition);
        }

        GameUIManager.Instance.canvasWindow.itemHubUI.OpenTowerInfoPanel();
    }

}
