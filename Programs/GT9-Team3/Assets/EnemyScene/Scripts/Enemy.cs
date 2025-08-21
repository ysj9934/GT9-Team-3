using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int key; // JSON에 정의된 key
    public EnemyStat stat;  //ScriptableObject로부터 적의 스탯을 가져옴

    Enemy_DataTable_EnemyStatTable data;
    string prefabName;

    //체력 관련
    [SerializeField] private float currentHp;
    [SerializeField] Image healthBar;     // Foreground Image 연결

    public Vector2 targetPosition;  // 목표 위치는 public으로 둠

    private EnemyAnimationController animController;
    private bool isDead = false;

    private void Awake()
    {
        prefabName = gameObject.name;

        // (Clone) 제거
        string displayName = prefabName;
        if (displayName.EndsWith("(Clone)"))
            displayName = displayName.Substring(0, displayName.Length - 7);

        animController = GetComponent<EnemyAnimationController>();

        if (EnemyDataReader.Instance == null)
        {
            Debug.LogError("EnemyDataReader.Instance is null. 싱글톤 초기화 순서 확인!");
            return;
        }

        Debug.Log($"프리팹 이름 찾는 중: '{displayName}' in EnemyDataReader");

        data = EnemyDataReader.Instance.GetEnemyStatByImage(prefabName);
        if (data != null)
        {
            key = data.key;

            // ScriptableObject 개별 생성
            stat = ScriptableObject.CreateInstance<EnemyStat>();
            stat.enemyID = data.key;
            stat.enemy_Inner_Name = data.Enemy_Inner_Name;
            stat.maxHP = data.MaxHP;
            stat.movementSpeed = data.MovementSpeed;
            stat.attackDamage = data.AttackDamage;
            stat.attackSpeed = data.AttackSpeed;
            stat.attackRange = data.AttackRange;
            stat.attackType = data.AttackType;
            stat.projectileID = data.ProjectileID;
            stat.defense = data.Defense;
            stat.tilePieceAmount = data.TilePieceAmount;
            stat.ignoreDebuff = data.IgnoreDebuff;
            stat.enemy_Skill_ID = data.Enemy_Skill_ID;
        }
        else
        {
            Debug.LogWarning($"Enemy key {key} 데이터가 없습니다!");
        }
    }

    void Start()
    {
        currentHp = stat.maxHP;
        UpdateHealthBar();

        if (stat != null)
        {
            Debug.Log($"Enemy Stat Info:\n" +
                      $"ID: {stat.enemyID}\n" +
                      $"Name: {stat.enemy_Inner_Name}\n" +
                      $"MaxHP: {stat.maxHP}\n" +
                      $"Speed: {stat.movementSpeed}\n" +
                      $"AttackDamage: {stat.attackDamage}\n" +
                      $"AttackSpeed: {stat.attackSpeed}\n" +
                      $"AttackRange: {stat.attackRange}\n" +
                      $"AttackType: {stat.attackType}\n" +
                      $"ProjectileID: {stat.projectileID}\n" +
                      $"Defense: {stat.defense}\n" +
                      $"TilePieceAmount: {stat.tilePieceAmount}\n" +
                      $"IgnoreDebuff: {stat.ignoreDebuff}\n" +
                      $"SkillID: {stat.enemy_Skill_ID}");
        }
        else
        {
            Debug.LogWarning($"{prefabName}의 stat이 null입니다!");
        }
    }

    // 경로를 설정하는 함수 (예: 경로를 따라 이동하는 적을 만들 때 사용)
    //public void SetPath(Transform[] newPath)
    //{
    //    path = newPath;
    //}

    void Update()
    {

    }

    //충돌 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알에 맞은 경우
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("Enemy hit by Bullet");
            TakeDamage(1); // 기본 데미지 1
            Destroy(collision.gameObject); // 총알 제거
        }

        // 플레이어와 충돌한 경우
        else if (collision.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with Player");
            // 예: 플레이어에 데미지 주거나 자폭 등
            Die(); // 자폭형 적이라면
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // 이미 죽은 상태면 무시

        currentHp -= damage;
        UpdateHealthBar(); // 여기 추가
        if (currentHp <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;  // 중복 호출 방지

        isDead = true;

        // 죽는 애니메이션 재생 요청
        if (animController != null)
        {
            animController.PlayDieAnimation();

            StartCoroutine(DestroyAfterDelay(1f));
        }
        else
        {
            // 애니메이터 없으면 바로 삭제
            Debug.Log("애니메이션이 없네?");
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        TakeDamage(10); // 클릭 시 데미지 1
        Debug.Log($"{currentHp} , {animController.HasDied()}");
        if (healthBar != null)
            Debug.Log(healthBar.fillAmount);
        else
            Debug.LogWarning("healthBar is null");
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.fillAmount = currentHp / stat.maxHP;
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}