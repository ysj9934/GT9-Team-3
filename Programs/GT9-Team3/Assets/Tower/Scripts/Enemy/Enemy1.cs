using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public Enemy enemy;

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
        enemy = GetComponentInParent<Enemy>();
    }

    public void TakeDamage(int damage)
    {
        //enemy.currentHp -= damage;
        //Debug.Log($"[피격] {gameObject.name} 체력: {enemy.currentHp}");
    }

    //private void Die()
    //{
    //    Debug.Log($"[사망] {gameObject.name}");
    //    Destroy(gameObject);
    //}

}
