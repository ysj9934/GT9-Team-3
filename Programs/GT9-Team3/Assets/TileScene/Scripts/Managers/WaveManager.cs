using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    private GameManager _gameManager;

    [SerializeField] public GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 1f; // Time between spawns in seconds
    private int waveCount = 0;

    // public List<TileRoad> pathManager;
    private Transform[] pathPoints;
    private int currentPathIndex = 0;
    private Transform spawnPoint;
    [SerializeField] private int enemiesPerWave;
    [SerializeField] private float timeBetweenWaves = 5f;


    private bool isWaveActive = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }


    // public void Initilaize(List<TileRoad> pathManager, Transform spawnPoint)
    // {
    //     this.pathManager = pathManager;
    //     this.spawnPoint = spawnPoint;
    //
    //     currentPathIndex = 0;
    //     int childCount = pathManager.Count;
    //     pathPoints = new Transform[childCount];
    //
    //     for (int index = 0; index < childCount; index++)
    //     {
    //         pathPoints[index] = pathManager[index].transform;
    //     }
    // }

    //public void SpawnWave(int worldLevel, int StageLevel, int Round)
    //{
    //    if (Round == 1)
    //    { 
            
    //    }
    //}

    public void StartWave()
    {
        if (!isWaveActive)
            StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        isWaveActive = true;
        waveCount++;
        int enemiesSpawned = 0;
        int enemyTypes = 0;

        switch (waveCount)
        {
            case 1:
            case 2:
                enemyTypes = 0;
                enemiesPerWave++;
                break;
            case 3:
                enemyTypes = 1;
                enemiesPerWave = 5;
                break;
        }


        while (enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy(enemyTypes);
            enemiesSpawned++;
            yield return new WaitForSeconds(1f); // �� �� ����
        }

        yield return new WaitForSeconds(timeBetweenWaves);
        isWaveActive = false;

        // ���� ���̺� �ڵ� ���� ����
        if (waveCount < 3)
            StartWave();

        waveCount = 0;
    }

    private void SpawnEnemy(int num)
    {
        Transform spawnPoint = this.spawnPoint;
        GameObject go = Instantiate(enemyPrefabs[num], spawnPoint.position, Quaternion.identity);
        go.GetComponent<Enemy11>().Initialize(pathPoints);
    }


}
