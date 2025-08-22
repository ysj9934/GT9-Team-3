using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public HUD_CastleHP castleHUD;

    public int currentHealth;
    public int maxHealth = 100;

    public bool isDead;


    private void Awake()
    {
        
    }

    public void Start()
    {
        currentHealth = maxHealth;
        castleHUD = HUD_Canvas.Instance.castleHUD;
        if (castleHUD != null)
            castleHUD.Initialize(this);
            castleHUD.UpdateHPbar();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        
        castleHUD.UpdateHPbar();

        if (currentHealth <= 0)
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
