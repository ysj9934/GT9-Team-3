using UnityEngine;
using UnityEngine.EventSystems;

public class TileMove : MonoBehaviour
{
    // Managers
    private TileController _tileManager;
    private HUDCanvas _hudCanvas;


    // Object Structure
    //private Camera _camera;
    private TileData _tileData;
    private TileInfo _tileInfo;
    private Collider2D _collider;
    
    private readonly float holdDuration = 0.5f;
    
    public Vector2 originalPosition;
    public Color originalColor;
    public SpriteRenderer[] _sprites;

    public bool isDragging = false;
    public bool isPressing = false;
    public float pressTime = 0f;

    // InventoryItem
    private RectTransform inventoryContent;
    private TileUIObject tileUIObject;

    private void Awake()
    {
        _tileManager = TileController.Instance;
        _hudCanvas = HUDCanvas.Instance;
        //_camera = Camera.main;

        _tileData = GetComponent<TileData>();
        _tileInfo = GetComponent<TileInfo>();
        _collider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        inventoryContent = _hudCanvas._itemHudUI.inventoryContent;
    }
    
    public void TileMovePress()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        isPressing = true;
        pressTime = 0;
        originalPosition = transform.position;
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        originalColor = _sprites[0].color;
    }

    private void Update()
    {
        if (_tileManager.isMoveActive)
        {
            if (isPressing && !isDragging)
            {
                _tileInfo._tileUI.OpenLoadingUI();
                pressTime += Time.deltaTime;
                float progress = Mathf.Clamp01(pressTime / holdDuration);
                _tileInfo._tileUI.holdCircle.fillAmount = progress;

                // 0.5초 이상 누르면 이동 모드로 전환
                if (pressTime >= holdDuration)
                {
                    _tileInfo._tileUI.CloseLoadingUI();

                    isDragging = true;
                    _tileManager.CloseTowerRangeUI();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isPressing = false;
                pressTime = 0;
                _tileInfo._tileUI.holdCircle.fillAmount = 0f;
                _tileInfo._tileUI.CloseLoadingUI();
            }

            if (!isDragging) return;

            TileMoveDrag();

            if (Input.GetMouseButtonUp(0) ||
                (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {

                isDragging = false;

                if (EventSystem.current.IsPointerOverGameObject())
                {
                    LinkiedUI();
                }

                if (gameObject.activeSelf)
                {
                    TileMoveUp();
                }
            }
        }
    }

    /// <summary>
    /// Linked UI Object
    /// 링크 연동하기
    /// </summary>
    private void LinkedUIObject()
    { 
        _tileInfo._tileLink.linkedUIObject = Instantiate(_tileInfo._tileLink.tileUIPrefab);
        _tileInfo._tileLink.linkedUIObject.name = _tileInfo._tileLink.linkedUIObject.name;

        tileUIObject = _tileInfo._tileLink.linkedUIObject.GetComponent<TileUIObject>();
        tileUIObject.Initialize(inventoryContent);
        tileUIObject.link = _tileInfo._tileLink;
    }

    public void LinkiedUI()
    {
        // 본인을 끄고 게임에 영향을 받지 않은 구역으로 이동시킨다.
        gameObject.SetActive(false);
        _tileInfo.isInInventory = true;

        // 연결된 UIObject가 없는 경우
        if (_tileInfo._tileLink.linkedUIObject == null)
        {
            LinkedUIObject();
        }

        // 연결된 UIObject가 있는 경우
        GameObject uiItem = _tileInfo._tileLink.linkedUIObject;
        uiItem.SetActive(true);

        var itemImage = uiItem.GetComponent<TileUIObject>();
        itemImage.Initialize(inventoryContent);

        // 양방향 연결 설정
        itemImage.link = _tileInfo._tileLink;
        _tileInfo._tileLink.linkedUIObject = uiItem;
        _tileInfo._tileLink.linkedWorldObject = this.gameObject;

        RectTransform uiRect = uiItem.GetComponent<RectTransform>();
        uiRect.SetParent(itemImage.inventoryContent, false);
        uiRect.anchoredPosition = Vector2.zero;

        // 타일 연결정보 끊기
        CutTileConnectedInfo();
    }

    private void TileMoveDrag()
    { 
        if (isDragging)
        {
            _tileManager.CloseTileUI(null);
            // 들어 올린 것 처럼 보인다
            UpdateSpriteOrder();

            Plane plane = new Plane(Vector3.forward, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            _collider.enabled = false;

            if (plane.Raycast(ray, out float distance))
            {
                Vector2 point = ray.GetPoint(distance);
                Vector2 delta = point - originalPosition;

                // 기준 벡터들
                Vector2 diagUpRight = new Vector2(_tileManager.tileSize[0], _tileManager.tileSize[1]);
                Vector2 diagUpLeft = new Vector2(-_tileManager.tileSize[0], _tileManager.tileSize[1]);
                Vector2 diagDownRight = new Vector2(_tileManager.tileSize[0], -_tileManager.tileSize[1]);
                Vector2 diagDownLeft = new Vector2(-_tileManager.tileSize[0], -_tileManager.tileSize[1]);
                Vector2 horizontal = new Vector2(_tileManager.tileSize[0] * 2, 0f);
                Vector2 rhorizontal = new Vector2(-_tileManager.tileSize[0] * 2, 0f);
                Vector2 vertical = new Vector2(0f, _tileManager.tileSize[1] * 2);
                Vector2 rvertical = new Vector2(0f, -_tileManager.tileSize[1] * 2);
                Vector2 dualHorizontalUp = new Vector2(_tileManager.tileSize[0] * 4, _tileManager.tileSize[1] * 2);
                Vector2 dualHorizontalDown = new Vector2(_tileManager.tileSize[0] * 4, -_tileManager.tileSize[1] * 2);
                Vector2 rdualHorizontalUp = new Vector2(-_tileManager.tileSize[0] * 4, _tileManager.tileSize[1] * 2);
                Vector2 rdualHorizontalDown = new Vector2(-_tileManager.tileSize[0] * 4, -_tileManager.tileSize[1] * 2);
                Vector2 dualVerticalUp = new Vector2(_tileManager.tileSize[0] * 2, _tileManager.tileSize[1] * 4);
                Vector2 dualVerticalDown = new Vector2(_tileManager.tileSize[0] * 2, -_tileManager.tileSize[1] * 4);
                Vector2 rdualVerticalUp = new Vector2(-_tileManager.tileSize[0] * 2, _tileManager.tileSize[1] * 4);
                Vector2 rdualVerticalDown = new Vector2(-_tileManager.tileSize[0] * 2, -_tileManager.tileSize[1] * 4);
                Vector2 dualUpRight = new Vector2(_tileManager.tileSize[0] * 3, _tileManager.tileSize[1]);
                Vector2 dualUpleft = new Vector2(-_tileManager.tileSize[0] * 3, _tileManager.tileSize[1]);
                Vector2 dualDownRight = new Vector2(_tileManager.tileSize[0] * 3, -_tileManager.tileSize[1]);
                Vector2 dualDownleft = new Vector2(-_tileManager.tileSize[0] * 3, -_tileManager.tileSize[1]);
                Vector2 rdualUpRight = new Vector2(_tileManager.tileSize[0], _tileManager.tileSize[1] * 3);
                Vector2 rdualUpleft = new Vector2(-_tileManager.tileSize[0], _tileManager.tileSize[1] * 3);
                Vector2 rdualDownRight = new Vector2(_tileManager.tileSize[0], -_tileManager.tileSize[1] * 3);
                Vector2 rdualDownleft = new Vector2(-_tileManager.tileSize[0], -_tileManager.tileSize[1] * 3);
                Vector2 trippleUpRight = new Vector2(_tileManager.tileSize[0] * 5, _tileManager.tileSize[1]);
                Vector2 trippleUpleft = new Vector2(-_tileManager.tileSize[0] * 5, _tileManager.tileSize[1]);
                Vector2 trippleDownRight = new Vector2(_tileManager.tileSize[0] * 5, -_tileManager.tileSize[1]);
                Vector2 trippleDownleft = new Vector2(-_tileManager.tileSize[0] * 5, -_tileManager.tileSize[1]);
                Vector2 trippleUpdualRight = new Vector2(_tileManager.tileSize[0] * 5, _tileManager.tileSize[1] * 3);
                Vector2 trippleUpdualleft = new Vector2(-_tileManager.tileSize[0] * 5, _tileManager.tileSize[1] * 3);
                Vector2 trippleDowndualRight = new Vector2(_tileManager.tileSize[0] * 5, -_tileManager.tileSize[1] * 3);
                Vector2 trippleDowndualleft = new Vector2(-_tileManager.tileSize[0] * 5, -_tileManager.tileSize[1] * 3);
                Vector2 rtrippleUpRight = new Vector2(_tileManager.tileSize[0], _tileManager.tileSize[1] * 5);
                Vector2 rtrippleUpleft = new Vector2(-_tileManager.tileSize[0], _tileManager.tileSize[1] * 5);
                Vector2 rtrippleDownRight = new Vector2(_tileManager.tileSize[0], -_tileManager.tileSize[1] * 5);
                Vector2 rtrippleDownleft = new Vector2(-_tileManager.tileSize[0], -_tileManager.tileSize[1] * 5);
                Vector2 rtrippleUpdualRight = new Vector2(_tileManager.tileSize[0] * 3, _tileManager.tileSize[1] * 5);
                Vector2 rtrippleUpdualleft = new Vector2(-_tileManager.tileSize[0] * 3, _tileManager.tileSize[1] * 5);
                Vector2 rtrippleDowndualRight = new Vector2(_tileManager.tileSize[0] * 3, -_tileManager.tileSize[1] * 5);
                Vector2 rtrippleDowndualleft = new Vector2(-_tileManager.tileSize[0] * 3, -_tileManager.tileSize[1] * 5);


                // 가장 가까운 방향 찾기
                Vector2[] directions =
                {
                    diagUpRight,
                    diagUpLeft,
                    diagDownRight,
                    diagDownLeft,
                    horizontal,
                    rhorizontal,
                    vertical,
                    rvertical,
                    dualHorizontalUp,
                    dualHorizontalDown,
                    rdualHorizontalUp,
                    rdualHorizontalDown,
                    dualVerticalUp,
                    dualVerticalDown,
                    rdualVerticalUp,
                    rdualVerticalDown,
                    dualUpRight,
                    dualUpleft,
                    dualDownRight,
                    dualDownleft,
                    rdualUpRight,
                    rdualUpleft,
                    rdualDownRight,
                    rdualDownleft,
                    trippleUpRight,
                    trippleUpleft,
                    trippleDownRight,
                    trippleDownleft,
                    trippleUpdualRight,
                    trippleUpdualleft,
                    trippleDowndualRight,
                    trippleDowndualleft,
                    rtrippleUpRight,
                    rtrippleUpleft,
                    rtrippleDownRight,
                    rtrippleDownleft,
                    rtrippleUpdualRight,
                    rtrippleUpdualleft,
                    rtrippleDowndualRight,
                    rtrippleDowndualleft,
                };

                Vector2 bestDir = directions[0];
                float maxDot = Vector2.Dot(delta.normalized, directions[0].normalized);

                for (int i = 1; i < directions.Length; i++)
                {
                    float dot = Vector2.Dot(delta.normalized, directions[i].normalized);
                    if (dot > maxDot)
                    {
                        maxDot = dot;
                        bestDir = directions[i];
                    }
                }

                // 몇 칸 움직일지 계산
                float magnitude = delta.magnitude;
                float stepSize = bestDir.magnitude;
                int steps = Mathf.RoundToInt(magnitude / stepSize);

                // 새로운 위치 계산
                Vector2 newPos = originalPosition + (bestDir.normalized * stepSize * steps);
                transform.position = new Vector2(newPos.x, newPos.y);
            }

            bool isValid = IsValidPosition(transform.position);

            foreach (var sprite in _sprites)
            {
                // TowerRange는 무시
                TowerRange towerRange = sprite.GetComponent<TowerRange>();
                //towerRange.gameObject.SetActive(false);
                if (towerRange) continue;

                if (isValid)
                {
                    sprite.color = originalColor;
                }
                else
                {
                    sprite.color = Color.red;
                }
                    
            }
        }
    }
    
    public void TileMoveUp()
    {
        foreach (var sprite in _sprites)
        {
            TowerRange towerRange = sprite.GetComponent<TowerRange>();
            if (towerRange) continue;

            sprite.color = originalColor;
        }
        
        // Castle에 맞는 경우가 종종 있음
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (hit != null)
        {
            TileData tileData = hit != null ? hit.GetComponent<TileData>() : null;
            Castle tileCastle = hit != null ? hit.GetComponent<Castle>() : null;

            if (tileData != null || tileCastle != null)
            {
                transform.position = originalPosition;
                UpdateGridPosition();
                Debug.Log("this location already located");
            }
            else
            {
                UpdateGridPosition();
            }
        }
        

        if (_collider != null)
            _collider.enabled = true;

        isDragging = false;
        isPressing = false;
        pressTime = 0;
        UpdateGridPosition();

        // [사운드효과]: 타일 배치
        Debug.LogWarning("[Sound]: Tile Pos Sound");
    }
    
    private void UpdateGridPosition()
    {
        _tileData.UpdateMapping(transform.position);
        _tileData.UpdateTileIndex();
        _tileInfo.UpdateSpriteOrder();
        _tileManager.SetNeighbors();
    }
    
    private bool IsValidPosition(Vector2 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos);
        return hit == null || hit.gameObject == this.gameObject;
    }

    public void UpdateSpriteOrder()
    {
        foreach (SpriteRenderer sr in _sprites)
        {
            bool isTower = sr.GetComponent<Tower1>() != null;

            if (isTower)
            {
                int towerOrder = 1000 + (_tileInfo.tileIndex * 10);
                sr.sortingOrder = towerOrder;
            }
            else
            {
                int baseOrder = _tileInfo.originBlockOrder.ContainsKey(sr) ? _tileInfo.originBlockOrder[sr] : 0;
                sr.sortingOrder = baseOrder + (_tileInfo.tileIndex * 10) - 1000 + 1000;    
            }
        }
    }

    private void CutTileConnectedInfo()
    {
        if (IsValidTilePosition(_tileInfo.tileRow, _tileInfo.tileCol))
        {
            _tileManager.tileMap[_tileInfo.tileRow, _tileInfo.tileCol] = null;
        }
        
        _tileManager.SetNeighbors();
    }

    private bool IsValidTilePosition(int row, int col)
    {
        //Debug.Log($"Tile Position - Row: {row}, Col: {col}");
        return row >= 0 && row < _tileManager.tileMap.GetLength(0) &&
               col >= 0 && col < _tileManager.tileMap.GetLength(1);
    }
}