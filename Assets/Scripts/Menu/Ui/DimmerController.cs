using Core.Event;
using Core.Service;
using UnityEngine;

namespace Menu.Ui
{
    public class DimmerController : MonoBehaviour
    {
        [SerializeField] private GameObject _dimmer;
        
        private IEventDispatcher _eventDispatcher;
        
        private void Start()
        {
            _eventDispatcher = ServiceLocator.Instance.Get<IEventDispatcher>();
            
            _eventDispatcher.Subscribe(GameEventType.EnableDimmer, OnEnableDimmer);
            _eventDispatcher.Subscribe(GameEventType.DisableDimmer, OnDisableDimmer);
        }

        private void OnDestroy()
        {
            _eventDispatcher.Unsubscribe(GameEventType.EnableDimmer, OnEnableDimmer);
            _eventDispatcher.Unsubscribe(GameEventType.DisableDimmer, OnDisableDimmer);
        }
        
        private void OnDisableDimmer(IEvent e)
        {
            _dimmer.SetActive(false);
        }

        private void OnEnableDimmer(IEvent e)
        {
            _dimmer.SetActive(true);
        }
    }
}