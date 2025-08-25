using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    public int towerID;
    public string innerName;        // 타워를 시스템 내부에서 식별하기 위해 사용하는 고유 문자열 이름
    public int towerLevel;
    public string useProjectile;    // 해당 타워가 사용하는 발사체의 내부 이름(Inner_Name)을 정의하는 데이터


    public AttackType attackType;
    public float attackRadius;      // 공격이 적중하는 범위를 정의하는 데이터
    public int targetCount;         // 공격이 적중하는 범위를 정의하는 데이터
    public float attackRange;

    public EnemyType attackEnemyType1;
    public EnemyType attackEnemyType2;
    public TargetPriority[] targetOrder;    // 대상 우선순위

    public ResourceType makeCost;
    public int makeValue;               // 타워를 짓는데 소모되는 자원의 수치를 정의하는 데이터
    public ResourceType sellCost;       // 유저가 타워를 판매할 경우 돌려받는 자원의 종류를 정의하는 데이터
    public int sellValue;               // 타워를 파는데 소모되는 자원의 수치를 정의하는 데이터
    public ResourceType UpgradeCost;
    public int UpgradeValue;

    public int damage;             // 공격 데미지
    public float attackSpeed;      // 초당 공격 횟수

    public GameObject projectilePrefab;     // 사용할 발사체 프리팹
    public ProjectileData projectileData;   // 발사체 속성 정보



}

public enum AttackType { Single, AreaOfEffect }
public enum EnemyType { Ground_Unit, Air_Unit }
public enum TargetPriority { Boss, Base_Range, Lowest_HP, Base_Closest }
public enum ResourceType { Gold, Mana, Crystal, Tilepiece }
