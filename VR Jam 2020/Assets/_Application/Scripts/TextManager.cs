using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VRJam2020
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextManager : MonoBehaviour
    {
        private TextMeshProUGUI popUpText = null;
        private float timeLeft;
        private void Awake()
        {
            popUpText = GetComponent<TextMeshProUGUI>();
        }
        public void SetPopUpText(string text)
        {
            popUpText.SetText(text);
            timeLeft = 15;
        }

        private void Update()
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
                popUpText.SetText("");
        }


    }
}