using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
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
        _hudCanvas = _gameManager._hudCanvas;

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
        else if (_hudCanvas == null)
        {
            ValidateMessage(_hudCanvas.name);
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
        _hudCanvas.SetCastleData(this);
    }

    public void ResetButton()
    {
        currentHealth = maxHealth;
        _hudCanvas.UpdateHPBar();
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        _hudCanvas.UpdateHPBar();

        if (currentHealth <= 0)
        {
            DestroyBasement();
        }
    }

    public void DestroyBasement()
    {
        isDead = true;

        Debug.Log("GameOver");

        _gameManager.PauseGame();
        _gameManager._waveManager.StopWave();
        HUDCanvas.Instance._gameDefeatPanel.OpenWindow();
    }

}
