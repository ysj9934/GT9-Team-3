using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject samplePrefab;
    [SerializeField] private GameObject samplePrefab2;
    [SerializeField] public Vector2 startPosition = Vector2.zero;
    [SerializeField] public Vector2 startPosition2 = Vector2.zero;
    [SerializeField] private int col = 3;

    // void Start()
    // {
    //     for (int y = 0; y < col; y++)
    //     {
    //         for (int x = 0; x < col; x++)
    //         {
    //             Vector2 spawnPos = startPosition + new Vector2(x * 7.2f, y * 4.2f);
    //             Instantiate(samplePrefab, spawnPos, Quaternion.identity);
    //         }
    //     }
    // }
    
    void Start()
    {
        int[] blocksPerRow = {1, 2, 3, 4, 5, 4, 3, 2, 1};

        for (int row = 0; row < blocksPerRow.Length; row++)
        {
            int count = blocksPerRow[row]; // 2
            float offset = -(count - 1); // 1

            // 짝수 값 찾기
            if (row % 2 != 0)
            {
                for (int i = 0; i < count; i++) // count 4
                {
                    Vector2 pos = startPosition + new Vector2((offset) * 3.6f + 7.2f * i, -row * 2.1f);
                    GameObject go = Instantiate(samplePrefab, pos, Quaternion.identity);
                    SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
                    for (int j = 0; j < renderers.Length; j++)
                    {
                        renderers[j].sortingOrder += row * 10;
                    }
                }    
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Vector2 pos = startPosition + new Vector2(offset * 3.6f + 7.2f * i, -row * 2.1f);
                    GameObject go = Instantiate(samplePrefab, pos, Quaternion.identity);
                    SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
                    for (int j = 0; j < renderers.Length; j++)
                    {
                        renderers[j].sortingOrder += row * 10;
                    }
                }
            }
        }
        
        int[] blocksPerRow2 = {1, 2, 3, 2, 1};

        for (int row = 0; row < blocksPerRow2.Length; row++)
        {
            int count = blocksPerRow2[row]; // 2
            float offset = -(count - 1); // 1

            // 짝수 값 찾기
            if (row % 2 != 0)
            {
                for (int i = 0; i < count; i++) // count 4
                {
                    Vector2 pos = startPosition2 + new Vector2((offset) * 3.6f + 7.2f * i, -row * 2.1f);
                    GameObject go = Instantiate(samplePrefab2, pos, Quaternion.identity);
                    SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
                    for (int j = 0; j < renderers.Length; j++)
                    {
                        renderers[j].sortingOrder += row * 10;
                    }
                }    
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Vector2 pos = startPosition2 + new Vector2(offset * 3.6f + 7.2f * i, -row * 2.1f);
                    GameObject go = Instantiate(samplePrefab2, pos, Quaternion.identity);
                    SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
                    for (int j = 0; j < renderers.Length; j++)
                    {
                        renderers[j].sortingOrder += row * 10;
                    }
                }
            }
        }
        
        
    }

}
