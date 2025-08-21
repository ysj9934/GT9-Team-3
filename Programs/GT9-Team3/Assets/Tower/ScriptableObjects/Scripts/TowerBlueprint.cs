using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tower Defense/Tower Blueprint")]
public class TowerBlueprint : ScriptableObject
{
    public string displayName;
    public Sprite icon;

    public GameObject towerPrefab;
    public TowerData data;

    public ResourceType CostType => data.makeCost;
    public int CostValue => data.makeValue;
}
