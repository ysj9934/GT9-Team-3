using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GameObject waveLoaderPrefab; // WaveLoader 프리팹
    public Transform spawnPoint;        // 인스턴스화할 위치, 안 쓰면 Vector3.zero

    // 버튼 클릭 시 호출될 함수
    public void SpawnWaveLoader()
    {
        if (waveLoaderPrefab == null)
        {
            Debug.LogWarning("WaveLoader Prefab이 할당되지 않았습니다!");
            return;
        }

        Vector3 position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        GameObject instance = Instantiate(waveLoaderPrefab, position, Quaternion.identity);

        // 초기화 함수가 필요하다면 호출
        var logic = instance.GetComponent<WaveDataReader>();
        if (logic != null)
        {
            //logic.Initialize(); // WaveLoader에 Initialize 함수가 있다면 호출
            Debug.Log("하핳");
        }
    }
}
