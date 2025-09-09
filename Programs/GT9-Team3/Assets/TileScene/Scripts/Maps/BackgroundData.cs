using UnityEngine;

public class BackgroundData : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Background background;

    public void UpdateWorldLevel(int level)
    {
        if (level < 4)
            spriteRenderer.sprite = background.sprites[level - 1];
        else
            spriteRenderer.sprite = background.sprites[2];
    }
}
