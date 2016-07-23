using UnityEngine;
using System.Collections;

/// <summary>
/// C# translation from http://answers.unity3d.com/questions/155907/basic-movement-walking-on-walls.html
/// Author: UA @aldonaletto 

// Prequisites: create an empty GameObject, attach to it a Rigidbody w/ UseGravity unchecked
// To empty GO also add BoxCollider and this script. Makes this the parent of the Player
// Size BoxCollider to fit around Player model.

public class BananaMoon : MonoBehaviour {

	private float moveSpeed = 6; // move speed
	//private float turnSpeed = 90; // turning speed (degrees/second)
	//private float lerpSpeed = 10; // smoothing speed
	private float gravity = 10; // gravity acceleration
	private bool isGrounded;
	private float deltaGround = 0.2f; // character is grounded up to this distance
	private float jumpSpeed = 10; // vertical jump initial speed
	private float jumpRange = 10; // range to detect target wall
	private Vector3 surfaceNormal; // current surface normal
	private Vector3 myNormal; // character normal
	private float distGround; // distance from character position to ground
	private bool jumping = false; // flag &quot;I'm jumping to wall&quot;
	//private float vertSpeed = 0; // vertical jump current speed

	private Transform myTransform;
	public BoxCollider2D boxCollider; // drag BoxCollider ref in editor

	Rigidbody2D myRigidbody;
	public LayerMask layerMask;
	int previousWall = 0;

	void Awake(){
		myRigidbody = GetComponent<Rigidbody2D> ();
	}

	private void Start(){
		myNormal = transform.up; // normal starts as character up direction
		myTransform = transform;
		//rigidbody.freezeRotation = true; // disable physics rotation
		// distance from transform.position to ground
		distGround = boxCollider.bounds.extents.y - boxCollider.offset.y;

	}

	private void FixedUpdate(){
		// apply constant weight force according to character normal:
		myRigidbody.AddForce(-gravity*myRigidbody.mass*myNormal);
	}

	//use raycast for this
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.CompareTag ("Wall") && previousWall != other.gameObject.GetInstanceID ()) {
			previousWall = other.gameObject.GetInstanceID ();
			//if collide wall turn CCW
			TurnCCW();
		}
	}

	//use raycast for this
	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.CompareTag ("Wall") && previousWall != other.gameObject.GetInstanceID ()) {
			previousWall = other.gameObject.GetInstanceID ();
			//if exit collide wall turn CW
			TurnCW();
		}
	}

	void TurnCCW(){
		surfaceNormal = -transform.right;
		myNormal = surfaceNormal;
		isGrounded = true;
		transform.Rotate (0,0,90f);
	}

	void TurnCW(){
		surfaceNormal = transform.right;
		myNormal = surfaceNormal;
		isGrounded = true;
		transform.Rotate (0,0,-90f);
	}

	private void Update(){

        // jump code - jump to wall or simple jump
        if (jumping) return; // abort Update while jumping to a wall

		//Ray ray;
		RaycastHit2D hit;

		if (Input.GetKeyDown (KeyCode.X)) {
			TurnCCW ();
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			TurnCW ();
		}

		if (Input.GetButtonDown("Jump")){ // jump pressed:
			
			hit = Physics2D.Raycast(myTransform.position, myTransform.right, jumpRange, layerMask);

			if (hit.collider != null) { // wall ahead?
				JumpToWall(hit.point, hit.normal); // yes: jump to the wall
			}
			else if (isGrounded){ // no: if grounded, jump up
				myRigidbody.velocity += jumpSpeed * new Vector2(myNormal.x, myNormal.y);
			}
		}

		// movement code - turn left/right with Horizontal axis:
		//myTransform.Rotate(0, 0, Input.GetAxis("Horizontal")*turnSpeed*Time.deltaTime);

		// update surface normal and isGrounded:
		hit = Physics2D.Raycast(myTransform.position, -myNormal, jumpRange, layerMask); // cast ray downwards
		if (hit.collider != null) {  // use it to update myNormal and isGrounded
			isGrounded = hit.distance <= distGround + deltaGround;
			surfaceNormal = hit.normal;
		}
		else {
			//isGrounded = false;
			// assume usual ground normal to avoid "falling forever"
			//surfaceNormal = Vector3.up;
		}

		//myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed*Time.deltaTime);
		myNormal = surfaceNormal;

		// find forward direction with new myNormal:
		//Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);

		// align character to the new myNormal while keeping the forward direction:
		//Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
		//myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, lerpSpeed*Time.deltaTime);

		// move the character forth/back with Vertical axis:
		myTransform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, 0);
	}

	private void JumpToWall(Vector3 point, Vector3 normal){
		// jump to wall
		jumping = true; // signal it's jumping to wall
		myRigidbody.isKinematic = true; // disable physics while jumping
		Vector3 orgPos = myTransform.position;
		Quaternion orgRot = myTransform.rotation;
		Vector3 dstPos = point + normal * (distGround + 0.5f); // will jump to 0.5 above wall
		Vector3 myForward = Vector3.Cross(myTransform.right, normal);
		Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

		StartCoroutine (jumpTime (orgPos, orgRot, dstPos, dstRot, normal));
		//jumptime
	}

	private IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal) {
		for (float t = 0.0f; t < 1.0f; ){
			t += Time.deltaTime;
			myTransform.position = Vector3.Lerp(orgPos, dstPos, t);
			myTransform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
			yield return null; // return here next frame
		}
		myNormal = normal; // update myNormal
		myRigidbody.isKinematic = false; // enable physics
		jumping = false; // jumping to wall finished

	}

}