using UnityEngine;
using Valve.VR;

namespace VRJam2020
{
	public class FlyToHand : MonoBehaviour
	{
		[SerializeField] private Transform leftHand;
		[SerializeField] private Transform rightHand;

		private SteamVR_Input_Sources leftHandSource;
		private SteamVR_Input_Sources rightHandSource;

		private void Awake()
		{
			leftHandSource = SteamVR_Input_Sources.LeftHand;
			rightHandSource = SteamVR_Input_Sources.RightHand;
		}

	}
}
