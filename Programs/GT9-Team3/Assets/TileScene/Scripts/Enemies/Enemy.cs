using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public ObjectPoolManager _poolManager;

    public EnemyStat _enemyStat;
    public EnemyHealthHandler _enemyHealthHandler;
    public EnemyMovement _enemyMovement;
    public EnemyAnimationController _enemyAnimationController;

    public bool isAlive = false;

    private void Awake()
    {
        _poolManager = ObjectPoolManager.Instance;

        _enemyStat = GetComponent<EnemyStat>();
        _enemyHealthHandler = GetComponent<EnemyHealthHandler>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        
    }

    
}
