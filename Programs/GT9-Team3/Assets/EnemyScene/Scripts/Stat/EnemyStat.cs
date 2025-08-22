using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStat", menuName = "Stats/EnemyStat")]
public class EnemyStat : ScriptableObject
{
    public int enemyID;
    public string enemy_Inner_Name;
    public int maxHP;
    public float movementSpeed;
    public int attackDamage;
    public float attackSpeed;
    public float attackRange;
    public int attackType; // 0: ����, 1: ���Ÿ�
    public int projectileID; // -1�̸� Projectile ����
    public int defense;
    public int tilePieceAmount; // Ÿ�� ���� ����
    public string ignoreDebuff; // �����̻� ���� ���� (��: "None", "All", "Stun", "Poison" ��)
    public int enemy_Skill_ID; // -1�̸� ��ų ����
}