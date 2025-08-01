using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    private GameManager gameManager;
    
    [SerializeField] private GameObject baseTilePrefab;
    [SerializeField] private GameObject[] tilePrefabs;
    
    [SerializeField] public Vector2 startPosition = Vector2.zero;

    [SerializeField] private List<GameObject> tileList = new List<GameObject>();


    private void Start()
    {
        gameManager = GameManager.Instance;
        
        InitBaseTile(gameManager.blockCount);
    }

    // 타일 초기 세팅
    public void InitBaseTile(int blockCount)
    {
        // 타일 초기화
        ResetTile();
        
        // base tile setting
        CreateBaseTile(blockCount);
    }

    // 타일 초기화
    public void ResetTile()
    {
        gameManager.DestroyOfType<Tile>();
        tileList.Clear();
    }

    // 베이스 타일 생성
    private void CreateBaseTile(int blockCount)
    {
        // 타일 위치 초기화
        switch (blockCount)
        {
            case 5:
                startPosition = new Vector2(0, 4.2f);
                break;
            case 7:
                startPosition = new Vector2(0, 8.4f);
                break;
            case 9:
                startPosition = new Vector2(0, 12.6f);
                break;
        }
        
        // 타일 
        int[] blocksPerRow2 = {1, 2, 3, 2, 1};
        
        for (int row = 0; row < blocksPerRow2.Length; row++)
        {
            int count = blocksPerRow2[row];
            float offset = -(count - 1);
            
            for (int i = 0; i < count; i++)
            {
                // base tile
                if (row == 2 && i == 1)
                {
                    Vector2 pos = startPosition + new Vector2(offset * 3.6f + 7.2f * i, -row * 2.1f);
                    GameObject go = GetBaseTile(baseTilePrefab, pos);
                    SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
                    for (int j = 0; j < renderers.Length; j++)
                    {
                        renderers[j].sortingOrder += row * 10 -1000;
                    }
                
                    tileList.Add(go);
                }
                // basic tile
                else
                {
                    Vector2 pos = startPosition + new Vector2(offset * 3.6f + 7.2f * i, -row * 2.1f);
                    GameObject go = GetTile(tilePrefabs, pos);
                    SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
                    for (int j = 0; j < renderers.Length; j++)
                    {
                        renderers[j].sortingOrder += row * 10 -1000;
                    }
                
                    tileList.Add(go);
                }
            }
        }
    }

    // base tile
    private GameObject GetBaseTile(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity);
    }

    // basic tile
    private GameObject GetTile(GameObject[] prefab, Vector2 pos)
    {
        return Instantiate(prefab[Random.Range(0, prefab.Length)], pos, Quaternion.identity);
    }

}
