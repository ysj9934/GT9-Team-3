using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    private HUD_CastleHP castleHUD;

    public int health;
    public int maxHealth = 100;

    [SerializeField] private Slider healthGauge;
    [SerializeField] private TextMeshProUGUI healthText;

    public bool isDead;


    private void Awake()
    {
        castleHUD = HUD_Canvas.Instance.castleHUD;
    }

    public void Start()
    {
        health = maxHealth;
        castleHUD.UpdateHPbar(health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        
        castleHUD.UpdateHPbar(health, maxHealth);

        if (health <= 0)
        {
            DestroyBasement();
        }
    }

    public void DestroyBasement()
    {
        isDead = true;
        GameManager.Instance.gameDefeatPanel.SetActive(true);
    }

}
