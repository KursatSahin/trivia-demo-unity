using UnityEngine;
using TMPro;
using System;
using Core.Event;
using Core.Service;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public event Action _onTimeIsUp;
    
    [SerializeField] private float _defaultTime = 20;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private bool _timerRunning = false;
    [SerializeField] private Slider _timeSlider;

    private float _timeLeft;

    private IEventDispatcher _eventDispatcher;

    private void Start()
    {
        _eventDispatcher = ServiceLocator.Instance.Get<IEventDispatcher>();
        ResetTimer();
    }

    void Update()
    {
        if (_timerRunning)
        {
            _timeLeft -= Time.deltaTime;
            _timerText.text = "Time Left: " + (int)_timeLeft;
            _timeSlider.value = _timeLeft / _defaultTime;

            if (_timeLeft < 0)
            {
                _timerRunning = false;
                _timerText.text = "Time's Up!";
                OnTimerEnd();
            }
        }
    }

    public void StartTimer()
    {
        _timerRunning = true;
    }

    public void StopTimer()
    {
        _timerRunning = false;
    }

    public void ResetTimer()
    {
        _timerRunning = false;
        _timeLeft = _defaultTime; // change this to the desired time
        _timerText.text = "Time Left: " + (int)_timeLeft;
        _timeSlider.value = 1;
    }

    private async void OnTimerEnd()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        _onTimeIsUp?.Invoke();
        //_eventDispatcher.Fire(GameEventType.TimeIsUp);
    }
}