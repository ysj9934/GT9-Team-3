using System.IO;
using UnityEngine;
using System.Collections.Generic;   // List<> 사용
using System; // [NonSerialized] 사용을 위해 필요

//list to save
[System.Serializable]
public class SaveData
{
    [ReadOnly] public int mana;  
    [ReadOnly] public int gold;
    [ReadOnly] public int crystal;

    // 직렬화용 리스트
    [SerializeField] private StageClearStarListWrapper stageClearStarsWrapper = new StageClearStarListWrapper();

    // 런타임 Dictionary
    [NonSerialized] public Dictionary<int, ClearStar> stageClearStars = new Dictionary<int, ClearStar>();

    // 직렬화 전 변환
    public void PrepareForSave()
    {
        stageClearStarsWrapper.items.Clear();
        foreach (var kvp in stageClearStars)
        {
            stageClearStarsWrapper.items.Add(new StageClearStar { stageID = kvp.Key, clearStar = kvp.Value });
        }
    }

    // 로드 후 변환
    public void LoadFromSerialized()
    {
        stageClearStars.Clear();
        foreach (var scs in stageClearStarsWrapper.items)
        {
            stageClearStars[scs.stageID] = scs.clearStar;
        }
    }
}

[System.Serializable]
public class StageClearStar
{
    public int stageID;
    public ClearStar clearStar; // HUD에서 전달받아 저장.ClearStar는 HUDresource.cs에 정의되어 있음
}

[System.Serializable]
public class StageClearStarListWrapper
{
    public List<StageClearStar> items = new List<StageClearStar>();
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;
    public SaveData data = new SaveData();

    public event Action OnLoaded; // SaveManager에서 Load가 끝난 뒤에 호출

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/save.json";
            //플랫폼에 따라 OS에서 제공하는 앱 전용 저장소 경로
            //읽기/쓰기 모두 가능하고, 앱 삭제 전까지 보존됨
            //APK(안드로이드)에서 실행 → 반드시 Application.persistentDataPath 사용해야 함

            //savePath = Path.Combine(Application.dataPath, "Resources/Save/save.json");
            //Editor에서 실행

            Load();
            Debug.Log("저장 경로: " + savePath);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
        ResourceManager.Instance.OnResourceChanged += (type, value) => Save();
    }

    void OnDisable()
    {
        ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
    }

    private void HandleResourceChanged(ResourceType type, float value)
    {
        switch (type)
        {
            case ResourceType.Gold:
                data.gold = (int)value;
                break;
            case ResourceType.Mana:
                data.mana = (int)value;
                break;
            case ResourceType.Crystal:
                data.crystal = (int)value;
                break;
        }
        Save();
    }

    public void Save()
    {
        //string json = JsonUtility.ToJson(data, true);

        //// 디렉토리가 존재하지 않으면 생성
        //string directory = Path.GetDirectoryName(savePath);
        //if (!Directory.Exists(directory))
        //{
        //    Directory.CreateDirectory(directory);
        //}

        //File.WriteAllText(savePath, json);
        //Debug.Log("Saved to: " + savePath);

        data.PrepareForSave(); // Dictionary → List
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        Debug.Log("[SaveManager] Load() 호출됨");
        Debug.Log("[SaveManager] 저장 경로 = " + savePath);

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("[SaveManager] mana = " + data.mana);
            Debug.Log("[SaveManager] gold = " + data.gold);
            Debug.Log("[SaveManager] crystal = " + data.crystal);
            data.LoadFromSerialized(); // List → Dictionary
        }
        //else
        //{
        //    data.mana = 0;
        //    data.gold = 0; // 저장 파일이 없으면 기본값 0 설정
        //    data.crystal = 0;
        //    Debug.Log("[SaveManager] save.json 없음");
        //}
        OnLoaded?.Invoke();

        // ResourceManager가 null이 아니면 바로 적용
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.ApplySavedResources();
    }

    // 기존에 리스트를 순회하며 찾던 방식(9월 14일 이전)
    //public void SaveStageClearStar(int stageID, ClearStar star)
    //{
    //    // 이미 저장되어 있으면 업데이트
    //    var existing = data.stageClearStars.Find(s => s.stageID == stageID);
    //    if (existing != null)
    //    {
    //        existing.clearStar = star;
    //    }
    //    else
    //    {
    //        data.stageClearStars.Add(new StageClearStar { stageID = stageID, clearStar = star });
    //    }
    //    Save();
    //}

    // Dictionary를 사용하여 바로 매핑하는 방식(9월 14일 이후) O(n) → O(1)로 최적화
    // 저장/업데이트
    public void SaveStageClearStar(int stageID, ClearStar star)
    {
        // 존재 여부 상관없이 바로 저장/업데이트
        data.stageClearStars[stageID] = star;

        // 변경 즉시 저장
        Save();
    }

    public ClearStar GetStageClearStar(int stageID)
    {
        if (data.stageClearStars.TryGetValue(stageID, out var star))
            return star;

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