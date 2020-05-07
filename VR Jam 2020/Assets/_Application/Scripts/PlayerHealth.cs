using DG.Tweening;
using UnityEngine;
using Valve.VR;

namespace VRJam2020
{
    public class PlayerHealth : Health
    {
        [SerializeField] private Color damageHighlightColor;
        [SerializeField] private float damageHighlightTime;

        protected override void Die()
        {
            Debug.Log("u ded. sad.");
        }

        protected override void ShowDamageFeedback()
        {
            SteamVR_Fade.Start(damageHighlightColor, damageHighlightTime / 2);

            DOTween.Sequence()
                .InsertCallback(damageHighlightTime / 2, () => SteamVR_Fade.Start(Color.clear, damageHighlightTime / 2));
        }
    }
}