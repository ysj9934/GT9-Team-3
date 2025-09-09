using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for SceneManager
using System.Text;  // Required for StringBuilder

public class Debug_HHC : MonoBehaviour
{
    private static Debug_HHC instance;

    [Header("Canvas Name to Check in Scene")]
    public string canvasName = "LobbyScreen"; // Canvas name to find in Hierarchy

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
        // Optional update logic
    }
}