using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WavePanel : MonoBehaviour
{
    // Object Structure
    [SerializeField] public Slider waveProgressBar01;
    [SerializeField] public Slider waveProgressBar02;
    [SerializeField] public Slider waveProgressBar03;
    [SerializeField] public TextMeshProUGUI enemyCountText;

    // Object Data
    private float currentWavePoint = 0;
    private float totalWavePoint = 0;
    private int enemyleftCount = 0;
    private int enemyMaxCount = 0;

    private void Awake()
    {
        ResetAllWavePoint();
    }


    // 적을 소환할 때마다 호출되어 ProgressBar를 업데이트
    private void UpdateWaveProgress(int index)
    {
        switch (index)
        {
            case 1:
                waveProgressBar01.value = currentWavePoint / totalWavePoint;
                Debug.LogWarning($"waveProgressBar: {waveProgressBar01.value} / currentWavePoint: {currentWavePoint} / totalRounddPoint: {totalWavePoint}");
                break;
            case 2:
                waveProgressBar02.value = currentWavePoint / totalWavePoint;
                Debug.LogWarning($"waveProgressBar: {waveProgressBar02.value} / currentWavePoint: {currentWavePoint} / totalRounddPoint: {totalWavePoint}");
                break;
            case 3:
                waveProgressBar03.value = currentWavePoint / totalWavePoint;
                Debug.LogWarning($"waveProgressBar: {waveProgressBar03.value} / currentWavePoint: {currentWavePoint} / totalRounddPoint: {totalWavePoint}");
                break;
        }
    }

    public void UpdateWaveCount(int index)
    {
        currentWavePoint++;
        UpdateWaveProgress(index);
    }

    // Wave세팅시 최대 갯수 초기화
    public void ResetWavePoint(int totalWavePoint)
    {
        this.totalWavePoint = totalWavePoint;
        currentWavePoint = 0;
    }

    // 적을 죽일 때마다 호출되어 CountText를 업데이트
    private void UpdateEnemyCountText()
    {
        enemyCountText.text = $"X {enemyleftCount}";
        Debug.LogWarning($"enemyCountText: {enemyCountText.text} / enemyleftCount: {enemyleftCount} / enemyMaxCount: {enemyMaxCount}");
    }

    public void UpdateEnemyCount()
    {
        enemyleftCount--;
        UpdateEnemyCountText();
    }


    // 웨이브가 종료되면 Wave의 총 적유닛 수로 초기화
    public void ResetEnemyCount(int enemyMaxCount)
    {
        this.enemyMaxCount = enemyMaxCount;
        enemyleftCount = this.enemyMaxCount;
        UpdateEnemyCountText();
    }

    /// <summary>
    /// WavePoint Reset
    /// WaveController에서 제어한다
    /// </summary>
    public void ResetAllWavePoint()
    {
        waveProgressBar01.value = 0;
        waveProgressBar02.value = 0;
        waveProgressBar03.value = 0;
    }
}
