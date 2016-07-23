using UnityEngine;
using System.Collections;

public class AutoOrderLayer : MonoBehaviour {
	
	SpriteRenderer sr;

	void Awake () {
		sr = GetComponent<SpriteRenderer> ();
		sr.sortingOrder = -9999;
	}

	void Update () {
		sr.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
	}
}
