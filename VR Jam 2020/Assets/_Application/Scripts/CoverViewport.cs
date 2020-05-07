using UnityEngine;

namespace VRJam2020
{
	[RequireComponent(typeof(RectTransform))]
	public class CoverViewport : MonoBehaviour
	{
		[SerializeField] private Camera targetCamera;

		private RectTransform rectTransform;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		private void Start()
		{
			float width = rectTransform.rect.width;

			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width / targetCamera.aspect);

			float distanceFromCamera = transform.localPosition.z;

			float scale = 2.0f * distanceFromCamera * Mathf.Tan(Mathf.Deg2Rad * (targetCamera.fieldOfView * 0.5f)) / width;

			transform.localScale = scale * Vector3.one;
		}
	}
}
