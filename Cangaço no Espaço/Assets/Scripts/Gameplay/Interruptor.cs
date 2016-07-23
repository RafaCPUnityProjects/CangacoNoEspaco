using UnityEngine;
using System.Collections;

public class Interruptor : MonoBehaviour {

	public bool active;
	public GameObject affects;

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Box")) {
			affects.SetActive (false);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Box")) {
			affects.SetActive (true);
		}
	}
}
