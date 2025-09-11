using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public int SelectedStageID { get; private set; }

    public Wave_DataTable_WaveMasterTableLoader WaveTableLoader { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadWaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadWaveData()
    {
        WaveTableLoader = new Wave_DataTable_WaveMasterTableLoader("JSON/Wave/Wave_DataTable_WaveMasterTable");

        if (WaveTableLoader.ItemsList.Count == 0)
        {
            Debug.LogError("WaveMasterTable JSON이 비어있거나 로드 실패");
        }
    }

    // Stage_ID로 WaveMasterTable 가져오기
    public List<Wave_DataTable_WaveMasterTable> GetWavesByStageID(int stageID)
    {
        List<Wave_DataTable_WaveMasterTable> result = new List<Wave_DataTable_WaveMasterTable>();

        foreach (var item in WaveTableLoader.ItemsList)
        {
            if (item.Stage_ID == stageID)
                result.Add(item);
        }

        return result;
    }

    public void SelectStage(int id)
    {
        SelectedStageID = id;
        Debug.Log("선택된 Stage_ID: " + id);
    }
}