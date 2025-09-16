using UnityEngine;
using UnityEngine.UI;

public class StoryView : MonoBehaviour
{
    [SerializeField] public Image image;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button nextButton;

    private Sprite[] sprites;
    private int sceneCount;
    private int sceneIndex = 0;

    private int worldLevel;
    private int stageLevel;

    private void Awake()
    {
        CloseWindow();
    }

    public void RecieveData(Sprite[] sprites, int worldLevel, int stageLevel)
    {
        this.sprites = null;
        this.sceneCount = 0;
        this.sceneIndex = 0;

        this.sprites = sprites;
        this.worldLevel = worldLevel;
        this.stageLevel = stageLevel;

        sceneCount = this.sprites.Length;

        if (this.sprites != null)
        {
            image.sprite = this.sprites[sceneIndex];
            sceneIndex++;

            OpenWindow();
        }
        else 
        {
            Debug.LogWarning("Don't have Story.");
        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    public void NextScene()
    {
        if (sceneIndex < sceneCount)
        {
            if (worldLevel == 1 && 
                stageLevel == 5 &&
                sceneIndex == 2)
            {
                CloseWindow();
                image.sprite = sprites[sceneIndex];
                sceneIndex++;
            }
            else
            {
                image.sprite = sprites[sceneIndex];
                sceneIndex++;
            }
            
        }
        else
        {
            if (worldLevel == 1 &&
                stageLevel == 5 &&
                sceneCount == sceneIndex)
            {

                CloseWindow();

                // [사운드효과]: 게임 승리
                SoundManager.Instance.Play("11l-victory_sound_with_t-1749487402950-357606", SoundType.UI, 1f);
                Debug.LogWarning("[Sound]: Game Victory Sound");
            }
            else 
            {
                CloseWindow();
            }
                
        }
    }

    public void SkipScene()
    {
        if (worldLevel == 1 &&
            stageLevel == 5 &&
            sceneIndex < 2)
        {
            sceneIndex = 2;
            image.sprite = sprites[sceneIndex];
            sceneIndex++;
        }
        else if (
            worldLevel == 1 &&
            stageLevel == 5 &&
            sceneIndex > 2)
        {
            // [사운드효과]: 게임 승리
            SoundManager.Instance.Play("11l-victory_sound_with_t-1749487402950-357606", SoundType.UI, 1f);
            Debug.LogWarning("[Sound]: Game Victory Sound");
        }

        CloseWindow();

    }
}
