using UnityEngine;
using UnityEngine.UI;

public class StageStarDisplay : MonoBehaviour
{
    [Header("스테이지 ID")]
    public int stageID; // <- 여기 추가

    [Header("별 슬롯 (3칸)")]
    public Image[] starSlots; // 0,1,2 총 3칸

    [Header("별 Sprites")]
    public Sprite filledStar;
    public Sprite emptyStar;

    // 현재 별 개수만큼 칠해주기
    public void UpdateStarUI(int starCount)
    {
        for (int i = 0; i < starSlots.Length; i++)
        {
            if (i < starCount)
                starSlots[i].sprite = filledStar; // 채워진 별
            else
                starSlots[i].sprite = emptyStar;  // 빈 별
        }
    }
}