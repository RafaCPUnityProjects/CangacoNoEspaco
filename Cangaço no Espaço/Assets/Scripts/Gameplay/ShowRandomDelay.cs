using UnityEngine;
using System.Collections;

public class ShowRandomDelay : MonoBehaviour {

	SpriteRenderer img;
	Animator ani;
	public float seconds = 1f;

	void Start () {
		img = GetComponent<SpriteRenderer> ();
		ani = GetComponent<Animator> ();
		img.enabled = false;
		ani.enabled = false;
		Invoke ("Appear", seconds);
	}

	void Appear () {
		img.enabled = true;
		ani.enabled = true;
	}
}
