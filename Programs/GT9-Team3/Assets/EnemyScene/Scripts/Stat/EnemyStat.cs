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
    public int attackType; // 0: 근접, 1: 원거리
    public int projectileID; // -1이면 Projectile 없음
    public int defense;
    public int tilePieceAmount; // 타일 조각 개수
    public string ignoreDebuff; // 상태이상 적용 여부 (예: "None", "All", "Stun", "Poison" 등)
    public int enemy_Skill_ID; // -1이면 스킬 없음
}