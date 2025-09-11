using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Maps/BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField] public BlockCategory blockCategory;
    [SerializeField] public Sprite[] sprites;
}
