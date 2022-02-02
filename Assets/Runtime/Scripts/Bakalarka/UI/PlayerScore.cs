using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Woska.Core;

namespace Woska.Bakalarka.UI
{
    public class PlayerScore : MonoBehaviour
    {
        [SerializeField] private Color _emptyColor;
        private Image _background;
        private TextMeshProUGUI _playerName;
        private TextMeshProUGUI _scoreText;

        private void Awake()
        {
            _background = GetComponent<Image>();
            
            _playerName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            
            _scoreText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Setup(string nick, Color color)
        {
            Debug.Log(nick);
            _playerName.text = nick;
            _scoreText.text = "0";
            
            _background.color = color;
        }

        public void SlotIsEmpty()
        {
            _playerName.text = "Empty";
            _scoreText.text = "0";
            _background.color = _emptyColor;
        }
        public void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}