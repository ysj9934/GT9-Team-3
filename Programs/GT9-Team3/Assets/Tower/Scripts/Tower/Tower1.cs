using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower1 : MonoBehaviour
{
    public BlockInfo blockInfo;
    public TowerData towerdata;
    public ProjectileData projectileData;
    private float cooldownTimer;

    private GameObject rangeVisual;

    public TowerBlueprint blueprint;

    [SerializeField] private SpriteRenderer towerSprite;         // 타워 이미지
    [SerializeField] private List<Sprite> levelSprites;          // 레벨별 스프라이트
    [SerializeField] private GameObject auraEffect;              // 오로라 이펙트 오브젝트 (4레벨 이상에만 표시)

    public int level;

    private void Awake()
    {
        rangeVisual = transform.Find("RangeVisual")?.gameObject;
    }

    public void Intialize(BlockInfo blockInfo)
    {
        this.blockInfo = blockInfo;
    }

    private void Update()
    {

        if (towerdata == null) return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        Enemy1 target = FindTarget();
        if (target != null)
        {
            Attack(target);
            cooldownTimer = 1f / towerdata.attackSpeed;
        }

    }

    public void Shoot(Transform target)
    {
        GameObject projectileObj = Instantiate(towerdata.projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Initialize(target, towerdata.projectileData);
    }

    public void Attack(Enemy1 target)
    {
        if (towerdata.projectilePrefab != null && towerdata.projectileData != null)
        {
            GameObject projGO = Instantiate(towerdata.projectilePrefab, transform.position, Quaternion.identity);
            Projectile proj = projGO.GetComponent<Projectile>();
            proj.Initialize(target.transform, towerdata.projectileData);
        }
        else
        {
            Debug.LogWarning("[타워] 발사체 프리팹 또는 데이터가 연결되지 않았습니다.");
        }
    }
    public void ShowAttackRange()
    {
        if (rangeVisual != null)
            rangeVisual.SetActive(true);
    }

    public void ApplyData(TowerData d)
    {
        towerdata = Instantiate(d);
        cooldownTimer = 0f;

        if (d.projectileData != null)
        {
            towerdata.projectileData = Instantiate(d.projectileData);
        }

        if (rangeVisual != null)
        {
            float range = towerdata.attackRange * 2f;
            rangeVisual.transform.localScale = new Vector3(range, range, 1f);
            rangeVisual.SetActive(false);   // 처음엔 숨김
        }

        Debug.Log($"[타워] 스탯 적용됨: 고유번호 = {towerdata.towerID},  이름 = {towerdata.innerName}");
    }

    public void ApplyData(TowerBlueprint bp)
    {
        blueprint = bp;

        ApplyData(bp.data);
    }

    private void OnMouseDown()
    {

        bool isAlreadyOpen = TowerSellUI.Instance.IsOpenFor(this);

        // UI 열려있고 같은 타워를 누른 경우 닫기
        if (isAlreadyOpen)
        {
            TowerSellUI.Instance.Hide();

            if (rangeVisual != null)
                rangeVisual.SetActive(false);

            return;
        }

        // 다른 타워거나 처음 열리는 경우 기존 UI 닫고 새로 열기
        TowerSellUI.Instance.Show(this);

        if (rangeVisual != null)
            rangeVisual.SetActive(true);

        // 업그레이드 정보
        TowerUpgradeUI ui = FindObjectOfType<TowerUpgradeUI>();
        if (ui != null)
        {
            ui.SetTargetTower(this);
        }
    }

    // 우선순위 
    private Enemy1 FindTarget()
    {

        // 적 탐색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerdata.attackRange);

        List<Enemy1> enemies = new List<Enemy1>();

        foreach (var hit in hits)
        {

            if (hit.gameObject == gameObject) continue;     // 자기자신 제외

            Enemy1 enemy = hit.GetComponent<Enemy1>();

            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }

        if (enemies.Count == 0)
        {
            return null;
        }

        foreach (var priority in towerdata.targetOrder)
        {
            Enemy1 selected = null;

            switch (priority)
            {
                case TargetPriority.Boss:
                    selected = enemies.Find(e => e.CompareTag("Boss"));
                    break;
                case TargetPriority.Base_Range:
                    selected = enemies.OrderBy(e => e.DistanceToBase).FirstOrDefault();
                    break;
                case TargetPriority.Lowest_HP:
                    selected = enemies.OrderBy(e => e._enemy._enemyHealthHandler.currentHealth).FirstOrDefault();
                    break;
                case TargetPriority.Base_Closest:
                    selected = enemies.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
                    break;
                default:
                    selected = enemies.OrderBy(e => e._enemy._enemyHealthHandler.currentHealth).FirstOrDefault();
                    break;
            }

            if (selected != null)
            {
                return selected;
            }
        }

        return null;
    }

    // 타워 업그레이드
    public bool CanUpgrade()
    {
        return towerdata.UpgradeCost != ResourceType.Null && towerdata.UpgradeValue > 0;
    }

    public bool HasEnoughResources()
    {
        return ResourceManager.Instance.CanAfford(towerdata.UpgradeCost, towerdata.UpgradeValue);
    }

    public bool TryUpgrade()
    {
        if (!CanUpgrade())
        {
            Debug.Log("최대 레벨입니다.");
            return false;
        }

        if (!HasEnoughResources())
        {
            Debug.Log("자원이 부족합니다!");
            return false;
        }

        // 리소스 차감
        ResourceManager.Instance.Spend(towerdata.UpgradeCost, towerdata.UpgradeValue);
        HUDCanvas.Instance.ShowTilePiece();
            
        // ID 증가 및 데이터 갱신
        int nextTowerID = towerdata.towerID + 1;
        int nextProjectileID = towerdata.projectileData.projectileID + 1;

        var towerTable = TowerDataTableLoader.Instance.ItemsDict;
        var projectileTable = ProjectileDataLoader.Instance.ItemsDict;

        // Tower 업그레이드
        if (towerTable.TryGetValue(nextTowerID, out var newTowerRow))
        {
            TowerDataMapper.ApplyToSO(towerdata, newTowerRow);

            // Projectile 업그레이드
            if (projectileTable.TryGetValue(nextProjectileID, out var newProjRow))
            {
                if (towerdata.projectileData == null)
                    towerdata.projectileData = ScriptableObject.CreateInstance<ProjectileData>();

                ProjectileDataMapper.ApplyToSO(towerdata.projectileData, newProjRow);

                Debug.Log($"[ProjectileUpgrade] 성공 → ID: {nextProjectileID}");
            }
            else
            {
                Debug.LogWarning($"[ProjectileUpgrade] ID {nextProjectileID}에 해당하는 데이터 없음");
            }

            Debug.Log($"[TowerUpgrade] 성공 → ID: {nextTowerID}");

            
            return true;
        }
        else
        {
            Debug.LogWarning($"[TowerUpgrade] ID {nextTowerID}에 해당하는 데이터 없음");
            return false;
        }

        
    }

    // 업그레이드 이미지 갱신
    public void UpdateTowerVisual(int level)
    {
        if (level - 1 < levelSprites.Count)
        {
            towerSprite.sprite = levelSprites[level - 1];
        }

        if (auraEffect != null)
        {
            auraEffect.SetActive(level >= 4);
        }
    }

    public Sprite GetCurrentTowerSprite()
    {
        return towerSprite.sprite;
    }

}