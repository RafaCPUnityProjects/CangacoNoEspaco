using UnityEngine;
using System.Collections;

public class PushedExplosion : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("ExplosionPush")) {
			Debug.Log ("pushed");
			transform.parent.GetComponent<NimMove>().Push ((other.transform.position - transform.position).normalized);
		}
	}
}
