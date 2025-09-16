using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;

    public bool isDead;

    private void Awake()
    {
        GetCastleData();
        ResetButton();
    }

    private void ValidateMessage(string obj)
    {
        Debug.LogError($"{obj} is Valid");
    }

    public void GetCastleData()
    {
        GameUIManager.Instance.canvasFixed.ResourcePanel.SetCastleData(this);
    }

    public void ResetButton()
    {
        currentHealth = maxHealth;
        GameUIManager.Instance.canvasFixed.ResourcePanel.UpdateHPBar();
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        GameUIManager.Instance.canvasFixed.ResourcePanel.UpdateHPBar();

        if (currentHealth <= 0)
        {
            DestroyBasement();
        }
    }

    public void DestroyBasement()
    {
        isDead = true;

        GameManager.Instance.GameDefeat();
    }

}
