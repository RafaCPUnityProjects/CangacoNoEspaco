using UnityEngine;
using System.Collections;

public class MoveRandom : MonoBehaviour {

	Rigidbody2D rb;

	public float seconds = 3f;
	public float force = 0.2f;

	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
	}

	void Start(){
		InvokeRepeating ("Move", seconds, seconds);
	}

	void Move(){
		rb.velocity = Vector2.zero;
		Vector2 randomDir = new Vector2 (Random.Range (-1f, 1f), Random.Range (-1f, 1f)).normalized;
		rb.AddForce (randomDir * force, ForceMode2D.Impulse);
	}
}
