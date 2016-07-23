using UnityEngine;
using System.Collections;
using System;

public class DrawWireColliders2D : MonoBehaviour {

	//http://forum.unity3d.com/threads/is-there-really-no-way-to-view-all-colliders-in-the-editor.232771/

	void OnDrawGizmos() {

		BoxCollider2D bCol = GetComponent<BoxCollider2D>();
		CircleCollider2D sCol = GetComponent<CircleCollider2D>();

		Gizmos.color = Color.red;

		if(bCol != null)
		{
			Gizmos.DrawWireCube(bCol.bounds.center, bCol.bounds.size);
		}

		if(sCol != null)
		{
			float maxScale = Math.Max(gameObject.transform.localScale.x, 
				Math.Max(gameObject.transform.localScale.y, gameObject.transform.localScale.z));

			Gizmos.DrawWireSphere(sCol.bounds.center, sCol.radius * maxScale);
		}

	}

}