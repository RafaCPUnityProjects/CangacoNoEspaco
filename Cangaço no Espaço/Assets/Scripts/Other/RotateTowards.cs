using UnityEngine;
using System.Collections;

public class RotateTowards : MonoBehaviour {

	Rigidbody2D rb;
	float angle;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		Vector2 v = rb.velocity;
		angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
