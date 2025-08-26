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
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direaction = (target.position - transform.position).normalized;
        transform.position += direaction * data.speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        if(distance < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        Enemy1 enemy = target.GetComponent<Enemy1>();
        if (enemy != null)
        {
            enemy.TakeDamage(data.damage, data);
        }

        if(data.impactEffectPrefab)
        {
            Instantiate(data.impactEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
