using System;
using UnityEngine;

namespace Asteroids.Scripts.Audio
{
    [CreateAssetMenu(menuName = "Asteroids/BackgroundMusicData", fileName = "BackgroundMusicData")]
    public class BackgroundMusicData : ScriptableObject
    {
        [field:SerializeField] public AudioClip[] BackgroundMusic { get; private set; }
    }
}