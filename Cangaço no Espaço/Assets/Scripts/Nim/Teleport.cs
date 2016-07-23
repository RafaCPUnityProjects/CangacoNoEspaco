using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Teleport : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
		}
	}
}
