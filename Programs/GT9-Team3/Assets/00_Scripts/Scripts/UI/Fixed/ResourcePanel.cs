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

    public float HealthPercent => healthPercent;
    //private float healthPercent;  실제 데이터를 저장하는 필드

    //public float HealthPercent   외부에서 읽기만 가능한 프로퍼티
    //{
    //    get { return healthPercent; }
    //} 와 같은 내용


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

public enum ClearStar
{
    One,
    Two,
    Three,
}
