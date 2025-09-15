using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    // Object Data
    private Castle _castle;
    [SerializeField] private Slider castleHealthSlider;
    [SerializeField] private TextMeshProUGUI castleHealthText;
    private float healthPercent;

    [SerializeField] TextMeshProUGUI resourceTilePieceAmountText;


    // Castle & Resource
    public Castle SetCastleData(Castle castle)
    {
        return _castle = castle;
    }

    public void UpdateHPBar()
    {
        castleHealthText.text = $"{_castle.currentHealth}/{_castle.maxHealth}";
        healthPercent = (float)_castle.currentHealth / _castle.maxHealth;
        castleHealthSlider.value = healthPercent;
    }

    public void ShowTilePiece()
    {
        resourceTilePieceAmountText.text = $"{ResourceManager.Instance.GetAmount(ResourceType.Tilepiece)}";
    }

}
