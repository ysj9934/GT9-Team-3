using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("[GameInitializer] SaveManager가 아직 생성되지 않았습니다!");
            return;
        }

        // 1. SaveManager 로드
        SaveManager.Instance.Load();

        if (ResourceManager.Instance == null)
        {
            Debug.LogError("[GameInitializer] ResourceManager가 아직 생성되지 않았습니다!");
            return;
        }

        // 2. ResourceManager 초기화
        ResourceManager.Instance.ApplySavedResources();
        ResourceManager.Instance.ApplyOfflineRecovery();
    }
}