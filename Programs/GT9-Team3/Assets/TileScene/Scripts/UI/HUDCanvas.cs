using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDCanvas : MonoBehaviour
{
    public GameManager _gameManager;

    public static HUDCanvas Instance { get; private set; }

    // StageInfoHUD
    [SerializeField] private TextMeshProUGUI worldPanelText;
    [SerializeField] private TextMeshProUGUI stagePanelText;
    [SerializeField] private TextMeshProUGUI roundPanelText;

    // Castle & Resource
    private Castle _castle;
    [SerializeField] private Slider castleHealthSlider;
    [SerializeField] private TextMeshProUGUI castleHealthText;
    private float healthPercent;

    [SerializeField] TextMeshProUGUI resourceTilePieceAmountText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateTilePiece();
    }

    // StageInfoHUD
    public void ReceiveStageData(StageDataToHUD stageData)
    {
        if (stageData != null)
        {
            worldPanelText.text = $"{stageData.worldCode}";
            stagePanelText.text = $"{stageData.stageCode}";
            roundPanelText.text = $"{stageData.roundCode}";
        }
        else
        {
            Debug.LogError("StageData is Null");
        }
    }

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

    public void UpdateTilePiece()
    {
        resourceTilePieceAmountText.text = $"{ResourceManager.Instance.showGold()}";
    }
}
