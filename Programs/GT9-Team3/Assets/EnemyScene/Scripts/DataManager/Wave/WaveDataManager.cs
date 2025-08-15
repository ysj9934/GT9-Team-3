using UnityEngine;
using System.Collections;

public class WaveDataManager : MonoBehaviour
{
    // 테스트용 Key값 (존재하는 key로 바꾸세요)
    public int testKey = 10101;
    public GameObject[] enemyPrefabs; // EnemyID에 맞춰 넣을 프리팹 배열
    public Transform[] spawnerTransforms; // SpawnerID에 맞춰 배치할 스폰 위치들

    private void Start()
    {
        // WaveReader 싱글턴 접근
        if (WaveDataReader.Instance == null)
        {
            Debug.LogError("WaveReader instance not found!");
            return;
        }

        // Wave Master Table 접근
        var masterData = WaveDataReader.Instance.GetWaveMasterByKey(testKey);
        if (masterData != null)
        {
            Debug.Log($"[Master] Key: {masterData.key}, Name: {masterData.Inner_Name}, {masterData.RoundIndex}라운드의 {masterData.WaveInRound}웨이브");
        }
        else
        {
            Debug.LogWarning($"Wave Master data not found for key: {testKey}");
        }

        // Wave Spawn Table 접근
        var spawnData = WaveDataReader.Instance.GetWaveSpawnByKey(testKey);
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
                    Debug.Log($"{spawnSquence} 순서 : {spawnStartTime}초에 {spawnerID} 스폰서에서 EnemyID_{i}가 {enemyID}인 몬스터가 " +
                              $"{spawnIntervalSec}초 간격으로 {spawnBatchSize}마리씩 {spawnRepeat}번 생성");

                    // 스폰 코루틴 실행
                    StartCoroutine(SpawnEnemiesCoroutine(
                        spawnStartTime, spawnerID, enemyID, spawnBatchSize, spawnRepeat, spawnIntervalSec
                    ));
                }
            }
        }
        else
        {
            Debug.LogWarning($"Wave Spawn data not found for key: {testKey}");
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
        if (spawnerID < 0 || spawnerID >= spawnerTransforms.Length)
        {
            Debug.LogWarning($"스포너 {spawnerID}은 유효하지 않아요");
            return;
        }
        if (enemyID < 0 || enemyID >= enemyPrefabs.Length)
        {
            Debug.LogWarning($"적 {enemyID}은 유효하지 않아요");
            return;
        }

        Transform spawnPoint = spawnerTransforms[spawnerID];
        GameObject prefab = enemyPrefabs[enemyID];

        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log($"{enemyID} 스포너에서 {spawnerID} 적 생성");
    }
}