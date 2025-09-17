using UnityEngine;
using UnityEngine.UI;

public class CanvasFixed : MonoBehaviour
{
    private GameUIManager _UIManager;

    [SerializeField] public StagePanel StagePanel;
    [SerializeField] public WavePanel WavePanel;
    [SerializeField] public ResourcePanel ResourcePanel;
    [SerializeField] public GameSpeed GameSpeedButton;

    // 패스파인더 / 웨이브 시작 버튼
    [SerializeField] private Button pathfinderBtn;
    [SerializeField] private Image pathfinderImage;
    [SerializeField] private Button waveStartBtn;
    [SerializeField] private Image waveStartImage;

    private void Awake()
    {
        _UIManager = GetComponentInParent<GameUIManager>();
    }

    /// <summary>
    /// 게임 포기를 물어보는 버튼
    /// </summary>
    public void GamePause()
    {
        // [사운드효과]: 버튼 클릭
        SoundManager.Instance.Play("button-press-382713", SoundType.UI, 1f);

        Debug.LogWarning("[Sound]: Button Click Sound");

        _UIManager.canvasPopup.confirmMessage.PopupUIShow(
            "[경고]",
            "해당 스테이지를 포기하고 바로 로비로 나가시겠습니까?\n" +
            "스테이지를 포기하면 현재까지 얻은 재화를 얻을 수 없습니다.",
            "로비로 나가기",
            "게임으로 돌아가기",
            () => {
                GameManager.Instance.ResumeGame();
                SceneLoader.Instance.LoadSceneByIndex(0);
            },
            () => {
                GameSpeedButton.UpdateGameSpeed(1);
            }
            );
    }

    /// <summary>
    /// 패스파인더 잡는 버튼
    /// </summary>
    public void SetPathfinder()
    {
        GameManager.Instance._tileController.ShowConnectedPath();

        // [사운드효과]: 패스파인더
        SoundManager.Instance.Play("success_bell-6776", SoundType.UI, 1f);
        Debug.LogWarning("[Sound]: Pathfinder Sound");
    }

    /// <summary>
    /// 웨이브 시작 버튼
    /// </summary>
    public void StartWave()
    {
        TurnOffPathfinder();
        GameManager.Instance._waveController.StartWave();

        // [사운드효과]: 버튼 클릭
        SoundManager.Instance.Play("minimal-pop-click-ui-14-198314", SoundType.UI, 0.3f);
        Debug.LogWarning("[Sound]: Button Click Sound");
    }

    public void TurnOnPathfinder()
    {
        //Debug.Log("TurnOnPathfinder");
        pathfinderImage.color = new Color(0f, 0f, 0f);
        pathfinderBtn.interactable = true;
    }

    public void TurnOffPathfinder()
    {
        //Debug.Log("TurnOffPathfinder");
        pathfinderImage.color = new Color(0.7f, 0.7f, 0.7f);
        pathfinderBtn.interactable = false;
    }

    public void TurnOnStartWave()
    {
        //Debug.Log("TurnOnStartWave");
        waveStartImage.color = new Color(0f, 0f, 0f);
        waveStartBtn.interactable = true;
    }

    public void TurnOffStartWave()
    {
        //Debug.Log("TurnOffStartWave");
        waveStartImage.color = new Color(0.7f, 0.7f, 0.7f);
        waveStartBtn.interactable = false;
    }
}
