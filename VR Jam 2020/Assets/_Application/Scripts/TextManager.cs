using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace VRJam2020
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextManager : MonoBehaviour
    {
        [SerializeField] float textDelay = 0;
        [SerializeField] float textLifeTime = 0;

        private TextMeshProUGUI popUpText = null;
        private float timeLeft;
        private string currentText;
        private void Awake()
        {
            popUpText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                currentText = null;
                popUpText.SetText("");
            }
        }

        public IEnumerator TypeText(string fullText)
        {
            timeLeft = textLifeTime;

            if (fullText == currentText)
                yield break;

            currentText = fullText;

            string typingText = "";

            foreach (char letter in fullText.ToCharArray())
            {
                if (currentText != fullText)
                    yield break;
                typingText += letter;
                popUpText.SetText(typingText);
                yield return new WaitForSeconds(textDelay);
                timeLeft = textLifeTime;
            }
        }
    }
}