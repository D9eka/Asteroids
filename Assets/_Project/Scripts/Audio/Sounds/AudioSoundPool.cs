using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.Audio.Sounds
{
    public class AudioSoundPool : MemoryPool<Vector3, AudioClip, AudioSound>
    {
        protected override void Reinitialize(Vector3 position, AudioClip audioClip, AudioSound item)
        {
            if (item == null || audioClip == null)
            {
                Debug.LogError("Invalid parameters in AudioSourcePool.Reinitialize");
                return;
            }

            item.transform.position = position;
            item.Initialize(this, audioClip);
        }

        protected override void OnDespawned(AudioSound item)
        {
            base.OnDespawned(item);
            item?.OnDespawned();
        }

        protected override void OnSpawned(AudioSound item)
        {
            base.OnSpawned(item);
            item?.OnSpawned();
        }
    }
}