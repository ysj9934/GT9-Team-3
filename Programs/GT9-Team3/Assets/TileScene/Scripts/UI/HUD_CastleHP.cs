using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_CastleHP : MonoBehaviour
{
    public ResourceManager _resourceManager;
    public Castle castle;
    [SerializeField] private Slider hpbar;
    [SerializeField] private TextMeshProUGUI hpbarText;
    private float healthPercent = 0f;

    [SerializeField] private TextMeshProUGUI tilePieceText;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
        UpdateGold();
    }

    public void Initialize(Castle castle)
    {
        this.castle = castle;
    }

    public void UpdateHPbar()
    {
        hpbarText.text = $"{castle.currentHealth}/{castle.maxHealth}";

        healthPercent = (float)castle.currentHealth / castle.maxHealth;

        hpbar.value = healthPercent;
    }

    public void UpdateTilePiece()
    {
        tilePieceText.text = $"{_resourceManager.ShowTilePiece()}";
    }

    public void UpdateGold()
    {
        tilePieceText.text = $"{_resourceManager.showGold()}";
    }




}
