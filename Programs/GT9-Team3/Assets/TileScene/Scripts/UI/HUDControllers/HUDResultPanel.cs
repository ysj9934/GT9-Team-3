using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDResultPanel : MonoBehaviour
{
    private HUDCanvas _hudCanvas;

    // DefeatPanel
    public GameDefeat _gameDefeatPanel;

    // ResultPanel
    public GameResult _gameResultPanel;

    private void Start()
    {
        _hudCanvas = GetComponentInParent<HUDCanvas>();

        _gameDefeatPanel = GetComponentInChildren<GameDefeat>();
        _gameDefeatPanel.Initialize(_hudCanvas);
        _gameResultPanel = GetComponentInChildren<GameResult>();
        _gameResultPanel.Initialize(_hudCanvas);
    }
}
