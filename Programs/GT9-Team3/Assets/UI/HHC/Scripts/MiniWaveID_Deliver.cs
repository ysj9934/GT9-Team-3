using System.Collections.Generic;
using UnityEngine;

public class MiniWaveID_Deliver : MonoBehaviour
{
    [SerializeField] private int stageID; // Inspector에서 버튼마다 설정
    private List<Wave_DataTable_WaveMasterTable> waves;

    public void OnClick()
    {
        // StageManager 존재 체크
        if (StageManager.Instance == null)
        {
            Debug.LogError("StageManager가 존재하지 않습니다!");
            return;
        }

        // 클릭 시 Stage_ID 세팅
        StageManager.Instance.SelectStage(stageID);

        // Stage_ID별 Wave 데이터 가져오기
        waves = StageManager.Instance.GetWavesByStageID(stageID);

        if (waves.Count == 0)
        {
            Debug.LogWarning($"Stage_ID {stageID}에 해당하는 Wave가 없습니다.");
        }
        else
        {
            Debug.Log($"Stage_ID {stageID}에 {waves.Count}개의 Wave가 있습니다.");
        }
    }
}