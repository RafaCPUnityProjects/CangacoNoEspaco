using UnityEngine;
using System.Collections;
using Fungus;
public class LevelSelector : MonoBehaviour {

	public Flowchart flowchart;
	public string fungusMessage;
	bool nearPlayer;
	public bool isEnabled;
	public GameObject iconInteract;

	void Awake(){
		if (iconInteract != null) {
			iconInteract.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			nearPlayer = true;
			if (iconInteract != null) {
				iconInteract.SetActive (true);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			nearPlayer = false;
			if (iconInteract != null) {
				iconInteract.SetActive (false);
			}
		}
	}

	void Update(){
		if (isEnabled && nearPlayer && Input.GetKeyDown(KeyCode.E)) {
			flowchart.SendFungusMessage (fungusMessage);
		}
	}
}
