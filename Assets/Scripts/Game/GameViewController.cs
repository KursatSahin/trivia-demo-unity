using System;
using System.Collections.Generic;
using Containers;
using Core.Event;
using Core.Service;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Game.Ui;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Containers.ContainerFacade;

public class GameViewController : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _correctAnswerCountText;
    [SerializeField] private TMP_Text _wrongAnswerCountText;
    
    [SerializeField] private TMP_Text _questionNumberText;
    
    [SerializeField] private TMP_Text _questionText;
    
    [SerializeField] private AnswerButtonView[] _answerButtons;
    
    [SerializeField] TimerController _timerController;

    public int Score
    {
        get => _score;
        set
        {
            _score = Math.Max(0, value);
            _scoreText.text = $"{_score}";
        }
    }

    public int QuestionCounter
    {
        get => _questionCounter;
        set
        {
            _questionCounter = value;
            _questionNumberText.text = $"{_questionCounter}/{_totalQuestionsCount}";
        }
    }

    public int CorrectAnswerCount
    {
        get => _correctAnswerCount;
        set
        {
            _correctAnswerCount = value;
            _correctAnswerCountText.text = $"{_correctAnswerCount}";
        }
    }

    public int WrongAnswerCount
    {
        get => _wrongAnswerCount;
        set
        {
            _wrongAnswerCount = value;
            _wrongAnswerCountText.text = $"{_wrongAnswerCount}";
        }
    }

    private Queue<Question> _questions;
    private Question _currentQuestion = null;
    
    private int _score = 0;
    private int _questionCounter = 0;
    private int _correctAnswerCount = 0;
    private int _wrongAnswerCount = 0;
    private int _totalQuestionsCount = 0;

    private HttpQuestionsRequestHelper _httpQuestionsRequestHelper;
    private IEventDispatcher _eventDispatcher;

    // Start is called before the first frame update
    void Start()
    {
        _httpQuestionsRequestHelper = new HttpQuestionsRequestHelper();
        _eventDispatcher = ServiceLocator.Instance.Get<IEventDispatcher>();
        
        StartGame();
    }

    private void OnDestroy()
    {
        _eventDispatcher.Unsubscribe(GameEventType.OptionSelected, OnOptionSelected);
        _timerController._onTimeIsUp -= OnTimeIsUp;
    }

    private async void StartGame()
    {
        _eventDispatcher.Subscribe(GameEventType.OptionSelected, OnOptionSelected);
        
        _questions = new Queue<Question>(await _httpQuestionsRequestHelper.GetQuestions());
        _totalQuestionsCount = _questions.Count;

        if (_totalQuestionsCount <= 0)
        {
            Debug.LogError("Something went wrong, no questions found");
            return;
        }

        _timerController._onTimeIsUp += OnTimeIsUp;
        
        UpdateGameState();
    }

    private async void OnTimeIsUp()
    {
        Score -= ContainerFacade.ScoreConfigs.TimeoutPenalty;
        
        ShowCorrectAnswer();
        
        // wait for 1 second
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        UpdateGameState();
    }

    private async void OnOptionSelected(IEvent e)
    {
        if (e is not OptionSelectedEvent optionSelectedEvent)
            return;
        
        var currentSelectedButton = Array.Find(_answerButtons, x => x.OptionId.ToLower() == optionSelectedEvent.OptionId.ToLower());
        
        // set all answer buttons interactable false to prevent multiple clicks
        foreach (var answerButton in _answerButtons)
        {
            answerButton.SetButtonInteractable(false);
        }

        // stop timer
        _timerController.StopTimer();
        
        // wait for 1 second
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        
        // check if answer is correct
        if (optionSelectedEvent.OptionId.ToLower() == _currentQuestion.answer.ToLower())
        {
            // increase score
            Score += ContainerFacade.ScoreConfigs.CorrectAnswerScore;
            // increase correct answer count
            CorrectAnswerCount++;
            
            // show correct answer
            ShowCorrectAnswer();
        }
        else
        {
            // decrease score
            Score -= ContainerFacade.ScoreConfigs.WrongAnswerPenalty;
            
            // increase wrong answer count
            WrongAnswerCount++;
            
            // show wrong answer
            var wrongAnswerButton = Array.Find(_answerButtons, x => x.OptionId.ToLower() == optionSelectedEvent.OptionId.ToLower());
            wrongAnswerButton.SetButtonImage(ButtonState.Wrong);
            
            // show correct answer
            ShowCorrectAnswer();
        }
        
        // wait for 1 second
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        UpdateGameState();
    }

    private void ShowCorrectAnswer()
    {
        var correctAnswerButton = Array.Find(_answerButtons, x => x.OptionId.ToLower() == _currentQuestion.answer.ToLower());
        correctAnswerButton.SetButtonImage(ButtonState.Correct);
    }

    private void UpdateGameState()
    {
        if (GetNextQuestion())
        {
            SetQuestionView();
            _timerController.ResetTimer();
            _timerController.StartTimer();
        }
        else
        {
            LoadGameEnd();
        }
    }

    private bool GetNextQuestion()
    {
        if (_questions.Count > 0)
        {
            _currentQuestion = _questions.Dequeue();
            QuestionCounter++;
            
            return true;
        }

        return false;
    }
    
    private void SetQuestionView()
    {
        _questionText.text = _currentQuestion.question;

        for (int i = 0; i < 4; i++)
        {
            _answerButtons[i].SetData(_currentQuestion.choices[i]);
        }
    }
    
    private async void LoadGameEnd()
    {
        await UniTask.Delay(1000, false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());

        SceneManager.LoadSceneAsync(Strings.Scenes.EndGame,  LoadSceneMode.Additive);
    }
}