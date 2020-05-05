using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

namespace VRJam2020
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextManager : MonoBehaviour
    {
        [SerializeField] float textDelay = 0;
        [SerializeField] float textLifeTime = 0;
        [SerializeField] float fadeDuration = 0;

        private TextMeshProUGUI popUpText = null;
        private float timeLeft;
        private string currentText;

        private Sequence s;

        private bool isFading;
        private void Awake()
        {
            popUpText = GetComponent<TextMeshProUGUI>();
            DOTween.Init();
            
            s = DOTween.Sequence();
            s.Append(DOTween.ToAlpha(() => popUpText.color, x => popUpText.color = x, 0.001f, fadeDuration));
            s.AppendCallback(() =>
            {
                isFading = false;
                popUpText.SetText("");
            });
        }

        private void Update()
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                currentText = null;
                if(!isFading && popUpText.color.a > 0.002f)
                    FadeText();
            }

            Debug.Log(popUpText.color.a);

        }

        private void FadeText()
        {
            isFading = true;
            s = DOTween.Sequence();
            s.Append(DOTween.ToAlpha(() => popUpText.color, x => popUpText.color = x, 0.001f, fadeDuration));
            s.AppendCallback(() =>
            {
                isFading = false;
                popUpText.SetText("");
            });
        }

        public IEnumerator TypeText(string fullText)
        {
            timeLeft = textLifeTime;

            if (fullText == currentText)
                yield break;

            currentText = fullText;


            string typingText = "";

            if (!s.IsComplete())
            {
                s.Kill();
                isFading = false;
            }

            Sequence showText = DOTween.Sequence();
            showText.Append(DOTween.ToAlpha(() => popUpText.color, x => popUpText.color = x, 1, 0));

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