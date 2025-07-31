using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int maxHP = 10;
    private int currentHP;

    private void Start()
    {
        currentHP = maxHP; // 체력 초기화
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"[피격] {gameObject.name} 체력: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"[사망] {gameObject.name}");
        Destroy(gameObject);
    }
}
