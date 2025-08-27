using UnityEngine;
using UnityEngine.SceneManagement; // �� �ε带 ���� �ʿ�

public class SceneLoader : MonoBehaviour
{
    // ��ư�̳� �̺�Ʈ���� ȣ��
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}