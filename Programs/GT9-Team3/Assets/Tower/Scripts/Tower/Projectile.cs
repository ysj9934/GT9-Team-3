using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileData data;
    private Transform target;

    public void Initialize(Transform target, ProjectileData projectileData)
    {
        this.target = target;
        this.data = projectileData;
        Destroy(gameObject, data.lifetime);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direaction = (target.position - transform.position).normalized;
        transform.position += direaction * data.speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {

        if (data.impactEffectPrefab)
        {
            Instantiate(data.impactEffectPrefab, transform.position, Quaternion.identity);
        }

        // 스플래쉬 공격
        if (data.impactRadius > 0f)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, data.impactRadius, LayerMask.GetMask("Enemy"));
            Debug.Log($"스플래시 타격: {hitEnemies.Length}명 감지됨");

            foreach (var collider in hitEnemies)
            {
                Enemy1 enemy = collider.GetComponent<Enemy1>();
                if (enemy != null)
                {
                    enemy.TakeDamage(data.damage, data);
                    Debug.Log($"스플래시 적중: {enemy.name} / 데미지: {data.damage}");
                }

            }
        }
        else
        {
            Enemy1 enemy = target.GetComponent<Enemy1>();
            if (enemy != null)
            {
                enemy.TakeDamage(data.damage, data);
            }
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (data != null && data.impactRadius > 0f)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.impactRadius);
        }
    }
#endif

}
