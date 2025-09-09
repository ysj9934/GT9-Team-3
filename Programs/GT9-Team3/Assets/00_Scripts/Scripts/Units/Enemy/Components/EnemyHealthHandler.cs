using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour
{
    // Object Structure
    private Enemy _enemy;

    public event Action OnDeath;

    public float currentHealth;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// 체력 초기화
    /// </summary>
    public void InitializeHealth()
    {
        currentHealth = _enemy._enemyStat.enemyMaxHP;
    }

    /// <summary>
    /// 성 피격시 처리
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Castle castle = collision.GetComponent<Castle>();

        if (castle != null)
        {
            // 성에 도달했을 때 처리
            castle.TakeDamage((int)_enemy._enemyStat.enemyAttackDamage);
            Debug.Log("castle touch");
            EnemyDeath(HitTarget.Castle);
        }
    }

    /// <summary>
    /// 적 피격시 처리
    /// </summary>
    public void TakeDamage(int damage, ProjectileData projectileData)
    {
        if (!_enemy.isAlive) return;

        currentHealth -= damage;

        // 투사체 피격
        if (projectileData != null)
        {
            // 슬로우
            if (projectileData.slowEffect > 0)
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
            EnemyDeath(HitTarget.Projectile);
        }
    }

    private void EnemyDeath(HitTarget target)
    {
        OnDeath?.Invoke();
        GameManager.Instance._waveManager.activeEnemies.Remove(this.gameObject);
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

        EnemyManager.Instance.ReturnEnemy(this.gameObject);

        _enemy.isAlive = false;
    }


    /// <summary>
    /// 적 유닛 체력 회복
    /// </summary>
    public void EnemyHeal(float value, bool isShare)
    {
        if (!_enemy.isAlive) return;

        float maxHealth = _enemy._enemyStat.enemyMaxHP;


        // 체력 회복 사운드의 경우 단일기와 단체기별 사운드 분리

        // [이펙트효과]: 체력 회복 이펙트
        Debug.LogWarning("[Effect] Heal Effect");

        if (isShare)
        {
            // [사운드효과]: 체력 회복 사운드
            Debug.LogWarning("[Sound] Heal Sound");
        }

        // 최대체력이상으로 회복 불가
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}


