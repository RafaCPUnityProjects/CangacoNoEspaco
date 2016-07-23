using UnityEngine;
using System.Collections;
using System;

public class WalkImpulseTarget : MonoBehaviour
{

	bool walk = true;
	public Transform target;

	bool facingRight = true;

	Rigidbody2D rb;
	public float walkForce = 1.2f;
	Vector3 normalizedMove;

	float T_IMPULSE = 2f;
	float timeToMove = 0f;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		GameplayEvents.nimOnStage.Subscribe (OnNimOnStage);
	}

	private void OnNimOnStage(NimHealth nimHealth){
		target = nimHealth.transform;
	}


	void Update()
	{
		//Flip sprite
		if (target.position.x < transform.position.x && facingRight)
		{
			Flip();
		}
		if (target.position.x > transform.position.x && !facingRight)
		{
			Flip();
		}

		timeToMove -= Time.deltaTime;
		if (timeToMove <= 0) {
			timeToMove = T_IMPULSE;
			ImpulsedMove ();
		}
	}

	void ImpulsedMove()
	{
		if (!walk)
			return;

		rb.AddForce((transform.position - target.position).normalized * -walkForce, ForceMode2D.Impulse);
	}

	public void Stop()
	{
		rb.velocity = Vector2.zero;
		walk = false;
	}

	public void Move()
	{
		walk = true;
	}

	void Flip()
	{
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public Vector3 GetDirection()
	{
		if (facingRight)
			return Vector3.right;
		else
			return Vector3.left;
	}
}

