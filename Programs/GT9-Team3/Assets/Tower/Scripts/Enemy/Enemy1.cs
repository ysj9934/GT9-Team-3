using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public Enemy _enemy;

    //[SerializeField] public int maxHP = 10;
    //private int currentHP;
    //public int CurrentHP => currentHP;

    public float DistanceToBase
    {
        get
        {
            return Vector3.Distance(transform.position, GameManager.Instance.BasePosition);
        }
    }

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    
    // ✅ 발사체 위치까지 받는 오버로드 (핸들러 직접 X, 부모 Enemy를 통해 호출)
    public void TakeDamage(int damage, ProjectileData projectileData, Vector3 hitOrigin)
    {
        _enemy.TakeDamage(damage, projectileData, hitOrigin);
    }

}
