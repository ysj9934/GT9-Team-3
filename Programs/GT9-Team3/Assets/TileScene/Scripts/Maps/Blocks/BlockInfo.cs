using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public class BlockInfo : MonoBehaviour
{
    // Block Parent
    public TileInfo _tileInfo;

    // Block Component
    [SerializeField] public SpriteRenderer spriteRenderer;
    private Collider2D _collider;

    // Block Info
    [SerializeField] public BlockData _blockData;
    [SerializeField] public BlockCategory blockCategory;
    [SerializeField] public int blockSerialNumber;
    public bool hasTower = false;

    // dnjswls
    [SerializeField] public TowerBuildUI towerUIdnjswls;
    private TowerPlacer towerPlacerdnjswls;

    private void Awake()
    {
        _tileInfo = GetComponentInParent<TileInfo>(true);
        _collider = GetComponent<Collider2D>();
        towerUIdnjswls = FindObjectOfType<TowerBuildUI>(true);
        towerPlacerdnjswls = FindObjectOfType<TowerPlacer>(true);
    }

    private void Start()
    {
        if (blockCategory == BlockCategory.PlaceTower)
        {
            _collider.enabled = true;
        }
    }

    // 확인 필요한 코드 
    // 업데이트가 가야 할거 같은 코드 
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

    /// <summary>
    /// Tile World Level
    /// World Level 갱신시 타일 성격 변화
    /// </summary>
    /// <param name="level">월드 레벨</param>
    public void UpdateWorldLevel(int level)
    {
        spriteRenderer.sprite = _blockData.sprites[level - 1];
    }

    /// <summary>
    /// Tower Installer UI Open
    /// Tower 설치 UI 열기
    /// </summary>
    public void OpenTowerInstallerUI()
    {
        towerUIdnjswls.Hide();
        towerUIdnjswls.ShowAt(this);
    }

    /// <summary>
    /// Tower Installer UI Close
    /// Tower 설치 UI 닫기
    /// </summary>
    public void CloseTowerInstallerUI()
    {
        towerUIdnjswls.Hide();
    }

    /// <summary>
    ///  Set Tower Placement
    /// Tower 위치 설정
    /// </summary>
    /// <param name="bp">타워 청사진</param>
    public void SetTowerPlace(TowerBlueprint bp)
    {
        _tileInfo._tilePlaceOnTower.HandleTowerPlacement(blockSerialNumber, hasTower, bp, null, false);
    }

    public void SetTowerUpgrade(Tower1 tower)
    {
        _tileInfo._tilePlaceOnTower.HandleTowerPlacement(blockSerialNumber, hasTower, null, tower, true);
    }

    /// <summary>
    /// Tower Install
    /// 타워 설치
    /// </summary>
    /// <param name="bp">타워 청사진</param>
    public void TowerInstall(TowerBlueprint bp)
    {
        if (hasTower)
        {
            Debug.LogWarning("A tower is already installed");
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);
        GameObject go = Instantiate(bp.towerPrefab, pos, Quaternion.identity);
        // Child of the block
        // Block에 귀속
        go.transform.SetParent(transform); 
        Tower1 tower = go.GetComponent<Tower1>();
        tower.ApplyData(bp);
        //tower.ApplyData(bp.data);
        tower.Intialize(this);

        TowerSellUI.Instance.Show(tower);

        // Tower Install Cost
        // 타워 설치 비용
        ResourceManager.Instance.Spend(bp.CostType, (float)bp.CostValue / 4);
        HUDCanvas.Instance.ShowTilePiece();

        hasTower = true;

        if (hasTower && _collider != null)
            _collider.enabled = false;
    }

    /// <summary>
    /// Remove Tower
    /// 타워 제거
    /// </summary>
    /// <param name="currentTower"></param>
    public void TowerRemove(Tower1 currentTower)
    {
        //Debug.Log("타워 제거 및 골드 환급");
        ResourceManager.Instance.Earn(currentTower.towerdata.makeCost, (float)currentTower.towerdata.sellValue / 4);
        HUDCanvas.Instance.ShowTilePiece();

        Tower1 tower = GetComponentInChildren<Tower1>(true);
        Destroy(tower.gameObject);

        hasTower = false;

        if (_collider != null && !hasTower)
            _collider.enabled = true;
    }

    public void TowerUpgrade(Tower1 currentTower)
    {
        Debug.Log("Upgrade");
        Tower1 hasTower = GetComponentInChildren<Tower1>(true);
        Destroy(hasTower.gameObject);
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);
        GameObject go = Instantiate(currentTower.gameObject, pos, Quaternion.identity);
        go.transform.SetParent(transform);
    }
}
