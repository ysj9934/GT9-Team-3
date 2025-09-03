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

    private List<EnemySkillTable> activeSkills = new List<EnemySkillTable>();
    private Dictionary<int, float> cooldownTimers = new Dictionary<int, float>();
    private Dictionary<int, float> intervalTimers = new Dictionary<int, float>();
    private HashSet<int> skillFlags = new HashSet<int>(); // 한 번만 발동되는 스킬 체크용


    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (!_enemy.isAlive) return;


        foreach (var skill in activeSkills)
        {
            cooldownTimers[skill.key] += Time.deltaTime;
            intervalTimers[skill.key] += Time.deltaTime;
            float coolDownValue = skill.Cooldown;
            float intervalValue = skill.TriggerValue;
            float effectValue = skill.EffectValue;

            switch (skill.key)
            {
                /// <summary>
                /// 체력회복 
                /// 10초마다 체력 20(상수) 회복
                /// </summary>
                case 2000:
                    // 완료

                    if (intervalTimers[skill.key] < intervalValue) continue;

                    if (cooldownTimers[skill.key] >= coolDownValue)
                    {
                        cooldownTimers[skill.key] = 0f;
                        // 체력 회복
                        _enemy._enemyHealthHandler.EnemyHeal(effectValue);
                        Debug.LogWarning("Heal " + effectValue);
                    }
                    break;

                /// <summary>
                /// 고속질주
                /// 조건: 체력이 50% 이하로 떨어졌을 때
                /// 액션: 이동속도 30% 증가
                /// </summary>
                case 2003:
                    // 완료
                    float currentHealth = _enemy._enemyHealthHandler.currentHealth;

                    if (!skillFlags.Contains(skill.key) && currentHealth <= enemyMaxHP / 2)
                    {
                        enemyMovementSpeed *= (1 + effectValue / 100);
                        skillFlags.Add(skill.key); // 한 번 발동되었음을 기록
                    }
                    break;

                /// <summary>
                /// 전장 가속 오라
                /// 조건: 없음
                /// 대상: 전체 아군
                /// 범위: 전체 맵
                /// 액션: 이동속도 증가
                /// </summary>
                case 2004:
                    // 완료
                    if (skillFlags.Contains(skill.key)) break; // 이미 발동했으면 스킵

                    GameManager.Instance._waveManager.activeEnemies.ForEach(enemy =>
                    {
                        if (enemy.TryGetComponent<EnemyStat>(out var enemyStat))
                        {
                            Debug.LogWarning("Speed Boost " + effectValue);
                            enemyStat.enemyMovementSpeed *= (1 + effectValue / 100);
                        }
                    });

                    skillFlags.Add(skill.key); // 한 번 발동되었음을 기록
                    break;

                /// <summary>
                /// 전장 가속 오라 3월드
                /// 조건: 패시브
                /// 대상: 전체 아군
                /// 범위: 전체 맵
                /// 액션: 이동속도 증가
                /// </summary>
                case 2005:
                    if (skillFlags.Contains(skill.key)) break; // 이미 발동했으면 스킵

                    GameManager.Instance._waveManager.activeEnemies.ForEach(enemy =>
                    {
                        if (enemy.TryGetComponent<EnemyStat>(out var enemyStat))
                        {
                            enemyStat.enemyMovementSpeed *= (1 + effectValue / 100);
                            Debug.LogWarning("Speed Boost " + effectValue);
                        }
                    });

                    skillFlags.Add(skill.key); // 한 번 발동되었음을 기록

                    break;

                /// <summary>
                /// 체력 회복 3월드
                /// 조건: 없ㅇ음
                /// 대상: 전체 아군
                /// 범위: 전체 맵
                /// 액션: 5초마다 체력 40(상수) 회복
                /// </summary>
                case 2006:

                    if (intervalTimers[skill.key] < intervalValue) continue;
                    
                    if (cooldownTimers[skill.key] >= coolDownValue)
                    {
                        cooldownTimers[skill.key] = 0f;
                        
                        GameManager.Instance._waveManager.activeEnemies.ForEach(enemy =>
                        {
                            // 체력 회복
                            _enemy._enemyHealthHandler.EnemyHeal(effectValue);
                            Debug.LogWarning("Heal " + effectValue);
                        });
                    }
                    break;
            }
            
        }
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
        //ApplyEnemySkills(enemySkillID);
        {
            int key = Mathf.FloorToInt(config.enemySkillID);
            if (_enemy._gameManager._dataManager.EnemySkillListTableLoader.ItemsDict.TryGetValue(key, out var skillList))
            {
                foreach (var skillId in skillList.Skill_ID)
                {
                    Debug.LogWarning("skillId " + skillId);

                    if (_enemy._gameManager._dataManager.EnemySkillTableLoader.ItemsDict.TryGetValue(skillId, out var skillData))
                    {
                        activeSkills.Add(skillData);
                        cooldownTimers[skillId] = 0f;
                        intervalTimers[skillId] = 0f;
                    }
                }
            }

        }
        
    }

    
    public void Skill_2000(EnemySkillTable skillData)
    {
        
        
    }

    public void Skill_2001(EnemySkillTable skillData)
    {
        
    }

    
    public void Skill_2003(EnemySkillTable skillData)
    {
        
    }

    
    public void Skill_2004(EnemySkillTable skillData)
    {
        
    }

    
    /// <param name="skillData"></param>
    public void Skill_2005(EnemySkillTable skillData)
    {
        
    }

    public void Skill_2006(EnemySkillTable skillData)
    {
        
    }
}