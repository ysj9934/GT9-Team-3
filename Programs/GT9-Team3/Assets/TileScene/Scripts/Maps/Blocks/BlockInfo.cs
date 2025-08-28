using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public class BlockInfo : MonoBehaviour
{
    public TileInfo _tileInfo;
    [SerializeField] public BlockData _blockData;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public BlockCategory blockCategory;
    [SerializeField] public int blockSerialNumber;

    private Collider2D _collider;
    
    public int level = 1;
    public bool hasTower = false;
    
    // temp
    [SerializeField] public GameObject towerUI;
    [SerializeField] public GameObject towerPrefab;

    // dnjswls
    [SerializeField] public TowerBuildUI towerUIdnjswls;
    private TowerPlacer towerPlacerdnjswls;

    private void Awake()
    {
        _tileInfo = GetComponentInParent<TileInfo>();
        _collider = GetComponent<Collider2D>();
        towerUIdnjswls = FindObjectOfType<TowerBuildUI>();
        towerPlacerdnjswls = FindObjectOfType<TowerPlacer>();

        if (towerUI != null)
            towerUI.SetActive(false);

        if (_collider != null)
            _collider.enabled = false;

    }

    private void Start()
    {
        if (_collider != null)
            _collider.enabled = true;
    }

    private void OnEnable()
    {
        Tower1 tower = GetComponentInChildren<Tower1>();
        if (BlockCategory.PlaceTower == blockCategory)
        {
            if (tower != null)
                _collider.enabled = false;
            else
                _collider.enabled = true;
        }
    }

    public void UpdateWorldLevel(int level)
    {
        this.level = level;
        spriteRenderer.sprite = _blockData.sprites[level - 1];
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        //ToggleTowerPlacementUI();
        ToggleTowerPlacementUI2();
    }

    public void ToggleTowerPlacementUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.gameObject != this.gameObject)
            {
                // 내가 아닌 다른 GameObject가 클릭된 경우 → 무시
                return;
            }
        }

        towerUI.SetActive(!towerUI.activeSelf);
    }

    public void ToggleTowerPlacementUI2()
    {
        bool isOpening = towerUIdnjswls.root.gameObject.activeSelf;
        if (isOpening)
            towerUIdnjswls.Hide();
        else
            towerUIdnjswls.ShowAt(this);
    }

    // 명칭변경 필요
    public void SetTower(TowerBlueprint bp)
    {
        _tileInfo._tilePlaceOnTower.HandleTowerPlacement(blockSerialNumber, hasTower, bp, null);
    }

    public void PlaceTower(TowerBlueprint bp)
    {
        if (hasTower)
        {
            Debug.LogWarning("A tower is already installed");
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);
        GameObject go = Instantiate(bp.towerPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        Tower1 tower = go.GetComponent<Tower1>();
        tower.ApplyData(bp);
        tower.ApplyData(bp.data);
        tower.Intialize(this);
        TowerSellUI.Instance.Show(tower);
        Debug.Log($"Tower data applied: {(float)bp.CostValue / 4}");
        ResourceManager.Instance.Spend(bp.CostType, (float)bp.CostValue / 4);
        HUDCanvas.Instance.UpdateTilePiece();

        hasTower = true;
        Debug.Log("Placed tower");

        if (_collider != null)
            _collider.enabled = false;
    }
    
    public void PlaceTower()
    {
        if (hasTower)
        {
            Debug.LogWarning("A tower is already installed");
            return;
        }
        
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);
        
        // 본인에게 설치
        GameObject go = Instantiate(towerPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        Tower1 tower = go.GetComponent<Tower1>();
        tower.Intialize(this);

        

        hasTower = true;
        Debug.Log("Placed tower");

        if (_collider != null)
            _collider.enabled = false;

        // 타워 설치시 UI 비활성화
        // Disable UI when a tower is placed;
        towerUI.SetActive(!towerUI.activeSelf);

    }

    public void RemoveTower(Tower1 currentTower)
    {
        Debug.Log("타워 제거 및 골드 환급");
        ResourceManager.Instance.Earn(currentTower.towerdata.makeCost, (float)currentTower.towerdata.sellValue / 4);

        HUDCanvas.Instance.UpdateTilePiece();

        Tower1 tower = GetComponentInChildren<Tower1>();
        Destroy(tower.gameObject);

        if (_collider != null)
            _collider.enabled = true;

        hasTower = false;
    }
}
