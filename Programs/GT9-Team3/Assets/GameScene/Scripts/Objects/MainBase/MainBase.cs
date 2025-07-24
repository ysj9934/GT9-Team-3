using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 메인기지

// 메인기지의 체력이 모두 소진되는 경우 게임이 패배됩니다.
public class MainBase : MonoBehaviour
{
    [SerializeField] private TextMeshPro hpText;
    [SerializeField] private GameObject hpBar;

    [Header("Base Info")] 
    private float healthPoint;
    [SerializeField] private float maxHealthPoint = 10;
    private bool isLive = false;

    private void Start()
    {
        Initialize();
        hpTexting();
    }

    private void Initialize()
    {
        isLive = true;
        healthPoint = maxHealthPoint;
    }
    
    // (Test) 마우스 클릭시 타워 체력 감소
    private void OnMouseDown()
    {
        if (!isLive) return;
        
        TakeDamage(2);
    }

    // 적에게 피격시 체력이 감소됩니다.
    public void TakeDamage(int damage)
    {
        healthPoint -= damage;
        hpTexting();
        hpBar.transform.localScale = new Vector3(healthPoint / maxHealthPoint, hpBar.transform.localScale.y, 1);
        
        Debug.Log($"TakeDamage: {damage}");
        
        if (healthPoint <= 0) 
            GameOver();
    }
    
    // (Test) 체력 표기
    private void hpTexting()
    {
        hpText.text = $"{healthPoint.ToString()} / {maxHealthPoint}";
    }

    // (Test) 게임 패배
    private void GameOver()
    {
        if (!isLive) return;
        
        Debug.Log("Game Over");
        isLive = false;
    }
}
