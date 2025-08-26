using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.EditorTools;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour
{
    private Enemy _enemy;

    public float currentHealth;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void InitializeHealth()
    {
        currentHealth = _enemy._enemyStat.enemyMaxHP;
    }

    // 예제 용도: 마우스 클릭으로 데미지 입히기
    private void OnMouseDown()
    {
        if (!_enemy.isAlive) return;

        Debug.Log($"Enemy Clicked: {_enemy._enemyStat.enemyName}");

        TakeDamage(1000, null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Castle castle = collision.GetComponent<Castle>();
        //Projectile projectile = collision.GetComponent<Projectile>();

        if (castle != null)
        {
            // 성에 도달했을 때 처리
            castle.TakeDamage((int)_enemy._enemyStat.enemyAttackDamage);
            DeathMotion(HitTarget.Castle);
        }
        //else if (projectile != null)
        //{
        //    // 투사체에 맞았을 때 처리
        //    TakeDamage(projectile.data.damage);
        //    //projectile.Hit();
        //}
    }

    public void TakeDamage(int damage, ProjectileData projectileData)
    {
        // 슬로우 적용전에 적이 죽으면 에러
        if (!_enemy.isAlive) return;

        currentHealth -= damage;

        if (_enemy.isAlive)
        {
            if (projectileData != null && projectileData.slowEffect > 0)
            {
                _enemy._enemyMovement.ApplySlow(projectileData.slowEffect, projectileData.slowTime);
            }

            // 스턴 미구현
            //if (projectileData != null && projectileData.stunTime > 0)
            //{
            //    _enemy._enemyMovement.ApplyStun(projectileData.stunTime);
            //}
        }

        if (currentHealth <= 0)
        {
            DeathMotion(HitTarget.Projectile);
        }
    }

    private void DeathMotion(HitTarget target)
    {
        _enemy.isAlive = false;

        switch (target)
        { 
            case HitTarget.Castle:
                Debug.Log($"{_enemy._enemyStat.enemyName} has reached the castle and is destroyed.");
                break;
            case HitTarget.Projectile:
                Debug.Log($"{_enemy._enemyStat.enemyName} has reached the castle and is destroyed.");

                // 처치 보상
                ResourceManager.Instance.Earn(ResourceType.Gold, _enemy._enemyStat.enemyTilePieceAmount);
                HUD_Canvas.Instance.castleHUD.UpdateGold();
                break;
        }

        _enemy._poolManager.ReturnEnemy(this.gameObject);
    }

}


