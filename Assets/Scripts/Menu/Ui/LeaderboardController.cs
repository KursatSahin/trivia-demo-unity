using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Core.Event;
using Core.Service;
using Lean.Pool;

public class LeaderboardController : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    [SerializeField] private LoopScrollRect _loopScrollRect;
    [SerializeField] private GameObject _leaderboardEntryPrefab;
    [SerializeField] private int totalCount = -1;
    
    [SerializeField] private Button _closeButton;

    private List<LeaderboardEntry> _leaderboardEntries;
    private IEventDispatcher _eventDispatcher;

    private void Start()
    {
        _eventDispatcher = ServiceLocator.Instance.Get<IEventDispatcher>();
    }
    
    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
        _eventDispatcher.Fire(GameEventType.DisableDimmer);
    }
    
    public GameObject GetObject(int index)
    {
        return LeanPool.Spawn(_leaderboardEntryPrefab);    
    }

    public void ReturnObject(Transform trans)
    {
        LeanPool.Despawn(trans);
    }

    public void ProvideData(Transform transform, int idx)
    {
        transform.GetComponent<LeaderboardItemView>().SetLeaderboardItemData(_leaderboardEntries[idx]);
    }

    public void Initialize(List<LeaderboardEntry> leaderboardList)
    {
        _leaderboardEntries = leaderboardList;
        _leaderboardEntries.Sort();
        
        _loopScrollRect.prefabSource = this;
        _loopScrollRect.dataSource = this;
        _loopScrollRect.totalCount = totalCount = _leaderboardEntries.Count;
        _loopScrollRect.RefillCells();
    }
}
