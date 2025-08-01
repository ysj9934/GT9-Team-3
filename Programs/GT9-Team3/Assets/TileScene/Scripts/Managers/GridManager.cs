using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GameManager gameManager;
    
    // 수정필요 사항 2025.07.30
    // 1. 타일 위치 정리
    // 2. 타일 교체
    // 3. 타일 회전
    // 4. 타일 배치 계산 정확하게
    
    [SerializeField] private GameObject samplePrefab;
    [SerializeField] public Vector2 startPosition = Vector2.zero;
    

    [SerializeField] private List<GameObject> gridList = new List<GameObject>();
    

    private void Awake()
    {
        
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        
        InitGrid(gameManager.blockCount);
    }
    
    // 그리드 초기 세팅  
    public void InitGrid(int blockCount)
    {
        gameManager.blockCount = blockCount;
        
        // 그리드 타일 초기화
        ResetGrid();
        
        CreateGrid(gameManager.blockCount);

    }

    public void ResetGrid()
    {
        gameManager.DestroyOfType<TileGrid>();
        gridList.Clear();
    }

    private void CreateGrid(int blockCount)
    {
        // 그리드 초기 배치
        switch (blockCount)
        {
            case 5:
                startPosition = new Vector2(0, 8.4f);
                break;
            case 7:
                startPosition = new Vector2(0, 12.6f);
                break;
            case 9:
                startPosition = new Vector2(0, 16.8f);
                break;
        }
        
        // 그리드 배열 맞추기
        int[] gridRow = new int[blockCount * 2 - 1];

        for (int i = 0; i < gridRow.Length; i++)
        {
            if (i < blockCount)
            {
                gridRow[i] = i + 1;    
            }
            else
            {
                gridRow[i] = gridRow.Length - i;
            }
        }
        
        for (int row = 0; row < gridRow.Length; row++)
        {
            int count = gridRow[row]; // 2
            float offset = -(count - 1); // 1

            for (int i = 0; i < count; i++) // count 4
            {
                Vector2 pos = startPosition + new Vector2(offset * 3.6f + 7.2f * i, -row * 2.1f);
                GameObject go = GetGrid(samplePrefab, pos);
                SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].sortingOrder += row * 10 - 1000;
                }

                gridList.Add(go);
            }
        }
    }

    private GameObject GetGrid(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity);
    }

}
