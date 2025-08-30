using System.Collections.Generic;
using UnityEngine;

public class MiniWaveID_Deliver : MonoBehaviour
{
    [SerializeField] private int stageID; // Inspector���� ��ư���� ����
    private List<Wave_DataTable_WaveMasterTable> waves;

    public void OnClick()
    {
        // StageManager ���� üũ
        if (StageManager.Instance == null)
        {
            Debug.LogError("StageManager�� �������� �ʽ��ϴ�!");
            return;
        }

        // Ŭ�� �� Stage_ID ����
        StageManager.Instance.SelectStage(stageID);

        // Stage_ID�� Wave ������ ��������
        waves = StageManager.Instance.GetWavesByStageID(stageID);

        if (waves.Count == 0)
        {
            Debug.LogWarning($"Stage_ID {stageID}�� �ش��ϴ� Wave�� �����ϴ�.");
        }
        else
        {
            Debug.Log($"Stage_ID {stageID}�� {waves.Count}���� Wave�� �ֽ��ϴ�.");
        }
    }
}