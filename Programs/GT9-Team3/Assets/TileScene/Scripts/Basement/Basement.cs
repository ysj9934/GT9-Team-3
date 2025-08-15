using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Basement : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    [SerializeField] private Slider healthGauge;
    [SerializeField] private TextMeshProUGUI healthText;

    public void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;


        healthGauge.value = health / maxHealth;
        healthText.text = $"{health.ToString()}/{maxHealth.ToString()}";

        if (health <= 0)
        {
            DestroyBasement();
        }
    }

    public void DestroyBasement()
    { 
        GameManager.Instance.gameDefeatPanel.SetActive(true);
    }

}
