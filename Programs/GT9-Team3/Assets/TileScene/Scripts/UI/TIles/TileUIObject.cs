using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileUIObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler
{
    // Object Structure
    public TileLink link;
    private CanvasGroup canvasGroup;
    public RectTransform inventoryContent;

    // Object Data
    private bool isHolding = false;
    private float holdTimer = 0f;
    private bool hasSpawned = false;

    // Object UX
    //[SerializeField] private Image holdCircle;
    private readonly float holdDuration = 0.5f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(RectTransform content)
    {
        inventoryContent = content;
    }

    void Update()
    {
        if (isHolding)
        {
            // Object UX
            holdTimer += Time.deltaTime;
            //float progress = Mathf.Clamp01(holdTimer / holdDuration);
            //holdCircle.fillAmount = progress;

            if (!hasSpawned && holdTimer >= holdDuration)
            {
                // Object UX
                //holdCircle.gameObject.SetActive(false);

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
        // canvasGroup.alpha = 0.5f;
        // canvasGroup.blocksRaycasts = false;

        // Object UX
        //holdCircle.fillAmount = 0f;
        //holdCircle.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        holdTimer = 0f;
        // canvasGroup.alpha = 1f;
        // canvasGroup.blocksRaycasts = true;

        // Object UX
        //holdCircle.fillAmount = 0f;
        //holdCircle.gameObject.SetActive(false);
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
    private void SpawnWorldObject()
    {
        if (link.linkedWorldObject == null)
        {
            link.linkedWorldObject = Instantiate(link.tileObjectPrefab);
            link.linkedWorldObject.name = link.tileObjectPrefab.name;
        }
        link.linkedUIObject = this.gameObject;
        link.linkedWorldObject.SetActive(true);

        TileInfo tileObject = link.linkedWorldObject.GetComponent<TileInfo>();
        tileObject._tileMove.isDragging = true;
        tileObject._tileLink = link;
        tileObject._tileLink.linkedUIObject = link.linkedUIObject;

        link.linkedUIObject.SetActive(false);
    }
}
