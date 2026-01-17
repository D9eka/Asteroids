using System;
using System.Collections.Generic;
using Asteroids.Scripts.Weapons.Core;
using Asteroids.Scripts.Weapons.Types.BulletGun;
using Asteroids.Scripts.Weapons.Types.Laser;
using UnityEngine;

namespace Asteroids.Scripts.Audio.Sounds.Weapon
{
    public class WeaponShotAudioSpawner : IDisposable
    {
        private readonly AudioSoundFactory _audioSoundFactory;
        private readonly List<IWeapon> _weapons = new List<IWeapon>();
        private readonly WeaponAudioData _data;

        public WeaponShotAudioSpawner(AudioSoundFactory audioSoundFactory, WeaponAudioData data)
        {
            _audioSoundFactory = audioSoundFactory;
            _data = data;
        }
        
        public void AddWeapon(IWeapon weapon)
        {
            _weapons.Add(weapon);
            weapon.OnShoot += PlayShootSound;
        }

        public void Dispose()
        {
            foreach (IWeapon weapon in _weapons)
            {
                weapon.OnShoot -= PlayShootSound;
            }
        }
        
        private void PlayShootSound(IWeapon weapon)
        {
            AudioClip[] weaponClips = GetWeaponClips(weapon);
            int shoutSoundIndex = UnityEngine.Random.Range(0, weaponClips.Length);
            _audioSoundFactory.Create(weapon.Transform.position, weaponClips[shoutSoundIndex]);
        }
        
        private AudioClip[] GetWeaponClips(IWeapon weapon)
        {
            return weapon switch
            {
                BulletGun => _data.BulletGunShoutSounds,
                LaserGun => _data.LaserGunShoutSounds,
                _ => throw new NotImplementedException()
            };
        }
    }
}