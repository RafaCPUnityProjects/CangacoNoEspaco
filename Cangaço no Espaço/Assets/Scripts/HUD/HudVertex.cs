using UnityEngine;
using System.Collections;

public class HudVertex : MonoBehaviour {

	Rigidbody2D myRigidbody;
	public float vertexSpeed = 2f;
	public float distanceToCenter = 0.1f;

	public Transform targetVertex;
	TargetJoint2D tj2d;

	void Awake(){
		myRigidbody = GetComponent<Rigidbody2D>();
		tj2d = GetComponent<TargetJoint2D> ();
	}

	void FixedUpdate(){
		//FollowTargetWitouthRotation (transform.parent, distanceToCenter, vertexSpeed);
		tj2d.target.Set(targetVertex.position.x, targetVertex.position.y);
	}

	//Helpers
	//http://answers.unity3d.com/questions/199744/how-would-i-move-a-rigidbody-towards-another-objec.html

	void FollowTargetWithRotation(Transform target, float distanceToStop, float speed)
	{
		Vector2 dest = new Vector2 (target.position.x, target.position.y);
		Vector2 origin = new Vector2 (transform.position.x, transform.position.y);

		if(Vector2.Distance(origin, dest) > distanceToStop)
		{
			transform.LookAt(target);
			myRigidbody.AddRelativeForce(Vector2.up * speed, ForceMode2D.Force);
		}
	}

	void FollowTargetWitouthRotation(Transform target, float distanceToStop, float speed)
	{
		var direction = Vector2.zero;
		Vector2 dest = new Vector2 (target.position.x, target.position.y);
		Vector2 origin = new Vector2 (transform.position.x, transform.position.y);

		if (Vector2.Distance(origin, dest) > distanceToStop)
		{
			direction = target.position - transform.position;
			myRigidbody.AddRelativeForce(direction.normalized * speed, ForceMode2D.Force);
		}
	}
}
