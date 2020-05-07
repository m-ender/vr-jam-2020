using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace VRJam2020
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextManager : MonoBehaviour
    {
        [SerializeField] private float textDelay = 0;
        [SerializeField] private float fadeDuration = 0;

        public bool isFree { get; private set; }

        private TextMeshProUGUI popUpTextObject = null;
        private float lifeTimeLeft;
        private string currentFullText;

        private Sequence s;

        private bool isFading;
        private void Awake()
        {
            popUpTextObject = GetComponent<TextMeshProUGUI>();
            DOTween.Init();

            s = DOTween.Sequence();
        }

        private void Update()
        {
            lifeTimeLeft -= Time.deltaTime;

            if (lifeTimeLeft < 0)
            {
                currentFullText = null;
                if (!isFading && popUpTextObject.color.a > 0.002f)
                    FadeText();
            }
        }

        private void FadeText()
        {
            //Can't tween to 0 decimal places, so tween to near 0.
            isFading = true;
            s = DOTween.Sequence();
            s.Append(DOTween.ToAlpha(() => popUpTextObject.color, x => popUpTextObject.color = x, 0.001f, fadeDuration));
            s.AppendCallback(() =>
            {
                isFading = false;
                popUpTextObject.SetText("");
                isFree = true;
            });
        }

        public void TypeText(PopUpType type, string fullText, float displayTime)
        {
            StartCoroutine(typeText(type, fullText, displayTime));
        }

        public void ShowText(PopUpType type, string fullText, float displayTime)
        {
            EndCurrentFades();

            isFree = false;
            
            Sequence showText = DOTween.Sequence();
            showText.Append(DOTween.ToAlpha(() => popUpTextObject.color, x => popUpTextObject.color = x, 1, 0));

            currentFullText = fullText;

            ChangeFontOnType(type);

            popUpTextObject.SetText(fullText);

            lifeTimeLeft = displayTime;
        }


        private IEnumerator typeText(PopUpType type,string fullText, float displayTime)
        {
            isFree = false;

            lifeTimeLeft = displayTime;

            if (fullText == currentFullText)
                yield break;

            ChangeFontOnType(type);

            currentFullText = fullText;

            string typingText = "";

            EndCurrentFades();

            Sequence showText = DOTween.Sequence();
            showText.Append(DOTween.ToAlpha(() => popUpTextObject.color, x => popUpTextObject.color = x, 1, 0));

            foreach (char letter in fullText.ToCharArray())
            {
                if (currentFullText != fullText)
                    yield break;
                typingText += letter;
                popUpTextObject.SetText(typingText);
                yield return new WaitForSeconds(textDelay);
                lifeTimeLeft = displayTime;
            }
        }

        private void ChangeFontOnType(PopUpType type)
        {
            if (type == PopUpType.Alert)
            {
                popUpTextObject.fontStyle = FontStyles.Bold;
                popUpTextObject.faceColor = Color.magenta;
            }

            if (type == PopUpType.Enemy)
            {
                popUpTextObject.fontStyle = FontStyles.Normal;
                popUpTextObject.color = Color.red;
            }

            if (type == PopUpType.PlayerDialogue)
            {
                popUpTextObject.fontStyle = FontStyles.Normal;
                popUpTextObject.color = Color.white;
            }
        }

        private void EndCurrentFades()
        {
            if (!s.IsComplete())
            {
                s.Kill();
                isFading = false;
            }
        }
    }
}