using UnityEngine;

[CreateAssetMenu(fileName = "Background", menuName = "Maps/Background")]
public class Background : ScriptableObject
{
    [SerializeField] public Sprite[] sprites;
}
