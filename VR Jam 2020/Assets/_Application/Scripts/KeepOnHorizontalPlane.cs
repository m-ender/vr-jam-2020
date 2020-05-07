using UnityEngine;

namespace VRJam2020
{
	[ExecuteAlways]
	public class KeepOnHorizontalPlane : MonoBehaviour
	{
		[SerializeField] private Transform reference;

		private void Update()
		{
			Vector3 pos = transform.position;
			pos.y = reference.position.y;
			transform.position = pos;
			transform.rotation = Quaternion.identity;
		}
	}
}
