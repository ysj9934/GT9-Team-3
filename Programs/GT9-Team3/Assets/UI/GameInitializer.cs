using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private static GameInitializer instance;

    void Awake()
    {
        // 싱글톤 유지: 씬 전환 시 새로 생성되지 않도록
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGame();
    }

    private void InitializeGame()
    {
        // 1. SaveManager 준비 확인 및 로드
        if (SaveManager.Instance == null)
        {
            Debug.LogError("[GameInitializer] SaveManager가 아직 생성되지 않았습니다!");
            return;
        }
        SaveManager.Instance.Load();
        Debug.Log("[GameInitializer] SaveManager 로드 완료");

        // 2. ResourceManager 준비 확인 및 초기화
        if (ResourceManager.Instance == null)
        {
            Debug.LogError("[GameInitializer] ResourceManager가 아직 생성되지 않았습니다!");
            return;
        }
        ResourceManager.Instance.ApplySavedResources();
        ResourceManager.Instance.ApplyOfflineRecovery();
        Debug.Log("[GameInitializer] ResourceManager 초기화 완료");
    }
}