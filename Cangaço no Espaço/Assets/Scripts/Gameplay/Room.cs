using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	//Active child objects if Player is inside

	void Awake(){
		SetActiveChild (false);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			SetActiveChild (true);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			SetActiveChild (false);
		}
	}

	void SetActiveChild(bool active){
		foreach(Transform child in transform){
			child.gameObject.SetActive (active);
		}
	}
}
