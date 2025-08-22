using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TileManager _tileManager;

    public Transform baseTransform;

    // 게임 레벨
    // GameLevel
    public int gameWorldLevel = 1;
    public int gameStageLevel = 1;
    public int gameRoundLevel = 1;
    public int gameWaveLevel = 1;
    [SerializeField] private TextMeshProUGUI gameWorldLevelText;
    [SerializeField] private TextMeshProUGUI gameStageLevelText;
    [SerializeField] private TextMeshProUGUI gameRoundLevelText;
    //[SerializeField] private TextMeshProUGUI gameWorldLevelText;

    // 게임 일시정지 및 재개
    public bool isGamePaused = false;

    [SerializeField] public GameObject gameDefeatPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _tileManager = TileManager.Instance;

        if (gameDefeatPanel != null)
            gameDefeatPanel.SetActive(false);

        // 첫 타일 배치
        // first tile placement
        //InitializeTiles();
    }   


    public void DestroyOfType<T>() where T : Component
    {
        T[] objs = GameObject.FindObjectsOfType<T>();

        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }
    }



    // 초기 타일 배치
    // Initial tile placement
    public void InitializeTiles()
    {
        // 타일 생성
        _tileManager.Initialize();
        _tileManager.SetSpawnerPosition();
    }


    public void WaveStartButton()
    {
        WaveManager.Instance.StartWave();
    }


    // 적 베이스 찾기 (김원진)
    // Find the enemy base transform in the scene
    public Vector3 BasePosition => baseTransform != null ? baseTransform.position : Vector3.zero;
}
