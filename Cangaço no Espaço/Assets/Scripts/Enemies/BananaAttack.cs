using UnityEngine;
using System.Collections;

public class BananaAttack : MonoBehaviour {

	public GameObject missile;
	Animator anim;
	Patrol patrol;

	void Start(){
		
		anim = transform.FindChild ("Sprite").GetComponent<Animator> ();
		patrol = GetComponent<Patrol> ();

		if(missile != null)
			InvokeRepeating ("AirAttack", 3f, 10f);
	}

	void AirAttack(){
		
		if(patrol != null)
			patrol.stop = true;
		
		anim.SetTrigger ("triggerShoot");

		Invoke ("ShootMissile",1f);
		Invoke ("ShootMissile",2f);
		Invoke ("ShootMissile",3f);
	}

	void ShootMissile(){
		
		if(patrol != null)
			patrol.stop = false;
		
		Instantiate (missile, NimMove.instance.transform.position, Quaternion.identity);
	}
}
