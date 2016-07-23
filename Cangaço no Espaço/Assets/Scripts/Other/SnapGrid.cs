using UnityEngine;
using System.Collections;

// This script is executed in the editor
//[ExecuteInEditMode]
public class SnapGrid : MonoBehaviour {

	//http://www.alanzucconi.com/2015/07/22/how-to-snap-to-grid-in-unity3d/

	//#if UNITY_EDITOR
	public bool snapToGrid = true;
	public float snapValue = 1f;

	public bool sizeToGrid = false;
	public float sizeValue = 0.22f;

	public float xOffset = 0.0f;
	public float yOffset = 0.0f;

	// Adjust size and position
	void Awake ()
	{
		if (snapToGrid) {
			transform.position = RoundTransform (transform.position, snapValue);
		}

		if (sizeToGrid)
			transform.localScale = RoundTransform(transform.localScale, sizeValue);
	}

	void Start(){
		transform.position += new Vector3 (xOffset, yOffset, 0f); 
	}

	// The snapping code
	private Vector3 RoundTransform (Vector3 v, float snapValue)
	{
		return new Vector3
			(
				snapValue * Mathf.Round(v.x / snapValue),
				snapValue * Mathf.Round(v.y / snapValue),
				v.z
			);
	}
	//#endif
}
