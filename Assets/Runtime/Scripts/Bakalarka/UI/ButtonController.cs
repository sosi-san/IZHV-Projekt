using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Woska.Bakalarka.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TextMeshProUGUI _textfield;
        private void Awake()
        {
            _textfield = GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _textfield.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _textfield.fontStyle = FontStyles.Normal | FontStyles.UpperCase;
        }
    }
}