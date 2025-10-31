using UnityEngine;

namespace Asteroids.Scripts.Core
{
    public interface ITransformProvider
    {
        Transform Transform { get; }
    }
}