using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TileManager _tileManager;

    public Transform baseTransform;

    // ���� ����
    // GameLevel
    public int gameWorldLevel = 1;
    public int gameStageLevel = 1;
    public int gameRoundLevel = 1;
    public int gameWaveLevel = 1;
    [SerializeField] private TextMeshProUGUI gameWorldLevelText;
    [SerializeField] private TextMeshProUGUI gameStageLevelText;
    [SerializeField] private TextMeshProUGUI gameRoundLevelText;
    //[SerializeField] private TextMeshProUGUI gameWorldLevelText;

    // ���� �Ͻ����� �� �簳
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

        // ù Ÿ�� ��ġ
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



    // �ʱ� Ÿ�� ��ġ
    // Initial tile placement
    public void InitializeTiles()
    {
        // Ÿ�� ����
        _tileManager.Initialize();
        _tileManager.SetSpawnerPosition();
    }


    public void WaveStartButton()
    {
        WaveManager.Instance.StartWave();
    }


    // �� ���̽� ã�� (�����)
    // Find the enemy base transform in the scene
    public Vector3 BasePosition => baseTransform != null ? baseTransform.position : Vector3.zero;
}
