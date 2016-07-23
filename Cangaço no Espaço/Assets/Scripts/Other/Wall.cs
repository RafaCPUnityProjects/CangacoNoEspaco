using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){

		if (other.CompareTag ("Bullet")){
			Destroy (other.transform.parent.gameObject);
		}
	}
}
