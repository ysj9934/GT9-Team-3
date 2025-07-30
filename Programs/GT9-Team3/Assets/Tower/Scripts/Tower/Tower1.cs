using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower1 : MonoBehaviour
{
    public TowerData data;
    private float attackCooldown;

    private void Start()
    {
        //attackCooldown = 1f / data.attackRadius;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 적 유닛이면 공격 조건 확인
    }

    public void ApplyData(TowerData towerData)
    {
        data = towerData;

        attackCooldown = 1f / data.attackRadius;
        // 예: 사거리, 속도 등 초기화
        // 추후 타겟팅 및 공격 로직에서 data를 참조함
    }
}
