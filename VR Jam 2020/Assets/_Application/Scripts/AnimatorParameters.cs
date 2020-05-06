using UnityEngine;

namespace VRJam2020
{
	public static class AnimatorParameters
	{
		public static int IsMoving => Animator.StringToHash("Is Moving");
		public static int IsAttacking => Animator.StringToHash("Is Attacking");
	}
}
