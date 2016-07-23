using UnityEngine;
using System.Collections;

public class ShowExclamations : MonoBehaviour {

	RectTransform rt;
	public float stepWidth = 13.3f;
	public float delay = 0.05f;
	public float startDelay = 0f;

	void Start () {
		stepWidth = stepWidth / 50;
		startDelay = startDelay / 4;

		rt = GetComponent<RectTransform> ();
		InvokeRepeating ("MoveRight", startDelay, delay);
	}

	void MoveRight(){
		rt.transform.Translate (stepWidth, 0f, 0f);
	}

	void OnBecameInvisible() {
		CancelInvoke ();
		enabled = false;
	}
}
