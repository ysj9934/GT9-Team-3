using System.IO;
using UnityEngine;

//list to save
[System.Serializable]
public class SaveData
{
    public int gold;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;
    public SaveData data = new SaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/save.json";
            Load();
            Debug.Log("Save Path: " + savePath);
            Debug.Log(data.gold);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);

        // ??????????곸몵筌???밴쉐
        string directory = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(savePath, json);
        Debug.Log("Saved to: " + savePath);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            data.gold = 0; // 筌ㅼ뮇????쎈뻬 疫꿸퀡??첎?
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("write");
        Save();
    }
}