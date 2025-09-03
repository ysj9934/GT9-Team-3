using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AdsManager;

public class HUDCanvas : MonoBehaviour
{
    // Managers
    public GameManager _gameManager;

    private bool isTripleSpeedUnlocked = false;

    public static HUDCanvas Instance { get; private set; }


    // StageInfoHUD
    [SerializeField] private TextMeshProUGUI worldPanelText;
    [SerializeField] private TextMeshProUGUI stagePanelText;
    [SerializeField] private TextMeshProUGUI roundPanelText;

    // Castle & Resource
    private Castle _castle;
    [SerializeField] private Slider castleHealthSlider;
    [SerializeField] private TextMeshProUGUI castleHealthText;
    private float healthPercent;

    [SerializeField] TextMeshProUGUI resourceTilePieceAmountText;

    // WaveStartButton
    [SerializeField] Button pathfinderBtn;
    private Image pathfinderImage;
    [SerializeField] Button waveStartBtn;
    private Image waveStartImage;

    // GameSpeed
    [SerializeField] Button gameSpeed1xBtn;
    [SerializeField] Button gameSpeed2xBtn;
    [SerializeField] Button gameSpeed3xBtn;
    // GamePause
    [SerializeField] Button pauseBtn;

    // DefeatPanel
    public GameDefeat _gameDefeatPanel;

    // ResultPanel
    public GameResult _gameResultPanel;

    public TowerUpgradeUI upgradeUI;
    public TowerSellUI sellUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (sellUI != null) sellUI.Hide();

        pathfinderImage = pathfinderBtn.GetComponent<Image>();
        waveStartImage = waveStartBtn.GetComponent<Image>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;

        gameSpeed2xBtn.gameObject.SetActive(false);
        gameSpeed3xBtn.gameObject.SetActive(false);

        _gameDefeatPanel = GetComponentInChildren<GameDefeat>();
        _gameDefeatPanel.Initialize(this);
        _gameResultPanel = GetComponentInChildren<GameResult>();
        _gameResultPanel.Initialize(this);

        if (IsValidate())
        {
            ShowTilePiece();

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

    // StageInfoHUD
    public void ReceiveStageData(StageData stageData)
    {
        if (stageData != null)
        {
            worldPanelText.text = $"{stageData.worldCode}";
            stagePanelText.text = $"{stageData.stageCode}";
            roundPanelText.text = $"{stageData.roundCode}";
        }
        else
        {
            Debug.LogError("StageData is Null");
        }
    }

    // Castle & Resource
    public Castle SetCastleData(Castle castle)
    {
        return _castle = castle;
    }

    public void UpdateHPBar()
    {
        castleHealthText.text = $"{_castle.currentHealth}/{_castle.maxHealth}";
        healthPercent = (float)_castle.currentHealth / _castle.maxHealth;
        castleHealthSlider.value = healthPercent;
    }

    public void ShowTilePiece()
    {
        resourceTilePieceAmountText.text = $"{ResourceManager.Instance.GetAmount(ResourceType.Tilepiece)}";
    }

    public void TurnOnPathfinder()
    {
        Debug.Log("TurnOnPathfinder");
        pathfinderImage.color = new Color(0f, 0f, 0f);
        pathfinderBtn.interactable = true;
    }

    public void TurnOffPathfinder()
    {
        Debug.Log("TurnOffPathfinder");
        pathfinderImage.color = new Color(0.7f, 0.7f, 0.7f);
        pathfinderBtn.interactable = false;
    }

    public void TurnOnStartWave()
    {
        Debug.Log("TurnOnStartWave");
        waveStartImage.color = new Color(0f, 0f, 0f);
        waveStartBtn.interactable = true;
    }

    public void TurnOffStartWave()
    {
        Debug.Log("TurnOffStartWave");
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
        gameSpeed3xBtn.gameObject.SetActive(false);
        _gameManager.GameSpeed2x();

    }

    public void SetGameSpeed2x()
    {
        if (isTripleSpeedUnlocked)
        {
            ActivateTripleSpeed();
        }
        else
        {
            AdsManager.Instance.ShowRewardedAd(RewardAdType.SpeedBoost, () =>
            {
                Debug.Log("광고 시청 완료 -> 3배속");
                isTripleSpeedUnlocked = true;
                ActivateTripleSpeed();
            });
        }
    }

    private void ActivateTripleSpeed()
    {
        gameSpeed1xBtn.gameObject.SetActive(false);
        gameSpeed2xBtn.gameObject.SetActive(false);
        gameSpeed3xBtn.gameObject.SetActive(true);

        _gameManager.GameSpeed3x();
    }

    public void SetGameSpeed3x()
    {
        gameSpeed1xBtn.gameObject.SetActive(true);
        gameSpeed2xBtn.gameObject.SetActive(false);
        gameSpeed3xBtn.gameObject.SetActive(false);
        _gameManager.ResumeGame();
    }

    // GamePause
    public void SetGamePause()
    {
        _gameManager.PauseGame();
        gameSpeed1xBtn.gameObject.SetActive(false);
        gameSpeed2xBtn.gameObject.SetActive(false);
        gameSpeed3xBtn.gameObject.SetActive(true);

        //SetGameSpeed3x();
    }

    //public void ForceTripleSpeed()
    //{
    //    isTripleSpeedUnlocked = true;

    //    ActivateTripleSpeed();

    //    Debug.Log("광고 닫힘 후 강제 3배속 적용");
    //}

    public bool IsTripleSpeedUnlocked()
    {
        return isTripleSpeedUnlocked;
    }

}
