using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDResultPanel : MonoBehaviour
{
    private HUDCanvas _hudCanvas;

    // DefeatPanel
    [SerializeField] public GameDefeat _gameDefeatPanel;

    // ResultPanel
    [SerializeField] public GameResult _gameResultPanel;

    private void Start()
    {
        _hudCanvas = GetComponentInParent<HUDCanvas>();

    }
}
