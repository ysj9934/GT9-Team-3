using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Object Structure
    public EnemyStat _enemyStat;
    public EnemyHealthHandler _enemyHealthHandler;
    public EnemyMovement _enemyMovement;
    public EnemyAnimationController _enemyAnimationController;

    public Dictionary<SpriteRenderer, int> originSpriteOrder = new Dictionary<SpriteRenderer, int>();
    public SpriteRenderer[] spriteRenderers;


    // Object Info
    public bool isAlive = false;

    private void Awake()
    {
        _enemyStat = GetComponent<EnemyStat>();
        _enemyHealthHandler = GetComponent<EnemyHealthHandler>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void LateUpdate()
    {
        if (!isAlive) return;

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            int baseOrder = originSpriteOrder.ContainsKey(sr) ? originSpriteOrder[sr] : 0;
            sr.sortingOrder = baseOrder + Mathf.RoundToInt(-transform.position.y * 100);
        }
    }

    public EnemyAnimationController SetAnimationController(EnemyAnimationController enemyAni)
    {
        return _enemyAnimationController = enemyAni;
    }

    
}
