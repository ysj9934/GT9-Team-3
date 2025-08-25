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

    public void TakeDamage(int damage)
    {
        _enemy._enemyHealthHandler.TakeDamage(damage);
    }

}
