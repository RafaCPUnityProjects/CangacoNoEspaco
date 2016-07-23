using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	//http://forum.unity3d.com/threads/screen-shake-effect.22886/

	public float distanceShakeX = 0.05f;
	public float distanceShakeY = 0.05f;

	public float shakeYSpeed = 0.6f;
	public float shakeXSpeed = 0.6f;

	private float shakeX;
	private float shakeY;

	bool activeTrigger = false;

	void Awake(){
		shakeX = distanceShakeX;
		shakeY = distanceShakeY;
	}

	public void TriggerShake(){

		activeTrigger = true;

		shakeX = Random.Range(distanceShakeX/2, distanceShakeX);
		shakeY = Random.Range(distanceShakeY/2, distanceShakeY);

		if (Random.value >= 0.5) {
			shakeX = -shakeX;
		}
		if (Random.value >= 0.5) {
			shakeY = -shakeY;
		}
	}

	void Update(){

		if (Mathf.Abs(shakeY) > 0.01f && activeTrigger) {

			Vector2 _newPosition = new Vector2 (shakeX, shakeY);

			if (shakeY < 0) {
				shakeY *= shakeYSpeed;
			}
			shakeY = -shakeY;

			if (shakeX < 0) {
				shakeX *= shakeXSpeed;
			}
			shakeX = -shakeX;

			transform.Translate (_newPosition, Space.Self);
		} else {
			transform.localPosition = Vector3.zero;
		}
	}
}
