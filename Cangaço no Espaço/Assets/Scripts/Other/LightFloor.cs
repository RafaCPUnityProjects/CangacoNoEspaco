using UnityEngine;
using System.Collections;

public class LightFloor : MonoBehaviour {

	Animator ani;

	void Awake(){
		ani = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("PlayerFeet")) {
			ani.SetTrigger ("EnterFloor");
		}
	}
}
