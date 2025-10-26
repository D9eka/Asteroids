using _Project.Scripts.Camera;
using UnityEngine;

namespace _Project.Scripts.Spawning.Core
{
    public class EnemySpawnPointGenerator
    {
        private readonly ICameraBoundsUpdater _cameraBoundsUpdater;

        public EnemySpawnPointGenerator(ICameraBoundsUpdater cameraBoundsUpdater)
        {
            _cameraBoundsUpdater = cameraBoundsUpdater;
        }

        public Vector2 GetRandomPositionOutsideBounds(float distanceOutside)
        {
            Vector2 min = _cameraBoundsUpdater.MinBounds;
            Vector2 max = _cameraBoundsUpdater.MaxBounds;

            SpawnSide side = (SpawnSide)Random.Range(0, 3);
            return side switch
            {
                SpawnSide.Left => new Vector2(min.x - distanceOutside, Random.Range(min.y, max.y)),
                SpawnSide.Rigth => new Vector2(max.x + distanceOutside, Random.Range(min.y, max.y)),
                _ => new Vector2(Random.Range(min.x, max.x), max.y + distanceOutside)
            };
        }
    }
}