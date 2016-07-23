using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	NimHealth health;
	public GameObject[] shieldFx;

	int prevLife;

	void Start () {
		health = transform.parent.GetComponent<NimHealth> ();

		prevLife = health.lives;

		DisableShield ();
	}
	
	void Update () {

		if (prevLife == health.lives)
			return;

		if (prevLife < health.lives) {
			prevLife = health.lives;
			return;
		}

		DisableShield ();

		if(health.lives > 0 && health.lives <= shieldFx.Length)
			shieldFx [health.lives - 1].SetActive (true);	

		prevLife = health.lives;
	}

	void DisableShield(){
		for(int i=0; i < shieldFx.Length; i++){
			shieldFx [i].SetActive (false);	
		}
	}

}
