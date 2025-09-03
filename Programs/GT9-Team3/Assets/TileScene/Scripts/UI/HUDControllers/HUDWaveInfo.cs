using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDWaveInfo : MonoBehaviour
{
    // Object Structure
    [SerializeField] public Slider waveProgressBar;
    [SerializeField] public TextMeshProUGUI enemyCountText;

    // Object Data
    private float currentWavePoint = 0;
    private readonly float totalRoundPoint = 90;
    private int enemyleftCount = 0;
    private int enemyMaxCount = 30;

    private void Awake()
    {
        UpdateWaveProgress();
        ResetEnemyCount();
    }


    // 적을 소환할 때마다 호출되어 ProgressBar를 업데이트
    private void UpdateWaveProgress()
    {
        waveProgressBar.value = currentWavePoint / totalRoundPoint;
        Debug.LogWarning($"waveProgressBar: {waveProgressBar.value} / currentWavePoint: {currentWavePoint} / totalRounddPoint: {totalRoundPoint}");
    }

    public void UpdateWaveCount()
    {
        currentWavePoint++;
        UpdateWaveProgress();
    }

    // 라운드가 종료되면 0으로 초기화
    public void ResetWavePoint()
    {
        currentWavePoint = 0;
        UpdateWaveProgress();
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

    // 웨이브가 종료되면 30으로 초기화
    public void ResetEnemyCount()
    {
        enemyleftCount = enemyMaxCount;
        UpdateEnemyCountText();
    }
}
