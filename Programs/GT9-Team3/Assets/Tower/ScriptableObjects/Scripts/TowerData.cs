using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerID;
    public string innerName;        // Ÿ���� �ý��� ���ο��� �ĺ��ϱ� ���� ����ϴ� ���� ���ڿ� �̸�
    public int towerLevel;
    public string useProjectile;    // �ش� Ÿ���� ����ϴ� �߻�ü�� ���� �̸�(Inner_Name)�� �����ϴ� ������


    public AttackType attackType;
    public float attackRadius;      // ������ �����ϴ� ������ �����ϴ� ������
    public int targetCount;         // ������ �����ϴ� ������ �����ϴ� ������
    public float attackRange;

    public EnemyType attackEnemyType1;
    public EnemyType attackEnemyType2;
    public TargetPriority[] targetOrder;    // ��� �켱����

    public ResourceType makeCost;
    public int makeValue;           // Ÿ���� ���µ� �Ҹ�Ǵ� �ڿ��� ��ġ�� �����ϴ� ������
    public ResourceType sellCost;   // ������ Ÿ���� �Ǹ��� ��� �����޴� �ڿ��� ������ �����ϴ� ������
    public int sellValue;           // Ÿ���� ���µ� �Ҹ�Ǵ� �ڿ��� ��ġ�� �����ϴ� ������

}

public enum AttackType { Single, AreaOfEffect }
public enum EnemyType { Ground_Unit, Air_Unit }
public enum TargetPriority { Boss, Base_Range, Lowest_HP, Closest }
public enum ResourceType { Gold, Mana, Crystal }
