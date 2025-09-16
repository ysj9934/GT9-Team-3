using System.Linq;
using UnityEngine;

public class StoryLoader : MonoBehaviour
{
    private string filePath;
    private Sprite[] sprites;

    private int worldLevel;
    private int stageLevel;

    // 1. SaveManager 
    // 2. 영상을 봤는지 안봤는지 확인하기
    // 3. 본 경우. 안본경우
    // 

    public void SetStoryImage(int worldLevel, int stageLevel)
    {
        this.worldLevel = worldLevel;
        this.stageLevel = stageLevel;
        string worldStr = "World0" + this.worldLevel;
        string stageStr = "Stage0" + this.stageLevel;
        filePath = "Story/" + worldStr + "/" + stageStr;
        sprites = Resources.LoadAll<Sprite>(filePath);
        sprites.OrderBy(s => s.name).ToArray();

        SendData();
    }

    public void SendData()
    {
        GameUIManager.Instance.canvasWindow.storyView.RecieveData(sprites, worldLevel, stageLevel);
    }
}
