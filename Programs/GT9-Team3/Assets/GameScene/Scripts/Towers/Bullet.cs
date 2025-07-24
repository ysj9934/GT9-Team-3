using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private ObjectPool<Transform> _pool;
    private Tower _tower;
    
    public float damage;        // bullet damage
    public int per;             // bullet piercing count

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 direction, ObjectPool<Transform> pool, Tower tower)
    {
        this.damage = damage;
        this.per = per;
        this._pool = pool;
        this._tower = tower;

        if (per > -1)
        {
            _rigid.velocity = direction * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy == null || per == -1) return;

        per--;

        if (per == -1)
        {
            _rigid.velocity = Vector2.zero;
            _pool.ReturnToPool(transform);
            _tower._activeBullets.Remove(transform);
            // gameObject.SetActive(false);
        }
    }
}
