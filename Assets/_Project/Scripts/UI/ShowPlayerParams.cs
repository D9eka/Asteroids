using System;
using System.Text;
using Asteroids.Scripts.Player;
using Asteroids.Scripts.Weapons.Types.Laser;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids.Scripts.UI
{
    public class ShowPlayerParams : MonoBehaviour, IShowPlayerParams
    {
        [SerializeField] private TMP_Text _text;
        
        public void Show(string parameters)
        {
            _text.text = parameters;
        }
    }
}