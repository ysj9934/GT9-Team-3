using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower1 : MonoBehaviour
{
    public TowerData data;
    private float cooldownTimer;

    private GameObject rangeVisual;

    private void Awake()
    {
        rangeVisual = transform.Find("RangeVisual")?.gameObject;
    }
    
    public void Intialize(BlockInfo blockInfo)
    {
        
    }

    private void Update()
    {

        if (data == null) return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        Enemy1 target = FindTarget();
        if (target != null)
        {
            Attack(target);
            cooldownTimer = 1f / data.attackSpeed;
        }

    }

    public void Shoot(Transform target)
    {
        GameObject projectileObj = Instantiate(data.projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Initialize(target, data.projectileData);
    }

    public void Attack(Enemy1 target)
    {
        if (data.projectilePrefab != null && data.projectileData != null)
        {
            GameObject projGO = Instantiate(data.projectilePrefab, transform.position, Quaternion.identity);
            Projectile proj = projGO.GetComponent<Projectile>();
            proj.Initialize(target.transform, data.projectileData);
        }
        else
        {
            Debug.LogWarning("[타워] 발사체 프리팹 또는 데이터가 연결되지 않았습니다.");
        }
    }

    public void ApplyData(TowerData d)
    {
        data = d;
        cooldownTimer = 0f;

        if (rangeVisual != null)
        {
            float range = data.attackRange * 2f;
            rangeVisual.transform.localScale = new Vector3(range, range, 1f);
            rangeVisual.SetActive(false);   // 처음엔 숨김
        }

        Debug.Log($"[타워] 스탯 적용됨: 고유번호 = {data.towerID},  이름 = {data.innerName}");
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
    }

    // 우선순위 
    private Enemy1 FindTarget()
    {

        // 적 탐색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, data.attackRange);

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

        foreach (var priority in data.targetOrder)
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
                    selected = enemies.OrderBy(e => e.CurrentHP).FirstOrDefault();
                    break;
                case TargetPriority.Base_Closest:
                    selected = enemies.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
                    break;
            }

            if (selected != null)
            {
                return selected;
            }
        }

        return null;
    }


}