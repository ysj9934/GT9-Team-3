using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower_Tile : MonoBehaviour
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
            Debug.LogWarning("[Ÿ��] �߻�ü ������ �Ǵ� �����Ͱ� ������� �ʾҽ��ϴ�.");
        }
    }

    public void ApplyData(TowerData towerData)
    {
        data = towerData;
        cooldownTimer = 0f;
    }

    // �켱���� 
    private Enemy1 FindTarget()
    {


        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, data.attackRange);

        List<Enemy1> enemies = new List<Enemy1>();

        foreach (var hit in hits)
        {
            Enemy1 enemy = hit.GetComponent<Enemy1>();
            if (enemy != null)
            {
                Debug.Log($"[Ÿ��] �� ������: {enemy.name}");
                enemies.Add(enemy);
            }
            else
            {
                Debug.Log($"[Ÿ��] Enemy1 ������Ʈ ����: {hit.name}");
            }
        }

        if (enemies.Count == 0)
        {
            Debug.Log("[Ÿ��] ���� �� �� ����");
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
                Debug.Log($"[Ÿ��] �켱���� {priority} ��� ���õ�: {selected.name}");
            return selected;
        }

        Debug.Log("[Ÿ��] ��ȿ�� Ÿ�� ����");
        return null;
    }
}
