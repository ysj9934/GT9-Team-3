using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private Enemy _enemy;

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
    public float enemyEnemySkillID;
    public float enemyPrefabID;
    public string enemyImage;
    public float enemySize;
    public string enemyDescription;

    [SerializeField] private Transform visualRoot;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void Setup(EnemyConfig config)
    {
        _enemy.isAlive = true;

        foreach (Transform child in visualRoot)
        {
            Destroy(child.gameObject);
        }

        this.keycode = config.keycode;
        this.enemyName = config.enemyName;
        this.enemyType = config.enemyType;
        this.enemyMaxHP = config.enemyMaxHP;
        this.enemyMovementSpeed = config.enemyMovementSpeed;
        this.enemyAttackDamage = config.enemyAttackDamage;
        this.enemyAttackSpeed = config.enemyAttackSpeed;
        this.enemyAttackRange = config.enemyAttackRange;
        this.enemyAttackType = config.enemyAttackType;
        this.enemyProjectileID = config.enemyProjectileID;
        this.enemyDefense = config.enemyDefense;
        this.enemyTilePieceAmount = config.enemyTilePieceAmount;
        this.enemyIgnoreDebuff = config.enemyIgnoreDebuff;
        this.enemyEnemySkillID = config.enemyEnemySkillID;
        this.enemyPrefabID = config.enemyPrefabID;
        this.enemyImage = config.enemyImage;
        this.enemySize = config.enemySize;
        this.enemyDescription = config.enemyDescription;

        // 외형 변경
        GameObject visual = Instantiate(config.enemyPrefab, visualRoot);
        visual.transform.localPosition = Vector2.zero;

        // 크기 변경
        visual.transform.localScale = new Vector2(this.enemySize, this.enemySize);

        _enemy._enemyHealthHandler.InitializeHealth();
    }
}