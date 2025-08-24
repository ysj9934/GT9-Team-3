using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TileManager _tileManager;
    public ObjectPoolManager _poolManager;
    public DataManager _dataManager;

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
        _poolManager = ObjectPoolManager.Instance;
        _dataManager = DataManager.Instance;

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

    // TEMP
    // �� ����
    public void SpanwEnemy(Enemy_DataTable_EnemyStatTable jsonData)
    {
        GameObject enemyObj = _poolManager.GetEnemy();
        if (enemyObj != null)
        {
            //var config = EnemyConfigManager.Instance.CreateConfigFromJson(jsonData);
            //enemyObj.GetComponent<EnemyTEMP>().Setup(config);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Spawn Test Enemy")]
    void SpawnTestEnemy()
    {
        GameObject enemyObj = _poolManager.GetEnemy();
        if (enemyObj != null)
        {
            var testJson = new Enemy_DataTable_EnemyStatTable
            {
                key = 1000,
                Enemy_Inner_Name = "���ٴϴ� ������",
                MaxHP = 100,
                MovementSpeed = 3.5f
            };

            //var config = EnemyConfigManager.Instance.CreateConfigFromJson(testJson);
            //var enemy = Instantiate(config.enemyPrefab);
            //enemyObj.GetComponent<EnemyTEMP>().Setup(config);
        }
    }
#endif



    // �� ���̽� ã�� (�����)
    // Find the enemy base transform in the scene
    public Vector3 BasePosition => baseTransform != null ? baseTransform.position : Vector3.zero;
}
