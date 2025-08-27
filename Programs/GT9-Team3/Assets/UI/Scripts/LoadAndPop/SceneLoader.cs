using UnityEngine;
using UnityEngine.SceneManagement; // 씬 로드를 위해 필요

public class SceneLoader : MonoBehaviour
{
    // 버튼이나 이벤트에서 호출
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}