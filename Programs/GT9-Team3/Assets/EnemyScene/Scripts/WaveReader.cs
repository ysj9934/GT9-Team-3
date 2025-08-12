using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveReader : MonoBehaviour
{
    private Test_WaveLoader loader;

    void Start()
    {
        // JSON 파일 로드 (경로: Assets/Resources/JSON/Test_Wave.json)
        loader = new Test_WaveLoader();

        // 방법 1: key 값으로 데이터 가져오기
        int searchKey = 10101;  //첫 번째 웨이브
        Test_Wave waveData = loader.GetByKey(searchKey);
        if (waveData != null)
        {
            Debug.Log($"[KEY 검색] Key: {waveData.key}, Inner_Name: {waveData.Inner_Name}");
        }
        else
        {
            Debug.LogWarning($"Key {searchKey}에 해당하는 데이터가 없습니다.");
        }

        // 방법 2: 인덱스로 데이터 가져오기
        int searchIndex = 0;
        Test_Wave waveByIndex = loader.GetByIndex(searchIndex);
        if (waveByIndex != null)
        {
            Debug.Log($"[INDEX 검색] Index: {searchIndex}, Key: {waveByIndex.key}, Inner_Name: {waveByIndex.Inner_Name}");
        }

        // 방법 3: 전체 리스트 순회
        foreach (var wave in loader.ItemsList)
        {
            Debug.Log($"Key: {wave.key}, Stage: {wave.Stage_ID}, WaveNo: {wave.StageWaveNo}");
        }
    }
}
