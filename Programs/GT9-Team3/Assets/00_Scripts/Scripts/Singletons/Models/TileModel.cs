using UnityEngine;

[CreateAssetMenu(fileName = "RoadTile", menuName = "Maps/RoadTile")]
public class TileModel : ScriptableObject
{
    public TileCategory tileCategory;
    public TileShape tileShape;
    public int towerSlotCount;
    public GameObject tilePrefab;
}
