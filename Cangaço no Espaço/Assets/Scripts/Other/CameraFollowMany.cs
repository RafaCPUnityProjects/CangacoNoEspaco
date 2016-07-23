using UnityEngine;
using System.Collections;

public class CameraFollowMany : MonoBehaviour
{
	public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
	public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
	public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
													//[HideInInspector]
	public Transform[] m_Targets; // All the targets the camera needs to encompass.

	public float zoomMin;
	public Vector2 nimMaxPos;
	public Vector2 bossMaxPos;

	public float zoomMax;
	public Vector2 nimMinPos;
	public Vector2 bossMinPos;

	private float ax;// = -0.1754f;
	private float bx;// = 3.0175f;
	private float ay;
	private float by;

	//public float lowerZoomLimit = 1f;
	//public float upperZoomLimit = 2f;

	private tk2dCamera m_Camera;                        // Used for referencing the camera.
	private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
	private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
	private Vector3 m_DesiredPosition;              // The position the camera is moving towards.


	private void Awake()
	{
		m_Camera = GetComponentInChildren<tk2dCamera>();
		CalculateParameters();
	}

	private void CalculateParameters()
	{
		float dxMin = Mathf.Abs(nimMinPos.x - bossMinPos.x);
		float dxMax = Mathf.Abs(nimMaxPos.x - bossMaxPos.x);
		ax = (zoomMax - zoomMin) / (dxMin - dxMax);
		bx = zoomMin - dxMax * ax;

		float dyMin = Mathf.Abs(nimMinPos.y - bossMinPos.y);
		float dyMax = Mathf.Abs(nimMaxPos.y - bossMaxPos.y);
		ay = (zoomMax - zoomMin) / (dyMin - dyMax);
		by = zoomMin - dyMax * ay;
	}

	private void FixedUpdate()
	{
		// Move the camera towards a desired position.
		Move();

		// Change the size of the camera based.
		Zoom();
	}


	private void Move()
	{
		// Find the average position of the targets.
		FindAveragePosition();

		// Smoothly transition to that position.
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
	}


	private void FindAveragePosition()
	{
		Vector3 averagePos = new Vector3();
		int numTargets = 0;

		// Go through all the targets and add their positions together.
		for (int i = 0; i < m_Targets.Length; i++)
		{
			// If the target isn't active, go on to the next one.
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			// Add to the average and increment the number of targets in the average.
			averagePos += m_Targets[i].position;
			numTargets++;
		}

		// If there are targets divide the sum of the positions by the number of them to find the average.
		if (numTargets > 0)
			averagePos /= numTargets;

		// Keep the same y value.
		//averagePos.y = transform.position.y;
		averagePos.z = transform.position.z;

		// The desired position is the average position;
		m_DesiredPosition = averagePos;
	}


	private void Zoom()
	{
		float a, b, distance;
		Vector3 target0 = m_Targets[0].position;
		Vector3 target1 = m_Targets[1].position;
		float dx = Mathf.Abs(target0.x - target1.x);
		float dy = Mathf.Abs(target0.y - target1.y);
		if (dx > dy)
		{
			a = ax;
			b = bx;
			distance = dx;
		}
		else
		{
			a = ay;
			b = by;
			distance = dy;
		}

		float zoomFactor = Mathf.Clamp(a * distance + b, zoomMin, zoomMax);
		zoomFactor = Mathf.SmoothDamp(m_Camera.ZoomFactor, zoomFactor, ref m_ZoomSpeed, m_DampTime);
		m_Camera.ZoomFactor = zoomFactor;
	}


	private float FindRequiredSize()
	{
		// Find the position the camera rig is moving towards in its local space.
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

		// Start the camera's size calculation at zero.
		float size = 0f;

		// Go through all the targets...
		for (int i = 0; i < m_Targets.Length; i++)
		{
			// ... and if they aren't active continue on to the next target.
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			// Otherwise, find the position of the target in the camera's local space.
			Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

			// Find the position of the target from the desired position of the camera's local space.
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			// Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

			// Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / (m_Camera.nativeResolutionWidth / m_Camera.nativeResolutionHeight));
		}

		// Add the edge buffer to the size.
		size += m_ScreenEdgeBuffer;

		// Make sure the camera's size isn't below the minimum.
		size = Mathf.Max(size, m_MinSize);

		return size;
	}


	public void SetStartPositionAndSize()
	{
		// Find the desired position.
		FindAveragePosition();

		// Set the camera's position to the desired position without damping.
		transform.position = m_DesiredPosition;

		// Find and set the required size of the camera.
		m_Camera.ZoomFactor = FindRequiredSize();
	}
}

