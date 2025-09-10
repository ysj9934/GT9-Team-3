using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리
using System.Text;  // 텍스트 관리
using TMPro; // TextMeshPro 사용

public class Debug_HHC : MonoBehaviour
{
    private static Debug_HHC instance;

    [Header("연결된 캔버스")]
    public string canvasName = "LobbyScreen"; 

    [Header("연결된 UI 텍스트")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI crystalText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private void OnEnable()
    {
        // Register callback when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister callback when this object is disabled or destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // Confirm Scene and Canvas
    {
        if (scene.name == "Map UI")
        {
            GameObject lobbyCanvas = GameObject.Find(canvasName);

            if (lobbyCanvas != null)
            {
                Debug.Log($"'{scene.name}' Scene : O. '{canvasName}' Canvas : O");
            }
            else
            {
                Debug.LogWarning($"'{scene.name}' Scene : O. '{canvasName}' Canvas : X");
            }
        }
        else
        {
            Debug.Log($"'{scene.name}' Scene : X. '{canvasName}' Canvas : X");
        }
    }

    //// Example logging
    //void Start()
    //{
    //    StringBuilder debug_all = new StringBuilder();
    //
    //    debug_all.AppendLine("---------------------------------------------------------------------------------------------------------------------------------");
    //    debug_all.AppendLine("Debug Info Start");
    //    Debug.Log(debug_all.ToString());
    //}

    void Update()
    {
        if (ResourceManager.Instance == null) return;

        // ResourceManager 에서 현재 자원값 읽어서 UI에 표시
        float gold = ResourceManager.Instance.GetAmount(ResourceType.Gold);
        float crystal = ResourceManager.Instance.GetAmount(ResourceType.Crystal);

        //goldText.text = $"Gold: {gold}";
        //crystalText.text = $"Crystal: {crystal}";
    }
}