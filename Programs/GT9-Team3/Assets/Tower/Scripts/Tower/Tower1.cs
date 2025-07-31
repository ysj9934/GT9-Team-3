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
            Attack(enemy);
            cooldownTimer = 1f / data.attackSpeed; // 다음 공격까지 쿨타임
        }
    }

    void Attack(Enemy1 target)
    {
        Debug.Log($"[타워 공격] {target.name} 에게 {data.damage} 데미지");
        target.TakeDamage(data.damage);
    }

    public void ApplyData(TowerData towerData)
    {
        data = towerData;
        cooldownTimer = 0f;
    }
}
