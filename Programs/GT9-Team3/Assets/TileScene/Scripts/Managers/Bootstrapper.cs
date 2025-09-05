using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private HUDCanvas hudCanvas;

    void Start()
    {
        waveManager.Init();
        tileManager.Init();
        hudCanvas.Init();
        gameManager.Init();

        gameManager.ReceiveStageData();
    }
}
