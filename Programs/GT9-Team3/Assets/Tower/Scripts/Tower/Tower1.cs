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
        // �� �����̸� ���� ���� Ȯ��
    }

    public void ApplyData(TowerData towerData)
    {
        data = towerData;

        attackCooldown = 1f / data.attackRadius;
        // ��: ��Ÿ�, �ӵ� �� �ʱ�ȭ
        // ���� Ÿ���� �� ���� �������� data�� ������
    }
}
