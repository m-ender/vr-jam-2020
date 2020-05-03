using System;
using UnityEngine;
using Valve.VR;

namespace VRJam2020
{
	[RequireComponent(typeof(Rigidbody))]
	public class FlyToHand : MonoBehaviour
	{
		[SerializeField] private Transform leftHand;
		[SerializeField] private Transform rightHand;
		[SerializeField] private float flyingSpeed;

		private SteamVR_Input_Sources leftHandSource;
		private SteamVR_Input_Sources rightHandSource;
		private Rigidbody rigidBody;

		private bool isFlying;
		private Transform targetHand;

		private void Awake()
		{
			leftHandSource = SteamVR_Input_Sources.LeftHand;
			rightHandSource = SteamVR_Input_Sources.RightHand;

			rigidBody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (SteamVR_Actions.default_SummonBall.GetStateDown(leftHandSource))
			{
				StartFlying(leftHand);
			}
			else if (SteamVR_Actions.default_SummonBall.GetStateDown(rightHandSource))
			{
				StartFlying(rightHand);
			}

			if (isFlying && (
				targetHand == leftHand && SteamVR_Actions.default_SummonBall.GetStateUp(leftHandSource)
				|| targetHand == rightHand && SteamVR_Actions.default_SummonBall.GetStateUp(rightHandSource)))
			{
				StopFlying();
			}

			if (isFlying)
				FlyTowardsHand();
		}

		private void StartFlying(Transform hand)
		{
			isFlying = true;
			targetHand = hand;
		}

		private void StopFlying()
		{
			isFlying = false;
		}

		private void FlyTowardsHand()
		{
			rigidBody.velocity = (targetHand.position - transform.position).normalized * flyingSpeed;
		}
	}
}
