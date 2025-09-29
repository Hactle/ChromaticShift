using UnityEngine;
using Input;

public class PanelController : MonoBehaviour
{
    [SerializeField] private DeathZoneManager _deathZoneManager;
    [Space(20)]
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _loosePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private PlayerInputHandler _playerInput;

    private LevelState _levelState;
    

    private void Awake()
    {
        _levelState = GetComponentInParent<LevelState>();
    }

    private void Start()
    {
        _winPanel.SetActive(false);
        _loosePanel.SetActive(false);
        _pausePanel.SetActive(false);
    }

    public void SwitchPausePanel()
    {
        if (_pausePanel.activeSelf)
        {
            _pausePanel.SetActive(false);
            _levelState.ResumeGame();
        }
        else if (!_pausePanel.activeSelf)
        {
            _pausePanel.SetActive(true);
            _levelState.PauseGamee();
        }
    }

    private void ShowWinPanel()
    {
        _winPanel.SetActive(true);
        _levelState.PauseGamee();
    }

    private void ShowLoosePanel()
    {
        _loosePanel.SetActive(true);
        _levelState.PauseGamee();
    }

    private void OnEnable()
    {
        LevelCompletePoint.OnLevelComplete += ShowWinPanel;
        _deathZoneManager.OnPlayerDeath += ShowLoosePanel;
        _playerInput.OnPausePress += SwitchPausePanel;
    }

    private void OnDisable()
    {
        LevelCompletePoint.OnLevelComplete -= ShowWinPanel;
        _deathZoneManager.OnPlayerDeath -= ShowLoosePanel;
        _playerInput.OnPausePress -= SwitchPausePanel;
    }
}
