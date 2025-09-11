using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Controller
    public WaveController _waveController;
    public TileController _tileController;
    public ShopController _shopController;

    public Transform baseTransform;

    // Object Info
    public int worldLevel;
    public int stageLevel;
    public int roundLevel;
    public int waveLevel;
    public int mapExtendLevel = 1;
    public bool isHardMode;
    public List<Wave_DataTable> stageWaveList = new List<Wave_DataTable>();
    [SerializeField] public BackgroundData background;

    // 게임 일시정지 및 종료
    public bool isGamePaused = false;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _waveController = GetComponentInChildren<WaveController>();
        _tileController = GetComponentInChildren<TileController>();
        _shopController = GetComponentInChildren<ShopController>();
    }

    private void Start()
    {
        ReceiveStageData();
    }

    public void DestroyOfType<T>() where T : Component
    {
        T[] objs = GameObject.FindObjectsOfType<T>();

        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }
    }


    /// <summary>
    /// 스테이지 정보 습득
    /// </summary>
    // Load data from DataManager
    public void ReceiveStageData()
    {
        StageData stageData = DataManager.Instance.SendStageData();

        if (stageData != null)
        {
            // 0. GameManager에 스테이지 정보 갱신
            // update stage information in GameManager
            worldLevel = stageData.worldCode;
            stageLevel = stageData.stageCode;
            stageWaveList = stageData.stageWaveList;
            isHardMode = stageData.isHardMode;

            // send stage data to HUDCanvas
            // 1. WaveController에 스테이지 정보 전달
            SendStageDataToWaveController();
            // 2. TileController에 스테이지 정보 전달
            SendStageDataToTileController();
            // 3. HUDCanvas에 스테이지 정보 전달
            SendStageDataToUIManager();

            // 4. 월드 레벨별 배경화면 변경
            background.UpdateWorldLevel(worldLevel);
        }
        else 
        {
            Debug.LogError("StageData is Null");
        }
    }

    /// <summary>
    /// 스테이지 정보 전달
    /// </summary>
    public void SendStageDataToWaveController()
    {
        _waveController.ReceiveStageData(
            new StageData
            (
                0,
                0,
                0,
                0,
                0,
                stageWaveList,
                isHardMode
            )
        );
    }

    public void SendStageDataToTileController()
    {
        _tileController.ReceiveStageData
            (
                new StageData
                (
                    0,
                    worldLevel,
                    stageLevel,
                    roundLevel,
                    waveLevel,
                    null,
                    isHardMode
                )
            );
    }

    public void SendStageDataToUIManager()
    {
        HUDCanvas.Instance._hudStageInfo.ReceiveStageData(
            new StageData
                (
                    0,
                    worldLevel,
                    stageLevel,
                    roundLevel,
                    waveLevel,
                    null,
                    isHardMode
                )
            );
    }

    /// <summary>
    /// 스테이지 정보 습득
    /// </summary>
    public void ReceiveStageDataFromWaveManager(StageData stageData)
    {
        if (stageData != null)
        {
            waveLevel = stageData.waveCode;
            roundLevel = stageData.roundCode;
            SendStageDataToUIManager();

            if (waveLevel == 7)
            {
                MapExtend();
            }
        }
        else
        {
            Debug.LogError("StageData is Null");
        }
    }

    /// <summary>
    /// 맵 크기 확장
    /// </summary>
    public void MapExtend()
    {
        SendStageDataToTileController();
    }

    // ==================== 게임 재시작 ==================== //

    /// <summary>
    /// 게임 재시작 (스테이지)
    /// </summary>
    public void ReStartStage()
    {

        // 1. GameManager 초기화
        ResetStageGameManager();

        // 2. 게임 재게
        HUDCanvas.Instance.SetGameSpeed3x();

        ReceiveStageData();
    }

    private void ResetStageGameManager()
    {
        worldLevel      = 0;
        stageLevel      = 0;
        roundLevel      = 0;
        waveLevel       = 0;
        mapExtendLevel  = 1;
        stageWaveList   = new List<Wave_DataTable>();
    }

    // ==================== 게임 일시정지 ==================== //

    /// <summary>
    /// Pause Game
    /// 게임 정지
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused = true;
    }

    /// <summary>
    /// Resume Game
    /// 게임 재개
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void GameSpeed2x()
    {
        Time.timeScale = 2f;
        isGamePaused = false;
    }

    public void GameSpeed3x()
    {
        Time.timeScale = 3f;
        isGamePaused = false;
    }

    // ==================== 게임 결과 ==================== //

    public void GameVictory()
    {
        Debug.Log("Victory");

        PauseGame();
        HUDCanvas.Instance._hudResultPanel._gameResultPanel.OpenWindow(true);

        // [사운드효과]: 게임 승리
        SoundManager.Instance.Play("11l-victory_sound_with_t-1749487402950-357606", SoundType.UI, 1f);
        Debug.LogWarning("[Sound]: Game Victory Sound");
    }

    public void GameDefeat()
    {
        Debug.Log("GameOver");

        _waveController.StopWave();

        PauseGame();
        isGameOver = true;

        HUDCanvas.Instance._hudResultPanel._gameDefeatPanel.OpenWindow();
        // [사운드효과]: 게임 패배
        SoundManager.Instance.Play("open-new-level-143027", SoundType.UI, 1f);
        Debug.LogWarning("[Sound]: Game Defeat Sound");
    }

    // 적 베이스 찾기 (김원진)
    // Find the enemy base transform in the scene
    public Vector3 BasePosition => baseTransform != null ? baseTransform.position : Vector3.zero;

    /// <summary>
    /// 치트
    /// </summary>
    public void Earn50G()
    {
        ResourceManager.Instance.Earn(ResourceType.Tilepiece, 50);
        HUDCanvas.Instance._hudResource.ShowTilePiece();
    }

    public void Earn0G(int amount)
    {
        ResourceManager.Instance.Earn(ResourceType.Tilepiece, amount);
        HUDCanvas.Instance._hudResource.ShowTilePiece();
    }

}

