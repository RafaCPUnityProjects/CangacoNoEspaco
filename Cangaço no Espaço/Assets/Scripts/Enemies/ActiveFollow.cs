using UnityEngine;
using System.Collections;

public class ActiveFollow : MonoBehaviour {

	FollowTarget ft;

	void Start(){
		ft = transform.parent.GetComponent<FollowTarget> ();
		ft.enabled = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag("Player")) {
			ft.enabled = true;
			ft.Move ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag("Player")) {
			ft.enabled = false;
			ft.Stop ();
		}
	}
}
