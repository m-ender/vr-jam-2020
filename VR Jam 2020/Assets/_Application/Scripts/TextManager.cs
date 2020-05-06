using System.Collections;
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
        [SerializeField] float fadeDuration = 0;

        private TextMeshProUGUI popUpText = null;
        private float lifeTimeLeft;
        private string currentFullText;

        private Sequence s;

        private bool isFading;
        private void Awake()
        {
            popUpText = GetComponent<TextMeshProUGUI>();
            DOTween.Init();
            
            s = DOTween.Sequence();
        }

        private void Update()
        {
            lifeTimeLeft -= Time.deltaTime;

            if (lifeTimeLeft < 0)
            {
                currentFullText = null;
                if(!isFading && popUpText.color.a > 0.002f)
                    FadeText();
            }
        }

        private void FadeText()
        {
            //Can't tween to 0 decimal places, so tween to near 0.
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
            lifeTimeLeft = textLifeTime;

            if (fullText == currentFullText)
                yield break;

            currentFullText = fullText;

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
                if (currentFullText != fullText)
                    yield break;
                typingText += letter;
                popUpText.SetText(typingText);
                yield return new WaitForSeconds(textDelay);
                lifeTimeLeft = textLifeTime;
            }
        }
    }
}