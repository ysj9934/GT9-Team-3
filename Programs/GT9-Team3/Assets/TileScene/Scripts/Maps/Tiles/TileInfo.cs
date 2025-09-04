using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : TileData
{
    // Tile Component
    public TilePlaceOnTower _tilePlaceOnTower;
    public PolygonCollider2D collider2D;
    
    // Tile Info
    public GameObject[] rotatedPrefabs;
    public TileDirector tileDirector;
    
    // Block Info
    public Dictionary<int, BlockInfo[]> blockInfos = new Dictionary<int, BlockInfo[]>();
    public Dictionary<SpriteRenderer, int> originBlockOrder = new Dictionary<SpriteRenderer, int>();
    private bool originOrderInitialized = false;
    public List<Tower1> hasTowerList = new List<Tower1>();
    public Dictionary<TowerCategory, int> towerInfo = new Dictionary<TowerCategory, int>()
    {
        { TowerCategory.Common, 0 },
        { TowerCategory.Splash, 0 },
        { TowerCategory.Slow, 0 },
        { TowerCategory.Stun, 0 },
        { TowerCategory.Doom, 0 }
    };

    // Tile WorldEffect
    public bool isTileLocked = false;
    public bool isBattlefieldModified = false;
    public bool isTowerSlow = false;
    
    protected override void Awake()
    {
        base.Awake();
        _tilePlaceOnTower = GetComponent<TilePlaceOnTower>();
        collider2D = GetComponent<PolygonCollider2D>();
    }

    /// <summary>
    /// Tile Initialize
    /// Tile 초기화
    /// </summary>
    /// <param name="pos">타일 위치</param>
    public override void Initialize(Vector2 pos)
    {
        base.Initialize(pos);
        CacheOriginOrders();
        SetBlockInfos();
        UpdateSpriteOrder();
    }

    public void Initialize(TileLink tileLink)
    {
        _tileLink = tileLink;
        _tileMove = GetComponent<TileMove>();
        CacheOriginOrders();
        SetBlockInfos();
    }

    /// <summary>
    /// Cache Block Origin Order
    /// 블럭의 최초 위치를 저장
    /// </summary>
    private void CacheOriginOrders()
    {
        if (originOrderInitialized) return;

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        originBlockOrder.Clear();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            originBlockOrder[sr] = sr.sortingOrder;
        }

        originOrderInitialized = true;
    }

    /// <summary>
    /// Set Block List
    /// 하위 블록 정보 저장
    /// </summary>
    private void SetBlockInfos()
    {
        for (int index = 0; index < rotatedPrefabs.Length; index++)
        {
            BlockInfo[] blockInfoArray = rotatedPrefabs[index].GetComponentsInChildren<BlockInfo>(true);
            blockInfos.Add(index, blockInfoArray);
        }
    }

    /// <summary>
    /// Set Block Infos
    /// 소유하고 있는 블럭을 저장
    /// </summary>
    /// <param name="pos">타일 위치</param>
    public override void UpdateMapping(Vector2 pos)
    {
        base.UpdateMapping(pos);
    }

    /// <summary>
    /// sorting order blocks
    /// 블록의 order를 위치값에 맞게 지정
    /// </summary>
    public void UpdateSpriteOrder()
    {
        foreach (var blockInfo in blockInfos)
        {
            BlockInfo[] biArray = blockInfo.Value;
            foreach (var bi in biArray)
            {
                SpriteRenderer[] spriteRenderers = bi.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var sr in spriteRenderers)
                {
                    bool isTower = sr.GetComponent<Tower1>() != null;
                    bool isRange = sr.GetComponent<TowerRange>() != null;

                    if (isTower)
                    {
                        //int towerOrder = 1000 + (tileIndex * 10);
                        //sr.sortingOrder = towerOrder;
                        int towerOrder = Mathf.RoundToInt(-transform.position.y * 100) + 1000; // Y값 기준으로 반전
                        sr.sortingOrder = towerOrder;
                    }
                    else if (isRange)
                    {
                        int towerRangeOrder = 1000 + (tileIndex * 10) - 1;
                        sr.sortingOrder = towerRangeOrder;
                    }
                    else
                    {
                        int baseOrder = originBlockOrder.ContainsKey(sr) ? originBlockOrder[sr] : 0;
                        sr.sortingOrder = baseOrder + (tileIndex * 10) - 1000;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Tile World Level
    /// World Level 갱신시 타일 성격 변화
    /// </summary>
    /// <param name="level">월드 레벨</param>
    public override void UpdateWorldLevel(int level)
    {
        for (int index = 0; index < blockInfos.Count; index++)
        {
            foreach (var blockInfo in blockInfos[index])
            {
                blockInfo.UpdateWorldLevel(level);
            }
        }
    }

    /// <summary>
    /// Tile MapExtend
    /// 맵 확장시 재배치
    /// </summary>
    /// <param name="pos">타일 위치</param>
    public void MapExtend(Vector2 pos)
    {
        base.Initialize(pos);
        UpdateSpriteOrder();
    }

    public void WorldTileGimmic(bool isLocked, bool isBattleModify, bool isTowerSlow)
    {
        if (isLocked) TileLocked();
        if (isBattleModify) TileBattleFieldModify();
        if (isTowerSlow) TileTowerSlow();
    }

    private void TileLocked()
    {
        isTileLocked = true;

        foreach (var blockInfo in blockInfos)
        {
            BlockInfo[] biArray = blockInfo.Value;
            foreach (var bi in biArray)
            {
                SpriteRenderer[] spriteRenderers = bi.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var sr in spriteRenderers)
                {
                    bool isTower = sr.GetComponent<Tower1>() != null;
                    bool isRange = sr.GetComponent<TowerRange>() != null;

                    if (isTower || isRange)
                    {
                        continue;
                    }

                    StartCoroutine(FadeToGray(sr, 3f));
                }
            }
        }
    }

    private IEnumerator FadeToGray(SpriteRenderer sr, float duration)
    {
        Color startColor = sr.color;
        Color targetColor = Color.gray;
        float timer = 0f;

        while (timer < duration)
        {
            sr.color = Color.Lerp(startColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        sr.color = targetColor;
    }

    private void TileBattleFieldModify()
    {
        isBattlefieldModified = true;

        foreach (var blockInfo in blockInfos)
        {
            BlockInfo[] biArray = blockInfo.Value;
            foreach (var bi in biArray)
            {
                SpriteRenderer[] spriteRenderers = bi.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var sr in spriteRenderers)
                {
                    bool isTower = sr.GetComponent<Tower1>() != null;
                    bool isRange = sr.GetComponent<TowerRange>() != null;

                    if (isTower || isRange)
                    {
                        continue;
                    }

                    StartCoroutine(FadeToRed(sr, 3f));
                }
            }
        }
    }

    private IEnumerator FadeToRed(SpriteRenderer sr, float duration)
    {
        Color startColor = sr.color;
        Color targetColor = Color.red;
        float timer = 0f;

        while (timer < duration)
        {
            sr.color = Color.Lerp(startColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        sr.color = targetColor;
    }

    private void TileTowerSlow()
    {
        isTowerSlow = true;

        foreach (var blockInfo in blockInfos)
        {
            BlockInfo[] biArray = blockInfo.Value;
            foreach (var bi in biArray)
            {
                SpriteRenderer[] spriteRenderers = bi.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var sr in spriteRenderers)
                {
                    bool isTower = sr.GetComponent<Tower1>() != null;
                    //bool isRange = sr.GetComponent<TowerRange>() != null;

                    if (isTower)
                    {
                        Tower1 tower = sr.GetComponent<Tower1>();
                        tower.towerdata.attackSpeed *= 80 / 100;
                        StartCoroutine(FadeToBlue(sr, 3f));
                    }

                    
                }
            }
        }
    }

    private IEnumerator FadeToBlue(SpriteRenderer sr, float duration)
    {
        Color startColor = sr.color;
        Color targetColor = Color.red;
        float timer = 0f;

        while (timer < duration)
        {
            sr.color = Color.Lerp(startColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        sr.color = targetColor;
    }
}
