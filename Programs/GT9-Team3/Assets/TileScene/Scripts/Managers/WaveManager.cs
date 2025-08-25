using Assets.FantasyMonsters.Common.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    private GameManager _gameManager;
    public ObjectPoolManager _poolManager;

    List<Transform> path = new List<Transform>();

    List<float> SpawnStartTime = new List<float>();
    List<int> EnemyId = new List<int>();
    List<int> SpawnBatchSize = new List<int>();
    List<int> SpawnRepeat = new List<int>();
    List<float> SpawnintervalSec = new List<float>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _poolManager = ObjectPoolManager.Instance;

    }

    public void SetPath(List<Transform> path)
    {
        this.path = path;
    }

    public void SpawnWave(int monsterID)
    {
        float spawnTime = 1.1f;
        //int monsterID = 1000;
        float interval = 0.5f; // 간격 조절 가능

        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(SpawnEnemyWithDelay(i * spawnTime, monsterID));
        }

    }

    public void SpawnWaves(int waveID)
    {
        WaveSystem(waveID);
        
        StartCoroutine(AwakeWave());
        
    }

    IEnumerator AwakeWave()
    {
        int index = 0;
        while (SpawnStartTime[index] > -1)
        {
            float delay = index == 0 ? SpawnStartTime[0] : SpawnStartTime[index] - SpawnStartTime[index - 1]; 
            yield return new WaitForSeconds(delay);
            
            for (int j = 0; j < SpawnRepeat[index]; j++)
            {
                StartCoroutine(SpawnEnemyWithDelay(j * SpawnintervalSec[index], EnemyId[index]));    
            }
            
            Debug.Log($"Wave {index} 생성");
            
            index++;
        }
    }

    // 적 유닛 생성 시스템
    IEnumerator SpawnEnemyWithDelay(float spawnTime, int monsterID)
    {
        yield return new WaitForSeconds(spawnTime);
        Debug.Log($"Enemy 생성");

        var config = EnemyConfigManager.Instance.GetConfig(monsterID);
        if (config == null)
        {
            Debug.LogError($"EnemyConfig 생성 실패: monsterID {monsterID}");
            yield break;
        }

        SpanwEnemy(config);
    }

    public void SpanwEnemy(EnemyConfig config)
    {
        GameObject enemyObj = _poolManager.GetEnemy();
        if (enemyObj != null)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy._enemyStat.Setup(config);
            enemy._enemyMovement.pathPoint(path);
        }
    }

    public void WaveSystem(int waveID)
    { 
        // key = 10101;
        var jsonData = _gameManager._dataManager.WaveDataLoader.GetByKey(waveID);

        if (jsonData == null)
        {
            Debug.LogError($"웨이브 ID {waveID}에 대한 JSON 데이터 없음");
        }
        
        this.SpawnStartTime = jsonData.SpawnStartTime;
        this.EnemyId = jsonData.EnemyID;
        this.SpawnBatchSize = jsonData.SpawnBatchSize;
        this.SpawnRepeat = jsonData.SpawnRepeat;
        this.SpawnintervalSec = jsonData.SpawnintervalSec;
    }

}
