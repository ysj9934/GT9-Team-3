using System.Collections;
using System.Collections.Generic;
using Assets.FantasyMonsters.Common.Scripts;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    // Object Manager
    private GameManager _gameManager;

    // Object Structure
    private Enemy _enemy;

    // Object Info
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

    // Object Setting
    [SerializeField] private Transform visualRoot;

    // Object Skills
    private List<EnemySkillTable> activeSkills = new List<EnemySkillTable>();
    private Dictionary<int, float> cooldownTimers = new Dictionary<int, float>();
    private Dictionary<int, float> intervalTimers = new Dictionary<int, float>();
    private HashSet<int> skillFlags = new HashSet<int>();

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (!_enemy.isAlive) return;


        // 적 보유 스킬 처리
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
                /// 대상: 단일
                /// </summary>
                case 2000:
                    if (intervalTimers[skill.key] < intervalValue) continue;

                    if (cooldownTimers[skill.key] >= coolDownValue)
                    {
                        cooldownTimers[skill.key] = 0f;
                        // 체력 회복
                        _enemy._enemyHealthHandler.EnemyHeal(effectValue, false);
                    }
                    break;

                /// <summary>
                /// 고속질주
                /// 조건: 체력이 50% 이하로 떨어졌을 때
                /// 액션: 이동속도 30% 증가
                /// </summary>
                case 2003:
                    float currentHealth = _enemy._enemyHealthHandler.currentHealth;

                    if (!skillFlags.Contains(skill.key) && 
                        currentHealth <= enemyMaxHP / 2)
                    {
                        enemyMovementSpeed *= (1 + effectValue / 100);
                        skillFlags.Add(skill.key);

                        // [이펙트효과]: 이동속도 증가 이펙트
                        Debug.LogWarning("[Effect] MoveSpeed Effect");

                        // [사운드효과]: 이동속도 증가 사운드
                        Debug.LogWarning("[Sound] MoveSpeed Sound");
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
                    if (skillFlags.Contains(skill.key)) break;

                    _gameManager._waveController.activeEnemies.ForEach(enemy =>
                    {
                        if (enemy.TryGetComponent<EnemyStat>(out var enemyStat))
                        {
                            enemyStat.enemyMovementSpeed *= (1 + effectValue / 100);

                            // [이펙트효과]: 이동속도 증가 이펙트
                            Debug.LogWarning("[Effect] MoveSpeed Effect");
                        }
                    });

                    // [사운드효과]: 이동속도 증가 사운드
                    Debug.LogWarning("[Sound] MoveSpeed Sound");

                    skillFlags.Add(skill.key);
                    break;

                /// <summary>
                /// 전장 가속 오라 3월드
                /// 조건: 패시브
                /// 대상: 전체 아군
                /// 범위: 전체 맵
                /// 액션: 이동속도 증가
                /// </summary>
                case 2005:
                    if (skillFlags.Contains(skill.key)) break;

                    _gameManager._waveController.activeEnemies.ForEach(enemy =>
                    {
                        if (enemy.TryGetComponent<EnemyStat>(out var enemyStat))
                        {
                            enemyStat.enemyMovementSpeed *= (1 + effectValue / 100);

                            // [이펙트효과]: 이동속도 증가 이펙트
                            Debug.LogWarning("[Effect] MoveSpeed Effect");
                        }
                    });

                    // [사운드효과]: 이동속도 증가 사운드
                    Debug.LogWarning("[Sound] MoveSpeed Sound");

                    skillFlags.Add(skill.key);

                    break;

                /// <summary>
                /// 체력 회복 3월드
                /// 조건: 없음
                /// 대상: 전체 아군
                /// 범위: 전체 맵
                /// 액션: 5초마다 체력 40(상수) 회복
                /// </summary>
                case 2006:

                    if (intervalTimers[skill.key] < intervalValue) continue;
                    
                    if (cooldownTimers[skill.key] >= coolDownValue)
                    {
                        cooldownTimers[skill.key] = 0f;

                        _gameManager._waveController.activeEnemies.ForEach(enemy =>
                        {
                            // 체력 회복
                            _enemy._enemyHealthHandler.EnemyHeal(effectValue, true);
                        });

                        // [사운드효과]: 체력 회복 사운드
                        Debug.LogWarning("[Sound] Heal Sound");
                    }
                    break;
            }
            
        }

    }


    /// <summary>
    /// 월드 기믹 효과
    /// </summary>
    private float originEnemyMovementSpeed = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 월드 기믹: 전장 가속
        // 적이 전장 가속 타일에 들어왔을 때 이동속도 30% 증가
        TileInfo tileInfo = collision.GetComponent<TileInfo>();
        if (tileInfo != null)
        {
            if (tileInfo.isBattlefieldModified)
            {
                originEnemyMovementSpeed = enemyMovementSpeed;
                enemyMovementSpeed *= (1 + 30 / 100);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 월드 기믹: 전장 가속
        // 적이 전장 가속 타일에서 나갔을 때 이동속도 원래대로 복구
        TileInfo tileInfo = collision.GetComponent<TileInfo>();
        if (tileInfo != null)
        {
            if (tileInfo.isBattlefieldModified)
            {
                enemyMovementSpeed = originEnemyMovementSpeed;
            }
        }
    }

    /// <summary>
    /// 적 유닛 설정
    /// </summary>
    public void Setup(EnemyConfig config)
    {
        _gameManager = GameManager.Instance;
        DataManager _dataManager = DataManager.Instance;
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
        UpdateVisual(config);

        // 체력 초기화
        _enemy._enemyHealthHandler.InitializeHealth();

        // 보유 스킬 초기화
        if (enemySkillID > 0)
        {
            int key = Mathf.FloorToInt(config.enemySkillID);
            if (_dataManager.EnemySkillListTableLoader.ItemsDict.TryGetValue(key, out var skillList))
            {
                foreach (var skillId in skillList.Skill_ID)
                {
                    Debug.LogWarning("skillId " + skillId);

                    if (_dataManager.EnemySkillTableLoader.ItemsDict.TryGetValue(skillId, out var skillData))
                    {
                        activeSkills.Add(skillData);
                        cooldownTimers[skillId] = 0f;
                        intervalTimers[skillId] = 0f;
                    }
                }
            }
        }

        _enemy.isAlive = true;
    }

    /// <summary>
    /// 외형 변경
    /// </summary>
    /// <param name="config"></param>
    private void UpdateVisual(EnemyConfig config)
    {
        // 외형 변경
        GameObject visual = Instantiate(config.enemyPrefab, visualRoot);
        visual.transform.localPosition = Vector2.zero;

        // 외형 정보 캐싱
        _enemy.spriteRenderers = null;
        _enemy.originSpriteOrder = new Dictionary<SpriteRenderer, int>();
        _enemy.spriteRenderers = visual.GetComponentsInChildren<SpriteRenderer>();
        
        foreach (SpriteRenderer sr in _enemy.spriteRenderers)
        {
            _enemy.originSpriteOrder[sr] = sr.sortingOrder;
        }

        // 크기 변경
        visual.transform.localScale = new Vector2(this.enemySize, this.enemySize);
    }

    private void OnDestroy()
    {
        _enemy.isAlive = false;
        activeSkills.Clear();
        cooldownTimers.Clear();
        intervalTimers.Clear();
        skillFlags.Clear();
    }
}