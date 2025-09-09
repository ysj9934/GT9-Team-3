using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TileUI
/// created by: yoon
/// created at: 2025.08.18
/// description:
/// TileUI
/// </summary>

public class TileUI : MonoBehaviour
{
    private TileInfo _tileInfo;
    [SerializeField] public GameObject rotateUI;
    [SerializeField] public Image holdCircle;

    private void Awake()
    {
        CloseRotateUI();
        CloseLoadingUI();
    }

    private void Start()
    {
        _tileInfo = GetComponent<TileInfo>();
    }

    public void CloseRotateUI()
    {
        rotateUI.SetActive(false);

    }

    public void OpenLoadingUI()
    {
        holdCircle.gameObject.SetActive(true);
    }

    public void CloseLoadingUI()
    {
        holdCircle.gameObject.SetActive(false);
    }
}
