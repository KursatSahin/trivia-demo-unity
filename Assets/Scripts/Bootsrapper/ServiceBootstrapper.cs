using Core.Event;
using Core.Service;
using UnityEngine;

namespace Bootstrapper
{
    public class ServiceBootstrapper
    {
        private const int TweenersCapacity = 1024;
        private const int SequencesCapacity = 256;
        
        /// <summary>
        /// ServiceLocator instance
        /// </summary>
        private static readonly ServiceLocator _serviceProvider = ServiceLocator.Instance;

        /// <summary>
        /// This function is called AfterAssembliesLoaded automatically with this attribute
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void AfterAssembliesLoaded()
        {
            DG.Tweening.DOTween.defaultRecyclable = true;
            DG.Tweening.DOTween.SetTweensCapacity(TweenersCapacity, SequencesCapacity);
            
            // Add services to service provider below
            RegisterEventDispatcher();
            
            // Add models to model provider below
        }
        
        /// <summary>
        /// Register EventDispatcher as a service
        /// </summary>
        private static void RegisterEventDispatcher()
        {
            var eventDispatcher = new EventDispatcher();
            eventDispatcher.Initialize();
            _serviceProvider.RegisterService<IEventDispatcher>(eventDispatcher);
        }
    }
}