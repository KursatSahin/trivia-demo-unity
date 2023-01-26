using System;
using Core.Event;
using Core.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui
{
    [RequireComponent(typeof(Button))]
    internal class AnswerButtonView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _optionIdText;
        [SerializeField] private TMP_Text _answerText;

        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _correctSprite;
        [SerializeField] private Sprite _wrongSprite;

        public string OptionId => _optionId;
        
        private Button _button;
        private string _optionId;
        private string _answer;

        private ButtonState _buttonState = ButtonState.Normal;

        private IEventDispatcher _eventDispatcher;

        private void Start()
        {
            _button = GetComponent<Button>();
            _eventDispatcher = ServiceLocator.Instance.Get<IEventDispatcher>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void SetData(string answer)
        {
            _optionId = answer.Substring(0, 1);
            _answer = answer.Substring(2);

            _optionIdText.text = _optionId;
            _answerText.text = _answer;

            _button.interactable = true;
            SetButtonImage(ButtonState.Normal);
        }

        private void OnButtonClick()
        {
            SetButtonImage(ButtonState.Selected);
            _eventDispatcher.Fire(GameEventType.OptionSelected, new OptionSelectedEvent(_optionId));
        }

        public void SetButtonInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }

        public void SetButtonImage(ButtonState state)
        {
            _buttonState = state;

            switch (_buttonState)
            {
                case ButtonState.Normal:
                    _button.image.sprite = _normalSprite;
                    break;
                case ButtonState.Selected:
                    _button.image.sprite = _selectedSprite;
                    break;
                case ButtonState.Correct:
                    _button.image.sprite = _correctSprite;
                    break;
                case ButtonState.Wrong:
                    _button.image.sprite = _wrongSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum ButtonState
    {
        Normal,
        Selected,
        Correct,
        Wrong
    }
}