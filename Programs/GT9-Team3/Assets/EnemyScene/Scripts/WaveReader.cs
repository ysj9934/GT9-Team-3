using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveReader : MonoBehaviour
{
    private Test_WaveLoader loader;

    void Start()
    {
        // JSON ���� �ε� (���: Assets/Resources/JSON/Test_Wave.json)
        loader = new Test_WaveLoader();

        // ��� 1: key ������ ������ ��������
        int searchKey = 10101;  //ù ��° ���̺�
        Test_Wave waveData = loader.GetByKey(searchKey);
        if (waveData != null)
        {
            Debug.Log($"[KEY �˻�] Key: {waveData.key}, Inner_Name: {waveData.Inner_Name}");
        }
        else
        {
            Debug.LogWarning($"Key {searchKey}�� �ش��ϴ� �����Ͱ� �����ϴ�.");
        }

        // ��� 2: �ε����� ������ ��������
        int searchIndex = 0;
        Test_Wave waveByIndex = loader.GetByIndex(searchIndex);
        if (waveByIndex != null)
        {
            Debug.Log($"[INDEX �˻�] Index: {searchIndex}, Key: {waveByIndex.key}, Inner_Name: {waveByIndex.Inner_Name}");
        }

        // ��� 3: ��ü ����Ʈ ��ȸ
        foreach (var wave in loader.ItemsList)
        {
            Debug.Log($"Key: {wave.key}, Stage: {wave.Stage_ID}, WaveNo: {wave.StageWaveNo}");
        }
    }
}
