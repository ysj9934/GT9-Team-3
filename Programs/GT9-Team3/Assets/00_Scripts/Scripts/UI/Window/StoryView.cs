using UnityEngine;
using UnityEngine.UI;

public class StoryView : MonoBehaviour
{
    [SerializeField] public Image image;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button nextButton;

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }
}
