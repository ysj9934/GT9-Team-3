using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1 : MonoBehaviour
{
    public ProjectileData data;
    private Transform target;

    public void Initialize(ProjectileData projectileData, Transform targetTransform)
    {
        data = projectileData;
        target = targetTransform;

        Destroy(gameObject, data.lifetime);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * data.speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (data.impactEffectPrefab != null)
        {
            Instantiate(data.impactEffectPrefab, transform.position, Quaternion.identity);
        }

        Enemy1 enemy = target.GetComponent<Enemy1>();
        if(enemy != null)
        {
            enemy.TakeDamage(data.damage);
        }

        Destroy(gameObject);
    }
}
