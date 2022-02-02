using System.Collections;
using TMPro;
using UnityEngine;

namespace Woska.Core
{
    public class AnimatedEllipsis : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textMeshPro;
        [SerializeField] int numberOfDots = 3;
        [SerializeField] private float animationSpeed = 1f;
    
        private string initialText;
        private int baseTextLength;
        private int currentNumberOfDots = 0;
    
        private bool isCompleted;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            initialText = _textMeshPro.text;
            baseTextLength = initialText.Length;
        }

        private void OnEnable()
        {
            StartCoroutine(AnimateText());
        }

        private void OnDisable()
        {
            this.StopAllCoroutines();
        }

        IEnumerator AnimateText()
        {

            while (true)
            {
                if (currentNumberOfDots < numberOfDots - 1)
                {
                    _textMeshPro.text += ".";
                    currentNumberOfDots++;
                }
                else
                {
                    _textMeshPro.text = initialText;
                    currentNumberOfDots = 0;
                }
                yield return new WaitForSeconds(animationSpeed);
            }
        }
    }
}
