using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Camera
{
    public class CameraBoundsUpdater : ICameraBoundsUpdater, ITickable
    {
        private readonly UnityEngine.Camera _camera;
        private Vector3 _lastCameraPosition;
        private float _lastOrthographicSize;
        private float _lastAspect;

        public Vector2 MinBounds { get; private set; }
        public Vector2 MaxBounds { get; private set; }

        [Inject]
        public CameraBoundsUpdater(UnityEngine.Camera camera)
        {
            _camera = camera;
            UpdateBounds();
            CacheCameraParameters();
        }

        public void Tick()
        {
            if (IsCameraParamsChanged())
            {
                UpdateBounds();
                CacheCameraParameters();
            }
        }

        private bool IsCameraParamsChanged()
        {
            return _camera.transform.position != _lastCameraPosition ||
                   _camera.orthographicSize != _lastOrthographicSize ||
                   _camera.aspect != _lastAspect;
        }

        private void UpdateBounds()
        {
            float camHeight = _camera.orthographicSize;
            float camWidth = camHeight * _camera.aspect;
            Vector3 camPos = _camera.transform.position;

            MinBounds = new Vector2(camPos.x - camWidth, camPos.y - camHeight);
            MaxBounds = new Vector2(camPos.x + camWidth, camPos.y + camHeight);
        }

        private void CacheCameraParameters()
        {
            _lastCameraPosition = _camera.transform.position;
            _lastOrthographicSize = _camera.orthographicSize;
            _lastAspect = _camera.aspect;
        }
    }
}