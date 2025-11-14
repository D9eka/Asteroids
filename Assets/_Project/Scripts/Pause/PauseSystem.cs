using System.Collections.Generic;

namespace Asteroids.Scripts.Pause
{
    public class PauseSystem : IPauseSystem
    {
        private readonly List<ITickableSystem> _tickableSystems = new();
        private readonly HashSet<IPausable> _pausables = new();

        public bool IsPaused { get; private set; } = true;

        public PauseSystem(IEnumerable<ITickableSystem> tickableSystems)
        {
            _tickableSystems.AddRange(tickableSystems);
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

        public void Register(ITickableSystem pausable)
        {
            _tickableSystems.Add(pausable);
        }

        public void Unregister(IPausable pausable)
        {
            _pausables.Remove(pausable);
        }

        public void Unregister(ITickableSystem pausable)
        {
            _tickableSystems.Remove(pausable);
        }
    }
}