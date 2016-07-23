using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	public GameObject player;
	EnemyHealth health;
	//WeaponController wc;

	void Start () {
		player = GameObject.FindGameObjectWithTag("PlayerFeet");
		health = GetComponent<EnemyHealth> ();
		InvokeRepeating("CheckProtection", 1f, 0.2f);

		//wc = GameObject.FindObjectOfType<WeaponController> ();
	}

	void CheckProtection () {
		if (GameSave.nimWeapon.Equals (CurrentWeapon.None)) {
			//Case of "invincible rock" only with Melee
			health.invincible = (bool)(Vector2.Distance (player.transform.position, transform.position) > 0.3f);
		} else {
			health.invincible = false;
		}
	}
}
