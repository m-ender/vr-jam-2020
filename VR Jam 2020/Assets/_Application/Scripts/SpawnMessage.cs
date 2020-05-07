using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRJam2020
{
    public class SpawnMessage : MonoBehaviour
    {
        private TextManager textManager;

        private void Awake()
        {
            textManager = FindObjectOfType<TextManager>();
            textManager.TypeText(PopUpType.PlayerDialogue, "*You awaken in an empty cell*", 5f);
            StartCoroutine(writeTriggerHint());
        }

        private IEnumerator writeTriggerHint()
        {
            yield return new WaitForSeconds(25);
            if(textManager.isFree)
                textManager.ShowText(PopUpType.PlayerDialogue, "That ball seems to respond me pressing the TRIGGER", 10f);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

