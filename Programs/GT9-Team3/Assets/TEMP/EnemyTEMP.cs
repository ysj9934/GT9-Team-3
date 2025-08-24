using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTEMP : MonoBehaviour
{
    public ObjectPoolManager _poolManager;

    public string Enemy_Name;
    public int maxHP;
    public float movementSpeed;
    public int currentHealth;

    [SerializeField] private Transform visualRoot;
    public bool isAlive = false;

    private void Awake()
    {
        _poolManager = ObjectPoolManager.Instance;
    }

    public void Setup(EnemyConfig config)
    {
        isAlive = true;

        foreach (Transform child in visualRoot)
        {
            Destroy(child.gameObject);
        }

        this.Enemy_Name = config.Enemy_Name;
        this.maxHP = config.maxHP;
        this.movementSpeed = config.movementSpeed;

        // 외형 변경
        GameObject visual = Instantiate(config.enemyPrefab, visualRoot);
        visual.transform.localPosition = Vector2.zero;

        currentHealth = maxHP;
    }


    private Transform[] pathPoints;
    private int currentPathIndex = 0;

    public void pathPoint(List<Transform> path)
    {
        gameObject.transform.position = path[0].transform.position + new Vector3(0f, -0.16f, 0f);
        currentPathIndex = 0;

        int childCount = path.Count;
        pathPoints = new Transform[childCount];

        for (int index = 0; index < childCount; index++)
        {
            pathPoints[index] = path[index].transform;
        }
    }

    private void Update()
    {
        Transform target = pathPoints[currentPathIndex];

        // MoveTowards를 사용해 목표점까지 정확히 이동
        Vector2 pos = target.position + new Vector3(0f, -0.16f, 0f);
        transform.position = Vector3.MoveTowards(transform.position, pos, movementSpeed * Time.deltaTime);

        // 목표점에 도달했으면 다음 지점으로 이동
        if (Vector3.Distance(transform.position, pos) < 0.01f)
        {
            currentPathIndex++;
        }

        if (currentPathIndex >= pathPoints.Length)
        {
            currentPathIndex = 0;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log($"Enemy Clicked: {Enemy_Name}");

        if (!isAlive) return;

        TakeDamage(100);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DieMotion();
        }
    }

    private void DieMotion()
    {
        isAlive = false;

        _poolManager.ReturnEnemy(this.gameObject);
    }
}
