using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GameObject waveLoaderPrefab; // WaveLoader ������
    public Transform spawnPoint;        // �ν��Ͻ�ȭ�� ��ġ, �� ���� Vector3.zero

    // ��ư Ŭ�� �� ȣ��� �Լ�
    public void SpawnWaveLoader()
    {
        if (waveLoaderPrefab == null)
        {
            Debug.LogWarning("WaveLoader Prefab�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        Vector3 position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        GameObject instance = Instantiate(waveLoaderPrefab, position, Quaternion.identity);

        // �ʱ�ȭ �Լ��� �ʿ��ϴٸ� ȣ��
        var logic = instance.GetComponent<WaveDataReader>();
        if (logic != null)
        {
            //logic.Initialize(); // WaveLoader�� Initialize �Լ��� �ִٸ� ȣ��
            Debug.Log("���K");
        }
    }
}
