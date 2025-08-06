using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower1 : MonoBehaviour
{
    public TowerData data;
    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (cooldownTimer > 0f) return;

        Enemy1 enemy = collision.GetComponent<Enemy1>();
        if (enemy != null)
        {
            Shoot(enemy.transform);
            cooldownTimer = 1f / data.attackSpeed; // 다음 공격까지 쿨타임
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
}
