using System.Collections;
using System.Collections.Generic;
using Assets.FantasyMonsters.Common.Scripts;
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
    public float enemySkillID;
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
        this.enemySkillID = config.enemySkillID;
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

        if (enemySkillID > 0)
            ApplyEnemySkills(enemySkillID);
    }

    public void ApplyEnemySkills(float enemySkillId)
    {
        int key = Mathf.FloorToInt(enemySkillId); // float → int 변환

        if (!_enemy._gameManager._dataManager.EnemySkillListTableLoader.ItemsDict.TryGetValue(key, out var skillList))
        {
            Debug.LogWarning($"Skill List ID {key} not found.");
            return;
        }

        foreach (var skillId in skillList.Skill_ID)
        {
            if (!_enemy._gameManager._dataManager.EnemySkillTableLoader.ItemsDict.TryGetValue(skillId, out var skillData))
            {
                Debug.LogWarning($"Skill ID {skillId} not found.");
                return;
            }

            switch (skillId)
            {
                case 2000:
                    // 체력이 일정 이하로 떨어졌을 때 이동속도 증가
                    Skill_2000(skillData);
                    break;
                case 2001:
                    // 체력이 일정 이하로 떨어졌을 때 이동속도 증가
                    Skill_2001(skillData);
                    break;
                case 2003:
                    // 체력이 일정 이하로 떨어졌을 때 이동속도 증가
                    Skill_2003(skillData);
                    break;
                case 2004:
                    Skill_2004(skillData);
                    break;
                // 다른 스킬 타입에 대한 케이스 추가
                default:
                    
                    break;
            }
        }
    }

    /// <summary>
    /// 체력회복 
    /// 10초마다 체력 20(상수) 회복
    /// </summary>
    /// <param name="skillData"></param>
    public void Skill_2000(EnemySkillTable skillData)
    {
        float coolDownTimer = 0f;
        float coolDownValue = skillData.Cooldown;
        float intervalTimer = 0f;

        intervalTimer += Time.deltaTime;
        if (intervalTimer < skillData.TriggerValue) return;

        coolDownTimer += Time.deltaTime;
        if (coolDownTimer >= coolDownValue)
        { 
            // 체력 회복
            
        }

    }

    public void Skill_2001(EnemySkillTable skillData)
    {
        
    }

    /// <summary>
    /// 고속질주
    /// 조건: 체력이 50% 이하로 떨어졌을 때
    /// 액션: 이동속도 30% 증가
    /// </summary>
    /// <param name="skillData"></param>
    public void Skill_2003(EnemySkillTable skillData)
    {
        float castTimer = 0f;
        int castValue = (int) skillData.CastTime;

        if (_enemy._enemyHealthHandler.currentHealth <= enemyMaxHP / 2)
        {
            castTimer += Time.deltaTime;
            if (castTimer >= castValue)
                enemyMovementSpeed *= (1 + skillData.EffectValue / 100);
        }
    }

    /// <summary>
    /// 전장 가속 오라
    /// 조건: 없음
    /// 대상: 전체 아군
    /// 범위: 전체 맵
    /// 액션: 이동속도 증가
    /// </summary>
    /// <param name="skillData"></param>
    public void Skill_2004(EnemySkillTable skillData)
    {
        GameManager.Instance._waveManager.activeEnemies.ForEach(enemy =>
        {
            if (enemy.TryGetComponent<EnemyStat>(out var enemyStat))
            {
                enemyStat.enemyMovementSpeed *= (1 + skillData.EffectValue / 100);
            }
        });
    }

    /// <summary>
    /// 전장 개조
    /// 조건: 없음
    /// 대상: 전체 아군
    /// 범위: 전체 맵
    /// 액션: 이동속도 증가
    /// </summary>
    /// <param name="skillData"></param>
    public void Skill_2005(EnemySkillTable skillData)
    {
        
    }
}