using UnityEngine;

namespace _Project.Scripts.Core
{
    public interface ITransformProvider
    {
        Transform Transform { get; }
    }
}