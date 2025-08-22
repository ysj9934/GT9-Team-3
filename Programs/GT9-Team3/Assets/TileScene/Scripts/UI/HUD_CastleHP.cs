using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_CastleHP : MonoBehaviour
{
    public Castle castle;
    [SerializeField] private Slider hpbar;
    [SerializeField] private TextMeshProUGUI hpbarText;

    public void Initialize(Castle castle)
    {
        this.castle = castle;
    }

    public void UpdateHPbar(int currentHealth, int maxHealth)
    {
        hpbarText.text = $"{currentHealth}/{maxHealth}";

        float healthPercent = 0f;

        healthPercent = currentHealth / maxHealth;

        hpbar.value = healthPercent;
    }




}
