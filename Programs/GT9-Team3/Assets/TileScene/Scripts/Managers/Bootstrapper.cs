using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private HUDCanvas hudCanvas;
    //[SerializeField] private EnemyConfigController enemyConfigManager;
    [SerializeField] private EnemyManager objectPoolManager;

    void Start()
    {
        gameManager.Init();
        //enemyConfigManager.Init();
        objectPoolManager.Init();
        tileManager.Init();
        hudCanvas.Init();
        waveManager.Init();
        
        

        gameManager.ReceiveStageData();
    }
}
