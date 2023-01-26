﻿using System.Collections.Generic;

namespace Core.Event
{
    public class EventDispatcher : IEventDispatcher
    {
        #region Private Fields
        private Dictionary<GameEventType, EventAction> _eventDictionary;
        #endregion

        #region Public Functions
        public void Subscribe(GameEventType gameEventType, EventAction listener)
        {
            if (_eventDictionary.TryGetValue(gameEventType, out EventAction listeners))
            {
                listeners += listener;
                _eventDictionary[gameEventType] = listeners;
            }
            else
            {
                _eventDictionary.Add(gameEventType, listener);
            }
        }

        public void Unsubscribe(GameEventType gameEventType, EventAction listener)
        {
            if (_eventDictionary.TryGetValue(gameEventType, out EventAction listeners))
            {
                listeners -= listener;
                _eventDictionary[gameEventType] = listeners;

                if (listeners == null)
                {
                    _eventDictionary.Remove(gameEventType);
                }
            }
        }

        public void Fire(GameEventType gameEventType, IEvent e = null)
        {
            if (_eventDictionary.TryGetValue(gameEventType, out EventAction listeners))
            {
                listeners.Invoke(e);
            }
        }

        public void Initialize()
        {
            _eventDictionary = new Dictionary<GameEventType, EventAction>();
        }
        
        #endregion
    }

    
}