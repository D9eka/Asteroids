using System;
using System.Text;
using _Project.Scripts.Player;
using _Project.Scripts.Weapons.Types.Laser;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI
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