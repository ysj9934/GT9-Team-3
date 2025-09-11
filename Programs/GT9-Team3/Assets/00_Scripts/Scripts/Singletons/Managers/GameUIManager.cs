using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    public TowerSellUI towerSellUI;
    public HUDCanvas _hudCanvas;

    private void Awake()
    {
        Instance = this;

        towerSellUI = GetComponentInChildren<TowerSellUI>();

    }
}
