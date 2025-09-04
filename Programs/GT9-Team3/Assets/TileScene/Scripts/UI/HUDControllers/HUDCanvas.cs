using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDCanvas : MonoBehaviour
{
    // Managers
    public GameManager _gameManager;
    public TileManager _tileManager;

    public static HUDCanvas Instance { get; private set; }

    // StageInfoHUD
    public HUDStageInfo _hudStageInfo;

    // HUDWaveInfo
    public HUDWaveInfo _hudWaveInfo;

    // HUDResource
    public HUDResource _hudResource;

    // WaveStartButton
    [SerializeField] Button pathfinderBtn;
    private Image pathfinderImage;
    [SerializeField] Button waveStartBtn;
    private Image waveStartImage;

    // GameSpeed
    [SerializeField] Button gameSpeed1xBtn;
    [SerializeField] Button gameSpeed2xBtn;
    [SerializeField] Button gameSpeed5xBtn;
    // GamePause
    [SerializeField] Button pauseBtn;

    // ItemHubUI
    public ItemHubUI _itemHudUI;

    // HUDResultPanel
    public HUDResultPanel _hudResultPanel;

    // HUDMessageUI
    public HUDMessageUI _hudMessageUI;

    public TowerUpgradeUI upgradeUI;
    public TowerSellUI sellUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (sellUI != null) sellUI.Hide();

        pathfinderImage = pathfinderBtn.GetComponent<Image>();
        waveStartImage = waveStartBtn.GetComponent<Image>();

        _hudStageInfo = GetComponentInChildren<HUDStageInfo>();
        _hudWaveInfo = GetComponentInChildren<HUDWaveInfo>();
        _hudResource = GetComponentInChildren<HUDResource>();
        _itemHudUI = GetComponentInChildren<ItemHubUI>();
        _hudResultPanel = GetComponentInChildren<HUDResultPanel>();
        _hudMessageUI = GetComponentInChildren<HUDMessageUI>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _tileManager = TileManager.Instance;

        gameSpeed2xBtn.gameObject.SetActive(false);
        gameSpeed5xBtn.gameObject.SetActive(false);

        _hudStageInfo = GetComponentInChildren<HUDStageInfo>();
        _hudWaveInfo = GetComponentInChildren<HUDWaveInfo>();
        _hudResource = GetComponentInChildren<HUDResource>();
        _itemHudUI = GetComponentInChildren<ItemHubUI>();
        _hudResultPanel = GetComponentInChildren<HUDResultPanel>();
        _hudMessageUI = GetComponentInChildren<HUDMessageUI>();

        

        if (IsValidate())
        {
            _hudResource.ShowTilePiece();

            TurnOffStartWave();
        }
    }

    private bool IsValidate()
    {
        if (_gameManager == null)
        {
            ValidateMessage(_gameManager.name);
            return false;
        }
        else if (pathfinderImage == null)
        {
            ValidateMessage(pathfinderImage.name);
            return false;
        }
        else if (waveStartImage == null)
        {
            ValidateMessage(waveStartImage.name);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ValidateMessage(string obj)
    {
        Debug.LogError($"{obj} is Valid");
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

    // PathfindereButton
    public void SetPathfinder()
    {
        _gameManager._tileManager.ShowConnectedPath();
    }

    // WaveStartButton
    public void StartWave()
    { 
        TurnOffPathfinder();
        _gameManager._waveManager.StartWave();

    }

    // GameSpeed
    public void SetGameSpeed1x()
    {
        gameSpeed1xBtn.gameObject.SetActive(false);
        gameSpeed2xBtn.gameObject.SetActive(true);
        gameSpeed5xBtn.gameObject.SetActive(false);
        _gameManager.GameSpeed2x();
        
    }

    public void SetGameSpeed2x()
    {
        gameSpeed1xBtn.gameObject.SetActive(false);
        gameSpeed2xBtn.gameObject.SetActive(false);
        gameSpeed5xBtn.gameObject.SetActive(true);
        _gameManager.GameSpeed5x();
        
    }

    public void SetGameSpeed5x()
    {
        gameSpeed1xBtn.gameObject.SetActive(true);
        gameSpeed2xBtn.gameObject.SetActive(false);
        gameSpeed5xBtn.gameObject.SetActive(false);
        _gameManager.ResumeGame();
    }
    // GamePause
    public void SetGamePause()
    {
        _gameManager.PauseGame();
        gameSpeed1xBtn.gameObject.SetActive(false);
        gameSpeed2xBtn.gameObject.SetActive(false);
        gameSpeed5xBtn.gameObject.SetActive(true);

        //SetGameSpeed5x();
        _hudMessageUI.PopupUIShow(
            "[경고]",
            "해당 스테이지를 포기하고 바로 로비로 나가시겠습니까?\n" +
            "스테이지를 포기하면 현재까지 얻은 재화를 얻을 수 없습니다.",
            "로비로 나가기",
            "게임으로 돌아가기",
            () => { SceneLoader.Instance.LoadSceneByIndex(0); },
            () => { SetGameSpeed5x(); });
    }
}
