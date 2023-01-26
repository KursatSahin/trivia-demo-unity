using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Core.Event;
using Core.Service;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Network;
using UnityEngine.SceneManagement;

public class MenuViewController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button leaderBoardButton;

    [SerializeField] private LeaderboardController _leaderboardController;

    private List<LeaderboardEntry> _leaderboardList = new List<LeaderboardEntry>();
    private HttpLeaderboardRequestHelper _httpLeaderboardRequestHelper;
    private IEventDispatcher _eventDispatcher;

    private void Start()
    {
        _httpLeaderboardRequestHelper = new HttpLeaderboardRequestHelper();
        _eventDispatcher = ServiceLocator.Instance.Get<IEventDispatcher>();
        
        // TODO: if you want do some anims here
    }

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        leaderBoardButton.onClick.AddListener(OnLeaderBoardButtonClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
        leaderBoardButton.onClick.RemoveAllListeners();
    }

    private async void OnLeaderBoardButtonClicked()
    {
        _leaderboardList = await _httpLeaderboardRequestHelper.GetLeaderboard();

        _eventDispatcher.Fire(GameEventType.EnableDimmer);
        _leaderboardController.gameObject.SetActive(true);
        _leaderboardController.Initialize(_leaderboardList);
    }

    private void OnPlayButtonClicked()
    {
        LoadGameScene();
    }

    /// <summary>
    /// Load Game Scene
    /// </summary>
    private async void LoadGameScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(Strings.Scenes.Game);
        await UniTask.WaitUntil(() => asyncOperation.isDone);
    }
}