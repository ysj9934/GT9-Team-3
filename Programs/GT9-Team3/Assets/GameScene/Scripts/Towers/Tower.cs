using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ?ºÎ∞ò???Ä???§Ìéô?ÖÎãà??
public class Tower : MonoBehaviour
{
    [Header("Bullet Info")]
    [SerializeField] private GameObject _projectile;            // bullet prefab
    [SerializeField] private Transform _projectileParent;       // bullet parent
    [SerializeField] private Scanner _scanner;                  // search enemy
    private ObjectPool<Transform> _bulletPool;                  // bullet objectPool
    public List<Transform> _activeBullets = new List<Transform>();  // bullet active list
    [SerializeField] private const int maxBullets = 10;         // bullet maxObejctPool
    // [SerializeField] private int count = 1;
    
    [Header("Tower Stat Info")]
    [SerializeField] private float damage = 2;                  // tower damage
    [SerializeField] private int per = 0;                       // tower piercing count (default = 0)
    [SerializeField] private float attackDelay = 1f;            // tower autoReload (n <<< 0)
    
    private float timer = 0f;
    
    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
    }

    private void Start()
    {
        _bulletPool = new ObjectPool<Transform>(_projectile.transform, maxBullets, _projectileParent);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackDelay)
        {
            timer = 0;
            Fire();
        }
    }

    // Ï¥ùÏïå Î∞úÏÇ¨
    private void Fire()
    {
        // ?§Ï∫ê?àÏóê ?ÅÏùÑ ?¨Ï∞© / ?Ä?åÏùò ÏµúÎ? Ï¥ùÏïå Í∞?àò(ObjectPool)
        if (_scanner.nearestTarget == null || _activeBullets.Count >= maxBullets)
            return;

        Transform bullet = _bulletPool.Get();
        if (bullet == null) return;
        
        _activeBullets.Add(bullet);
        
        // ?ÅÍ≥º ?Ä?åÏùò ?ÑÏπò Í≥ÑÏÇ∞
        Vector3 targetPos = _scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;
        
        bullet.position = transform.position;
        // bullet.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
        // bullet.rotation = Quaternion.LookRotation(dir);

        bullet.GetComponent<Bullet>().Init(damage, per, dir, _bulletPool, this);
    }
}
