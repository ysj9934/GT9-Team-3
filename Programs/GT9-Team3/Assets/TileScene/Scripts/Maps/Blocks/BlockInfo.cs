using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BlockInfo : MonoBehaviour
{
    // Block Parent
    public TileInfo _tileInfo;

    // Block Component
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public PolygonCollider2D _collider;

    // Block Info
    [SerializeField] public BlockData _blockData;
    [SerializeField] public BlockCategory blockCategory;
    [SerializeField] public int blockSerialNumber;
    public bool hasTower = false;

    // dnjswls
    [SerializeField] public TowerBuildUI towerUIdnjswls;
    private TowerPlacer towerPlacerdnjswls;

    private void Start()
    {
        _tileInfo = GetComponentInParent<TileInfo>(true);
        towerUIdnjswls = FindObjectOfType<TowerBuildUI>(true);
    }

    // 확인 필요한 코드 
    // 업데이트가 가야 할거 같은 코드 
    private void OnEnable()
    {
        _tileInfo = GetComponentInParent<TileInfo>(true);
        towerUIdnjswls = FindObjectOfType<TowerBuildUI>(true);
    }

    private void Initialize()
    {
        _tileInfo = GetComponentInParent<TileInfo>(true);
        towerUIdnjswls = FindObjectOfType<TowerBuildUI>(true);
    }

    /// <summary>
    /// Tile World Level
    /// World Level 갱신시 타일 성격 변화
    /// </summary>
    /// <param name="level">월드 레벨</param>
    public void UpdateWorldLevel(int level)
    {
        if (level < 4)
            spriteRenderer.sprite = _blockData.sprites[level - 1];
        else
            spriteRenderer.sprite = _blockData.sprites[0];
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

    public void SetTowerPlace(TowerBlueprint bp)
    {
        _tileInfo._tilePlaceOnTower.HandleTowerPlacement(blockSerialNumber, hasTower, bp, null, false);
    }

    public void SetTowerUpgrade(Tower1 tower)
    {
        if (_tileInfo != null)
        {
            _tileInfo._tilePlaceOnTower.HandleTowerPlacement(blockSerialNumber, hasTower, null, tower, true);
        }
        else
        {
            Initialize();
            SetTowerUpgrade(tower);
        }
        
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
        tower.Intialize(this);
        

        string towerName = tower.towerdata.innerName;
        string towerType = towerName.Substring(1, towerName.IndexOf("_") - 1);

        if (_tileInfo == null)
            _tileInfo = GetComponentInParent<TileInfo>(true);

        _tileInfo.hasTowerList.Add(tower);

        switch (towerType)
        {
            case "Common":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Common))
                    _tileInfo.towerInfo[TowerCategory.Common] = 0;
                _tileInfo.towerInfo[TowerCategory.Common]++;
                break;
            case "Splash":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Splash))
                    _tileInfo.towerInfo[TowerCategory.Splash] = 0;
                _tileInfo.towerInfo[TowerCategory.Splash]++;
                break;
            case "Slow":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Slow))
                    _tileInfo.towerInfo[TowerCategory.Slow] = 0;
                _tileInfo.towerInfo[TowerCategory.Slow]++;
                break;
            case "Stun":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Stun))
                    _tileInfo.towerInfo[TowerCategory.Stun] = 0;
                _tileInfo.towerInfo[TowerCategory.Stun]++;
                break;
            case "Doom":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Doom))
                    _tileInfo.towerInfo[TowerCategory.Doom] = 0;
                _tileInfo.towerInfo[TowerCategory.Doom]++;
                break;
        }

        Debug.Log($"타워 설치 {towerType}");

        // Tower Install Cost
        // 타워 설치 비용
        ResourceManager.Instance.Spend(bp.CostType, (float)bp.CostValue / 4);
        HUDCanvas.Instance.ShowTilePiece();

        hasTower = true;

        if (_collider == null)
            Debug.LogWarning($"Collider is missing on block {name}");
        else
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
        _tileInfo.hasTowerList.Remove(tower);

        string towerName = tower.towerdata.innerName;
        string towerType = towerName.Substring(1, towerName.IndexOf("_") - 1);

        if (_tileInfo == null)
            _tileInfo = GetComponentInParent<TileInfo>(true);

        switch (towerType)
        {
            case "Common":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Common))
                    _tileInfo.towerInfo[TowerCategory.Common] = 0;
                _tileInfo.towerInfo[TowerCategory.Common]--;
                break;
            case "Splash":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Splash))
                    _tileInfo.towerInfo[TowerCategory.Splash] = 0;
                _tileInfo.towerInfo[TowerCategory.Splash]--;
                break;
            case "Slow":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Slow))
                    _tileInfo.towerInfo[TowerCategory.Slow] = 0;
                _tileInfo.towerInfo[TowerCategory.Slow]--;
                break;
            case "Stun":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Stun))
                    _tileInfo.towerInfo[TowerCategory.Stun] = 0;
                _tileInfo.towerInfo[TowerCategory.Stun]--;
                break;
            case "Doom":
                if (!_tileInfo.towerInfo.ContainsKey(TowerCategory.Doom))
                    _tileInfo.towerInfo[TowerCategory.Doom] = 0;
                _tileInfo.towerInfo[TowerCategory.Doom]--;
                break;
        }

        Destroy(tower.gameObject);

        hasTower = false;

        if (_collider != null && !hasTower)
            _collider.enabled = true;
    }

    public void TowerUpgrade(Tower1 currentTower)
    {
        Debug.Log("Upgrade");
        Tower1 hasTower = GetComponentInChildren<Tower1>(true);
        if (hasTower != null)
        {
            Destroy(hasTower.gameObject);
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.37f);
            GameObject go = Instantiate(currentTower.gameObject, pos, Quaternion.identity);
            go.transform.SetParent(transform);
            Tower1 tower = go.GetComponent<Tower1>();
            tower.enabled = true;
            tower.rangeVisual.SetActive(false);
            Collider2D collider = go.GetComponent<Collider2D>();
            collider.enabled = true;
        }
        else
        {
            Debug.LogError("Upgrade fail");
        }
        
    }
}
