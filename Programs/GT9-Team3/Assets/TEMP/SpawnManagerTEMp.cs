using Assets.FantasyMonsters.Common.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManagerTEMp : MonoBehaviour
{
    private GameManager _gameManager;
    public ObjectPoolManager _poolManager;

    [SerializeField] List<Transform> path = new List<Transform>();

    
    List<float> SpawnStartTime = new List<float>();
    List<int> EnemyId = new List<int>();
    List<int> SpawnBatchSize = new List<int>();
    List<int> SpawnRepeat = new List<int>();
    List<float> SpawnintervalSec = new List<float>();



    private void Start()
    {
        _gameManager = GameManager.Instance;
        _poolManager = ObjectPoolManager.Instance;

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


        //for (int i = 0; i < 5; i++)
        //{
        //    StartCoroutine(SpawnEnemyWithDelay(i * spawnTime, monsterID));
        //}
    }

    IEnumerator SpawnEnemyWithDelay(float spawnTime, int monsterID)
    {
        yield return new WaitForSeconds(spawnTime);

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
            enemyObj.GetComponent<EnemyTEMP>().Setup(config);
            enemyObj.GetComponent<EnemyTEMP>().pathPoint(path);
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
