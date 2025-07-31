using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 2f;  // 이동 속도 public으로 변경
    [SerializeField] int hp = 10;
    public Vector2 targetPosition;  // 목표 위치는 public으로 둠

    private Transform[] path; // 경로를 따라 이동할 때 사용 (예: Waypoint 시스템)
    private int pathIndex = 0;  // 경로를 따라 이동할 때 사용

    void Start()
    {
        Debug.Log("EnemyTest Initialized");
        Debug.Log(" → target: " + targetPosition);
    }

    // 경로를 설정하는 함수 (예: 경로를 따라 이동하는 적을 만들 때 사용)
    //public void SetPath(Transform[] newPath)
    //{
    //    path = newPath;
    //}

    void Update()
    {
        // 이동 처리
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
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
        hp -= damage;
        if (hp <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Debug.Log(hp);
        TakeDamage(1); // 클릭 시 데미지 1
    }
}