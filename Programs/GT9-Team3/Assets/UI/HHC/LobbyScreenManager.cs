using System.Collections.Generic; // List<T> 사용
using System.Linq;                // ToList() 사용
using UnityEngine;                // MonoBehaviour, Debug 등 사용

public class LobbyScreenManager : MonoBehaviour
{
    private List<StageStarDisplay> stageStars = new List<StageStarDisplay>();

    private void Start()
    {
        // 기존 GetComponentsInChildren 대신 씬 전체에서 StageStarDisplay 찾기
        stageStars = FindObjectsOfType<StageStarDisplay>(true).ToList();
        Debug.Log($"[LobbyScreenManager] {stageStars.Count}개의 스테이지 별 표시가 발견되었습니다.");
        RefreshAll();
    }

    public void RefreshAll()
    {
        foreach (var s in stageStars)
        {
            var data = SaveManager.Instance.data.stageClearStars.Find(d => d.stageID == s.stageID);
            if (data != null)
            {
                int starCount = (int)data.clearStar;
                s.UpdateStarUI(starCount + 1);
            }
            else
            {
                s.UpdateStarUI(0);
            }
        }
    }
}