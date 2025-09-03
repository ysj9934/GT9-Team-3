using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour
{
    private Enemy _enemy;

    public event Action OnDeath;

    public float currentHealth;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void InitializeHealth()
    {
        currentHealth = _enemy._enemyStat.enemyMaxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Castle castle = collision.GetComponent<Castle>();

        if (castle != null)
        {
            // 성에 도달했을 때 처리
            castle.TakeDamage((int)_enemy._enemyStat.enemyAttackDamage);
            Debug.Log("castle touch");
            DeathMotion(HitTarget.Castle);
        }
    }

    public void TakeDamage(int damage, ProjectileData projectileData)
    {
        // 슬로우 적용전에 적이 죽으면 에러
        if (!_enemy.isAlive) return;

        currentHealth -= damage;

        if (_enemy.isAlive)
        {
            // 슬로우
            if (projectileData != null && projectileData.slowEffect > 0)
            {
                _enemy._enemyMovement.ApplySlow(projectileData.slowEffect, projectileData.slowTime);
            }

            // 스턴
            if (projectileData != null && projectileData.stunTime > 0)
            {
                _enemy._enemyMovement.ApplyStun(projectileData.stunTime);
            }
        }

        if (currentHealth <= 0)
        {
            DeathMotion(HitTarget.Projectile);
        }
    }

    private void DeathMotion(HitTarget target)
    {
        _enemy.isAlive = false;

        OnDeath?.Invoke();
        _enemy._gameManager._waveManager.activeEnemies.Remove(this.gameObject);
        OnDeath = null;

        switch (target)
        { 
            case HitTarget.Castle:
                Debug.Log($"{_enemy._enemyStat.enemyName} has reached the castle and is destroyed.");
                break;
            case HitTarget.Projectile:
                Debug.Log($"{_enemy._enemyStat.enemyName} has reached the castle and is destroyed.");

                // 처치 보상
                ResourceManager.Instance.Earn(ResourceType.Tilepiece, _enemy._enemyStat.enemyTilePieceAmount);
                HUDCanvas.Instance._hudResource.ShowTilePiece();
                break;
        }

        _enemy._poolManager.ReturnEnemy(this.gameObject);
        
    }

    public void EnemyHeal(float value)
    {
        float maxHealth = _enemy._enemyStat.enemyMaxHP;

        if (!_enemy.isAlive) return;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}


