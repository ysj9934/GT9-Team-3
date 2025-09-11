using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDataManager : MonoBehaviour
{
    
    [SerializeField] private int testKey;
    public GameObject[] enemyPrefabs; // EnemyID에 맞춰 넣을 프리팹 배열
    //public Transform[] spawnerTransforms; // SpawnerID에 맞춰 배치할 스폰 위치들
    public List<TileData> path;


    private void Awake()
    {
        // 테스트용 Key값 (존재하는 key로 바꾸세요)
        //testKey = 10104;  // 인스펙터 값 무시하고 코드 값 강제 적용
    }

    private void Start()
    {
        // WaveReader 싱글턴 접근
        if (WaveDataReader.Instance == null)
        {
            Debug.LogError("웨이브 리더가 없음");
            return;
        }
    }

    public void StartWave(int key)
    {
        var masterData = WaveDataReader.Instance.GetWaveMasterByKey(key);
        if (masterData != null)
        {
            Debug.Log($"웨이브 정보 : {masterData.key}, 이름: {masterData.Inner_Name}, {masterData.RoundIndex}라운드의 {masterData.WaveInRound}웨이브");
        }
        else
        {
            Debug.LogWarning($"해당 키를 찾을 수 없음 : {key}");
        }

        // Wave Spawn Table 접근
        var spawnData = WaveDataReader.Instance.GetWaveSpawnByKey(key);
        if (spawnData != null)
        {
            //Debug.Log($"[Spawn] Key(고유번호): {spawnData.key}, EnemyID_01: {spawnData.EnemyID_01}, SpawnerID_01: {spawnData.SpawnerID_01}");
            for (int i = 1; i <= 5; i++)
            {
                int spawnSquence = spawnData.GetSpawnSquence(i);
                float spawnStartTime = spawnData.GetSpawnStartTime(i);
                int spawnerID = spawnData.GetSpawnerID(i);
                int enemyID = spawnData.GetEnemyID(i);
                int spawnBatchSize = spawnData.GetSpawnBatchSize(i);
                int spawnRepeat = spawnData.GetSpawnRepeat(i);
                float spawnIntervalSec = spawnData.GetSpawnIntervalSec(i);

                if (spawnSquence != -1 && enemyID != -1)    //해당 시퀀스(순서)가 존재하고 적이 존재하다면
                {
                    Debug.Log($"{spawnSquence} 순서 : {spawnStartTime}초부터 {spawnerID} 스폰서에서 EnemyID_{i}가 {enemyID}인 몬스터가 " +
                              $"{spawnIntervalSec}초 간격으로 {spawnBatchSize}마리씩 {spawnRepeat}번 생성");

                    EnemyDataManager.Instance.PrintEnemyInfo(enemyID);
                    // 스폰 코루틴 실행
                    StartCoroutine(SpawnEnemiesCoroutine(
                        spawnStartTime, spawnerID, enemyID, spawnBatchSize, spawnRepeat, spawnIntervalSec
                    ));
                }
            }
        }
        else
        {
            Debug.LogWarning($"{key}에 해당하는 웨이브 생성 데이터가 없어요");
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(
        float startTime, int spawnerID, int enemyID, int batchSize, int repeatCount, float intervalSec)
    {
        // 시작 시간 대기
        yield return new WaitForSeconds(startTime);

        // 반복 스폰
        for (int repeat = 0; repeat < repeatCount; repeat++)
        {
            for (int j = 0; j < batchSize; j++)
            {
                SpawnEnemy(spawnerID, enemyID);
            }

            // 다음 반복 전 대기
            if (repeat < repeatCount - 1)
                yield return new WaitForSeconds(intervalSec);
        }
    }

    private void SpawnEnemy(int spawnerID, int enemyID)
    {
        //GameObject prefab = EnemyDataReader.Instance.GetPrefabByKey(enemyID);
        //if (prefab == null)
        //{
        //    Debug.LogWarning($"EnemyID {enemyID}에 해당하는 프리팹을 찾을 수 없음");
        //    return;
        //}

        //if (TileController.Instance == null || TileController.Instance.startTile == null)
        //{
        //    Debug.LogWarning("TileController 또는 startTile이 비어있습니다. 스폰 불가");
        //    return;
        //}

        //Transform spawnPoint = TileController.Instance.startTile.transform;
        //Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        //Debug.Log($"스포너에서 적 {enemyID} 생성");

        path = TileController.Instance.path;
        Transform spawnPoint = TileController.Instance.startTile.transform;
        GameObject go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = go.GetComponent<Enemy>();
        //enemy.SetPath(path);
        //enemy.Initialize();

        Debug.Log($"스포너에서 적 {enemyID} 생성");
    }
}