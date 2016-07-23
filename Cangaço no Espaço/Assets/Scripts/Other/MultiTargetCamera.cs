using UnityEngine;
using System.Collections;

public class MultiTargetCamera : MonoBehaviour
{
	public Transform player;
	public Transform target;

	public Vector2 maxDistance;
	public Vector2 minDistance;

	public float maxZoom;
	public float minZoom;

	float zoomX;
	float zoomY;
	Vector3 midPoint;
	tk2dCamera myCamera;

	// Use this for initialization
	void Start()
	{
		myCamera = GetComponentInChildren<tk2dCamera>();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		midPoint.Set(player.position.x + ((target.position.x - player.position.x) / 2f), player.position.y + ((target.position.y - player.position.y) / 2f), -10f);
		transform.position = midPoint;

		zoomX = (Mathf.Abs(player.position.x - target.position.x) - minDistance.x) / (maxDistance.x - minDistance.x);
		zoomY = (Mathf.Abs(player.position.y - target.position.y) - minDistance.y) / (maxDistance.y - minDistance.y);

		if (myCamera)
		{
			myCamera.ZoomFactor = Mathf.Lerp(minZoom, maxZoom, zoomX > zoomY ? zoomX : zoomY);
		}
	}
}
