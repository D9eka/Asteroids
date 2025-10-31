using System.Collections.Generic;
using System.ComponentModel;
using Asteroids.Scripts.Input;
using Asteroids.Scripts.Spawning.Enemies.Core;
using Zenject;

namespace Asteroids.Scripts.Pause
{
    public class PauseSystem : IPauseSystem, IInitializable
    {
        private readonly DiContainer _container;
        private readonly HashSet<IPausable> _pausables = new();
        
        private List<ITickableSystem> _tickableSystems;
        
        public bool IsPaused { get; private set; }

        public PauseSystem(DiContainer container)
        {
            _container  = container;
        }

        public void Initialize()
        {
            _tickableSystems = _container.ResolveAll<ITickableSystem>();
        }
        
        public void Pause()
        {
            if (IsPaused) return;
            IsPaused = true;

            foreach (var tickableSystem in _tickableSystems)
            {
                tickableSystem.Disable();
            }

            foreach (var pausable in _pausables)
            {
                pausable.Pause();
            }
        }

        public void Resume()
        {
            if (!IsPaused) return;
            IsPaused = false;

            foreach (var tickableSystem in _tickableSystems)
            {
                tickableSystem.Enable();
            }

            foreach (var pausable in _pausables)
            {
                pausable.Resume();
            }
        }

        public void Register(IPausable pausable)
        {
            if (pausable == null) return;
            _pausables.Add(pausable);

            if (IsPaused)
                pausable.Pause();
        }

        public void Unregister(IPausable pausable)
        {
            _pausables.Remove(pausable);
        }
    }
}