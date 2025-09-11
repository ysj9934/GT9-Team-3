using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : ScriptableObject
{
    public int keycode;
    public string enemyName;
    public string enemyType;
    public float enemyMaxHP;
    public float enemyMovementSpeed;
    public float enemyAttackDamage;
    public float enemyAttackSpeed;
    public float enemyAttackRange;
    public int enemyAttackType;
    public int enemyProjectileID;
    public float enemyDefense;
    public int enemyTilePieceAmount;
    public string enemyIgnoreDebuff;
    public float enemySkillID;
    public float enemyPrefabID;
    public string enemyImage;
    public float enemySize;
    public string enemyDescription;
    public GameObject enemyPrefab;
}
