using UnityEngine;
using System.Collections;

public class BananaMove : MonoBehaviour {

	Rigidbody2D rb;
	float speed = 20f;
	//float distanceToWall = 0.1f;
	//float radius = 0.12f;

	public enum Direction {Up, Down, Left, Right}
	Direction currentDirection;

	bool checkCollision = true;

	public Animator spriteAnimator;

	public LayerMask layerMask;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		MoveForward ();
		currentDirection = Direction.Up;
	}
	
	void FixedUpdate () {

		if (!checkCollision)
			return;

		CancelInvoke ("ReturnToCheck");

		if (currentDirection.Equals (Direction.Up)) {
			
			if (!CheckCollide (Vector2.right)) {
				GoRight ();
				return;
			}

			if (CheckCollide (Vector2.up, true)) {
				GoLeft ();
				return;
			}
		}

		else if (currentDirection.Equals (Direction.Right)) {

			if (!CheckCollide (Vector2.down)) {
				GoDown ();
				return;
			}

			if (CheckCollide (Vector2.right, true)) {
				GoUp ();
				return;
			}
		}

		else if (currentDirection.Equals (Direction.Left)) {

			if (!CheckCollide (Vector2.up)) {
				GoUp ();
				return;
			}

			if (CheckCollide (Vector2.left, true)) {
				GoDown ();
				return;
			}
		}

		else if (currentDirection.Equals (Direction.Down)) {

			if (!CheckCollide (Vector2.left)) {
				GoLeft ();
				return;
			}

			if (CheckCollide (Vector2.down, true)) {
				GoRight ();
				return;
			}
		}

	}

	bool CheckCollide (Vector2 direction, bool frontCollision = false) {

		Vector2 backPosition = new Vector2(transform.position.x, transform.position.y);

		/*if (!frontCollision) {
			if (currentDirection.Equals (Direction.Up)) {
				backPosition = new Vector3 (transform.position.x, transform.position.y - radius);
			} else if (currentDirection.Equals (Direction.Right)) {
				backPosition = new Vector3 (transform.position.x - radius, transform.position.y);
			} else if (currentDirection.Equals (Direction.Left)) {
				backPosition = new Vector3 (transform.position.x + radius, transform.position.y);
			} else if (currentDirection.Equals (Direction.Down)) {
				backPosition = new Vector3 (transform.position.x, transform.position.y + radius);
			}
		}*/

		RaycastHit2D hit = Physics2D.Raycast(backPosition, direction, 0.3f, layerMask);

		if (hit.collider != null) {

			//float distanceX = Mathf.Abs(hit.point.x - backPosition.x);
			//float distanceY = Mathf.Abs(hit.point.y - backPosition.y);

			//if(new Vector2(distanceX, distanceY).magnitude < distanceToWall)
			return true;
		}
		return false;
	}

	void GoRight () {
		if (spriteAnimator != null)
			spriteAnimator.SetTrigger ("goLeftRight");

		checkCollision = false;
		Invoke ("ReturnToCheck", 3f);
		Debug.Log ("Right");
		currentDirection = Direction.Right;
		rb.velocity = Vector2.zero;
		rb.AddForce (Vector2.right * speed);
	}

	void GoLeft () {
		if (spriteAnimator != null)
			spriteAnimator.SetTrigger ("goLeftRight");
		
		checkCollision = false;
		Invoke ("ReturnToCheck", 3f);
		Debug.Log ("Left");
		currentDirection = Direction.Left;
		rb.velocity = Vector2.zero;
		rb.AddForce (Vector2.left * speed);
	}

	void GoUp () {
		if (spriteAnimator != null)
			spriteAnimator.SetTrigger ("goUp");
		
		checkCollision = false;
		Invoke ("ReturnToCheck", 3f);
		Debug.Log ("Up");
		currentDirection = Direction.Up;
		rb.velocity = Vector2.zero;
		rb.AddForce (Vector2.up * speed);
	}

	void GoDown () {
		
		if (spriteAnimator != null)
			spriteAnimator.SetTrigger ("goDown");
		
		checkCollision = false;
		Invoke ("ReturnToCheck", 3f);
		Debug.Log ("Down");
		currentDirection = Direction.Down;
		rb.velocity = Vector2.zero;
		rb.AddForce (Vector2.down * speed);
	}

	void ReturnToCheck(){
		checkCollision = true;
	}

	void MoveForward () {
		Debug.Log ("Forward");
		rb.AddForce (Vector2.up * speed);
	}
}
