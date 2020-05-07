using System.Collections;
using UnityEngine;

namespace VRJam2020
{
    public class SpawnMessage : MonoBehaviour
    {
        [SerializeField] GameObject teleportTextTrigger = null;
        private TextManager textManager;


        private void Awake()
        {
            textManager = FindObjectOfType<TextManager>();
            textManager.TypeText(PopUpType.PlayerDialogue, "*You awaken in an empty cell*", 5f);
            StartCoroutine(writeTriggerHint());
        }

        private IEnumerator writeTriggerHint()
        {
            yield return new WaitForSeconds(20);
            if(teleportTextTrigger != null)
                textManager.ShowText(PopUpType.PlayerDialogue, "Hmm... Maybe something happens if I hold the TRIGGER down. ", 10f);
        }
    }
}

