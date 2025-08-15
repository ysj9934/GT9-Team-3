using UnityEngine;

public class WaveDataManager : MonoBehaviour
{
    // 테스트용 Key값 (존재하는 key로 바꾸세요)
    public int testKey = 10102;

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
                if (spawnSquence != -1) // -1이면 없는 값
                    Debug.Log($"{spawnSquence} 순서 : {spawnStartTime}초에 {spawnerID} 스폰서에서 EnemyID_{i}가 {enemyID}인 몬스터가 " +
                        $"{spawnIntervalSec}초 간격으로 {spawnBatchSize}마리씩 {spawnRepeat}번 생성");
            }
        }
        else
        {
            Debug.LogWarning($"Wave Spawn data not found for key: {testKey}");
        }
    }
}