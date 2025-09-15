using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AdsManager;

public class GameSpeed : MonoBehaviour
{
    [SerializeField] public Button gameSpeedButton;
    [SerializeField] public TextMeshProUGUI gameSpeedText;
    private int gameSpeed = 1;
    private bool isTripleSpeedUnlocked = false;

    public void UpdateGameSpeed(int gameSpeed)
    {
        GameManager _gameManager = GameManager.Instance;

        switch (gameSpeed)
        {
            case 1:
                this.gameSpeed = 1;
                GameSpeed1x(_gameManager);
                break;
            case 2:
                this.gameSpeed = 2;
                GameSpeed2x(_gameManager);
                break;
            case 3:
                this.gameSpeed = 3;
                GameSpeed3x(_gameManager);
                break;
        }
    }

    private void GameSpeed1x(GameManager _gameManager)
    {
        _gameManager.ResumeGame();
        gameSpeedText.text = "X 1";
    }

    private void GameSpeed2x(GameManager _gameManager)
    {
        _gameManager.GameSpeed2x();
        gameSpeedText.text = "X 2";
    }

    private void GameSpeed3x(GameManager _gameManager)
    {
        if (!isTripleSpeedUnlocked)
        {
            AdsManager.Instance.ShowRewardedAd(RewardAdType.SpeedBoost, () =>
            {
                isTripleSpeedUnlocked = true;
                _gameManager.PauseGame();
            },
            () =>
            {
                Debug.Log("광고 닫힘 → 게임 재개");

                StartCoroutine(ApplySpeedBoostDelayed(_gameManager));
            });
        }
        else
        {
            _gameManager.GameSpeed3x();
            gameSpeedText.text = "X 3";
        }
    }

    private IEnumerator ApplySpeedBoostDelayed(GameManager _gameManager)
    {
        yield return new WaitForEndOfFrame(); // 또는 yield return null;
        _gameManager.GameSpeed3x();
        gameSpeedText.text = "X 3";
    }

}
