using UnityEngine;

namespace VRJam2020
{
	public class EnemyFieldOfView : MonoBehaviour
	{
		[SerializeField] private Enemy enemy;

		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<TrackablePlayer>())
			{
				enemy.DiscoverPlayer(other.transform);
				Destroy(gameObject);
			}
		}
	}
}
