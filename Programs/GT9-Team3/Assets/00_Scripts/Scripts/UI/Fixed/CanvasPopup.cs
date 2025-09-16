using UnityEngine;

public class CanvasPopup : MonoBehaviour
{
    [SerializeField] public ToastMessage toastMessage;
    [SerializeField] public ConfirmMessage confirmMessage;
    [SerializeField] public GameDefeat gameDefeatPanel;
    [SerializeField] public GameResult gameResultPanel;
}
