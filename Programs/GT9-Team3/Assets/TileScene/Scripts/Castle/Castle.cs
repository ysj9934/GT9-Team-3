using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public GameManager _gameManager;
    public HUDCanvas _hudCanvas;

    public int currentHealth;
    public int maxHealth = 100;

    public bool isDead;

    private void Awake()
    {
        _gameManager = GameManager.Instance;

        if (IsValidate())
        {
            GetCastleData();
            ResetButton();
        }
    }
    private bool IsValidate()
    {
        if (_gameManager == null)
        {
            ValidateMessage(_gameManager.name);
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ValidateMessage(string obj)
    {
        Debug.LogError($"{obj} is Valid");
    }

    public void GetCastleData()
    {
        HUDCanvas.Instance._hudResource.SetCastleData(this);
    }

    public void ResetButton()
    {
        currentHealth = maxHealth;
        HUDCanvas.Instance._hudResource.UpdateHPBar();
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        HUDCanvas.Instance._hudResource.UpdateHPBar();

        if (currentHealth <= 0)
        {
            DestroyBasement();
        }
    }

    public void DestroyBasement()
    {
        isDead = true;

        _gameManager._waveController.StopWave();

        Debug.Log("GameOver");

        _gameManager.PauseGame();
        _gameManager.isGameOver = true;
        
        HUDCanvas.Instance._hudResultPanel._gameDefeatPanel.OpenWindow();
    }

}
