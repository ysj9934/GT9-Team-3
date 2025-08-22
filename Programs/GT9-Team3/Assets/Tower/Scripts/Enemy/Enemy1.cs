using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] public int maxHP = 10;
    private int currentHP;
    public int CurrentHP => currentHP;

    public float DistanceToBase
    {
        get
        {
            return Vector3.Distance(transform.position, GameManager.Instance.BasePosition);
        }
    }

    private void Start()
    {
        currentHP = maxHP; // ü�� �ʱ�ȭ
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"[�ǰ�] {gameObject.name} ü��: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"[���] {gameObject.name}");
        Destroy(gameObject);
    }

}
