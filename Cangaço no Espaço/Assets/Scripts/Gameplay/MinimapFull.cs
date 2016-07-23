using UnityEngine;
using System.Collections;

public class MinimapFull : MonoBehaviour
{
	public float minimapTime = 0.5f;
	bool minimapMaximize = false;
	tk2dCamera myCamera;
	Rect startMinimapRect;
	private float t = 0f;

	// Use this for initialization
	void Start()
	{
		myCamera = GetComponent<tk2dCamera>();
		startMinimapRect = myCamera.CameraSettings.rect;
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetButtonDown("Minimap"))
		//{
		//	if (!minimapMaximize)
		//	{
		//		minimapMaximize = true;
		//		t = 0f;
		//	}
		//}
		//else if (Input.GetButtonUp("Minimap"))
		//{
		//	Debug.Log("Minimap button up");
		//	if (minimapMaximize)
		//	{
		//		minimapMaximize = false;
		//		t = 1f;
		//	}
		//}
		if (Input.GetButton("Minimap"))
		{
			Debug.Log("Minimap button down");
			if (t <= 1f)
			{
				t += minimapTime * Time.deltaTime;
				float x = Mathf.Lerp(startMinimapRect.x, 0f, t);
				float y = Mathf.Lerp(startMinimapRect.y, 0f, t);
				float w = Mathf.Lerp(startMinimapRect.width, 1f, t);
				float h = Mathf.Lerp(startMinimapRect.height, 1f, t);

				myCamera.CameraSettings.rect = new Rect(x, y, w, h);
			}
		}
		else
		{
			Debug.Log("Minimap button up");
			if (t >= 0)
			{
				t -= minimapTime * Time.deltaTime;
				//float x = Mathf.Lerp(0f, startMinimapRect.x, t);
				//float y = Mathf.Lerp(0f, startMinimapRect.y, t);
				//float w = Mathf.Lerp(1f, startMinimapRect.width, t);
				//float h = Mathf.Lerp(1f, startMinimapRect.height, t);

				//myCamera.CameraSettings.rect = new Rect(x, y, w, h);
				float x = Mathf.Lerp(startMinimapRect.x, 0f, t);
				float y = Mathf.Lerp(startMinimapRect.y, 0f, t);
				float w = Mathf.Lerp(startMinimapRect.width, 1f, t);
				float h = Mathf.Lerp(startMinimapRect.height, 1f, t);

				myCamera.CameraSettings.rect = new Rect(x, y, w, h);
			}
		}
	}
}
