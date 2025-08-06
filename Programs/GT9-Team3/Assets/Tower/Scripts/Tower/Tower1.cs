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
            cooldownTimer = 1f / data.attackSpeed; // ���� ���ݱ��� ��Ÿ��
        }
    }

    void Shoot(Transform target)
    {
        GameObject projectileObj = Instantiate(data.projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Initialize(target, data.projectileData);
    }

    //void Attack(Enemy1 target)
    //{
    //    Debug.Log($"[Ÿ�� ����] {target.name} ���� {data.damage} ������");
    //    target.TakeDamage(data.damage);
    //}

    public void ApplyData(TowerData towerData)
    {
        data = towerData;
        cooldownTimer = 0f;
    }
}
