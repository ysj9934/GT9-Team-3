using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower1 : MonoBehaviour
{
    public TowerData data;
    private float cooldownTimer;

    private void Update()
    {
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

    public void ApplyData(TowerData towerData)
    {
        data = towerData;
        cooldownTimer = 0f;
    }

    // 우선순위 
    private Enemy1 FindTarget()
    {


        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, data.attackRange);

        List<Enemy1> enemies = new List<Enemy1>();

        foreach (var hit in hits)
        {
            Enemy1 enemy = hit.GetComponent<Enemy1>();
            if (enemy != null)
            {
                Debug.Log($"[타워] 적 감지됨: {enemy.name}");
                enemies.Add(enemy);
            }
            else
            {
                Debug.Log($"[타워] Enemy1 컴포넌트 없음: {hit.name}");
            }
        }

        if (enemies.Count == 0)
        {
            Debug.Log("[타워] 범위 내 적 없음");
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
                case TargetPriority.Closest:
                    selected = enemies.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
                    break;
            }

            if (selected != null)
                Debug.Log($"[타워] 우선순위 {priority} 대상 선택됨: {selected.name}");
            return selected;
        }

        Debug.Log("[타워] 유효한 타겟 없음");
        return null;
    }


}
