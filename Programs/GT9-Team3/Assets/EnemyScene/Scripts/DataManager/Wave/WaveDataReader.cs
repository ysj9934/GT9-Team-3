using System.Collections.Generic;
using UnityEngine;

public class WaveDataReader : MonoBehaviour
{
    public static WaveDataReader Instance { get; private set; }

    // ������ ���̺� �δ�
    private Wave_DataTable_WaveMasterTableLoader masterLoader;
    private Wave_DataTable_WaveSpawnTableLoader spawnLoader;

    private void Awake()
    {
        // �̱��� �ʱ�ȭ. �̱����� �����ϰ� �ߺ��� ������
        if (Instance == null)
        {
            // �ٸ� ��ũ��Ʈ���� WaveReader.Instance�� ������ �� �ְ� ��
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ������ ���̺� �ε�
        masterLoader = new Wave_DataTable_WaveMasterTableLoader();
        spawnLoader = new Wave_DataTable_WaveSpawnTableLoader();
    }

    #region Wave Master Table ����
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

    #region Wave Spawn Table ����
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