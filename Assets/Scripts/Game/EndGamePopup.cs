using Core.Event;
using Core.Service;
using Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGamePopup : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private GameObject _endGamePopupRect;

    [Header("Buttons")]
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _restartButton;
    
    [Header("Button Texts")]
    [SerializeField] private TextMeshProUGUI _homeButtonText;
    [SerializeField] private TextMeshProUGUI _restartButtonText;
    
    [Header("Titles")]
    [SerializeField] private TextMeshProUGUI _gameResultTitle;
    #endregion

    private IEventDispatcher _eventDispatcher;
    
    #region Unity Events
        
    private void OnEnable()
    {
        _homeButton.onClick.AddListener(OnHomeButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void Start()
    {
        _eventDispatcher = ServiceLocator.Instance.Get<IEventDispatcher>();
        
        _homeButtonText.text = Strings.EndGameUI.HomeButtonText;
        _restartButtonText.text = Strings.EndGameUI.RestartButtonText;
        _gameResultTitle.text = Strings.EndGameUI.CongratsText;
    }

    private void OnDisable()
    {
        _homeButton.onClick.RemoveListener(OnHomeButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
    }

    #endregion

    #region Private Functions
        
    /// <summary>
    /// Restart button OnClick event handler
    /// </summary>
    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(Strings.Scenes.Game);
    }

    /// <summary>
    /// Home button OnClick event handler
    /// </summary>
    private void OnHomeButtonClicked()
    {
        SceneManager.LoadScene(Strings.Scenes.Home);
    }
        
    #endregion
}