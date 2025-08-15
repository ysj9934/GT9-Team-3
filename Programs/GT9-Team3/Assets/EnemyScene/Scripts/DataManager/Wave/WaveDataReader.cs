using System.Collections.Generic;
using UnityEngine;

public class WaveDataReader : MonoBehaviour
{
    public static WaveDataReader Instance { get; private set; }

    // 데이터 테이블 로더
    private Wave_DataTable_WaveMasterTableLoader masterLoader;
    private Wave_DataTable_WaveSpawnTableLoader spawnLoader;

    private void Awake()
    {
        // 싱글톤 초기화. 싱글톤을 보장하고 중복을 방지함
        if (Instance == null)
        {
            // 다른 스크립트에서 WaveReader.Instance로 접근할 수 있게 됨
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 데이터 테이블 로드
        masterLoader = new Wave_DataTable_WaveMasterTableLoader();
        spawnLoader = new Wave_DataTable_WaveSpawnTableLoader();
    }

    #region Wave Master Table 접근
    public Wave_DataTable_WaveMasterTable GetWaveMasterByKey(int key)
    {
        return masterLoader?.GetByKey(key);
    }

    public Wave_DataTable_WaveMasterTable GetWaveMasterByIndex(int index)
    {
        return masterLoader?.GetByIndex(index);
    }

    public List<Wave_DataTable_WaveMasterTable> GetAllWaveMasters()
    {
        return masterLoader != null ? new List<Wave_DataTable_WaveMasterTable>(masterLoader.ItemsList) : null;
    }
    #endregion

    #region Wave Spawn Table 접근
    public Wave_DataTable_WaveSpawnTable GetWaveSpawnByKey(int key)
    {
        return spawnLoader?.GetByKey(key);
    }

    public Wave_DataTable_WaveSpawnTable GetWaveSpawnByIndex(int index)
    {
        return spawnLoader?.GetByIndex(index);
    }

    public List<Wave_DataTable_WaveSpawnTable> GetAllWaveSpawns()
    {
        return spawnLoader != null ? new List<Wave_DataTable_WaveSpawnTable>(spawnLoader.ItemsList) : null;
    }
    #endregion
}