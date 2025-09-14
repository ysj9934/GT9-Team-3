using System.IO;
using UnityEngine;
using System.Collections.Generic;   // List<> 사용

//list to save
[System.Serializable]
public class SaveData
{
    [ReadOnly] public int mana;  
    [ReadOnly] public int gold;
    [ReadOnly] public List<StageClearStar> stageClearStars = new List<StageClearStar>();  // Stage_ID별 ClearStar 저장
}

[System.Serializable]
public class StageClearStar
{
    public int stageID;
    public ClearStar clearStar; // HUD에서 전달받아 저장.ClearStar는 HUDresource.cs에 정의되어 있음
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

            //savePath = Application.persistentDataPath + "/save.json";
            //플랫폼에 따라 OS에서 제공하는 앱 전용 저장소 경로
            //읽기/쓰기 모두 가능하고, 앱 삭제 전까지 보존됨
            //APK(안드로이드)에서 실행 → 반드시 Application.persistentDataPath 사용해야 함

            // 저장 경로를 Assets/Resources/Save 폴더로 지정
            savePath = Path.Combine(Application.dataPath, "Resources/Save/save.json");
            //Android 빌드 후 APK 내부 Assets 폴더를 가리키는데, 읽기 전용입니다.

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

        // 디렉토리가 존재하지 않으면 생성
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
            data.mana = 0;
            data.gold = 0; // 저장 파일이 없으면 기본값 0 설정
        }
    }

    public void SaveStageClearStar(int stageID, ClearStar star)
    {
        // 이미 저장되어 있으면 업데이트
        var existing = data.stageClearStars.Find(s => s.stageID == stageID);
        if (existing != null)
        {
            existing.clearStar = star;
        }
        else
        {
            data.stageClearStars.Add(new StageClearStar { stageID = stageID, clearStar = star });
        }
        Save();
    }

    public ClearStar GetStageClearStar(int stageID)
    {
        var existing = data.stageClearStars.Find(s => s.stageID == stageID);
        if (existing != null) return existing.clearStar;
        return ClearStar.One; // 기본값
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("App Paused - Saving data");
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("write");
        Save();
    }
}