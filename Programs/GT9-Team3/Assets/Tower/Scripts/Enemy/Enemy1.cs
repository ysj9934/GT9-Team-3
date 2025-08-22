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

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(int damage)
    {
        enemy.currentHp -= damage;
        Debug.Log($"[�ǰ�] {gameObject.name} ü��: {enemy.currentHp}");
    }

    //private void Die()
    //{
    //    Debug.Log($"[���] {gameObject.name}");
    //    Destroy(gameObject);
    //}

}
