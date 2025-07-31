using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int maxHP = 10;
    private int currentHP;

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
