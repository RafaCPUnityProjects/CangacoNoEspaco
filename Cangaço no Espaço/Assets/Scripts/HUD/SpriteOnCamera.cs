using UnityEngine;
using System.Collections;

public class SpriteOnCamera : MonoBehaviour {

	//Enable and disable components when enemy is visible

	public EnemyHealth health;
	public EnemyAttack attack;

	public GameObject gun;
	public GameObject weakPoint;
	public GameObject shadow;

	public GameObject closeExplosion;

	Animator mainAnimator;


	void Awake(){
		mainAnimator = GetComponent<Animator> ();
	}

	void Start(){
		if(health != null)
			health.enabled = false;

		if(attack != null)
			attack.enabled = false;

		if (gun != null)
			gun.SetActive (false);

		if (weakPoint != null)
			weakPoint.SetActive (false);

		if (shadow != null)
			shadow.SetActive (false);

		if (closeExplosion != null)
			closeExplosion.SetActive (false);

		if (mainAnimator != null)
			mainAnimator.enabled = false;
	}

	void OnBecameVisible() {

		//Debug.Log ("Visible:"+transform.parent.gameObject.name);

		if(health != null)
			health.enabled = true;

		if(attack != null)
			attack.enabled = true;

		if (gun != null)
			gun.SetActive (true);

		if (weakPoint != null)
			weakPoint.SetActive (true);

		if (shadow != null)
			shadow.SetActive (true);

		if (closeExplosion != null)
			closeExplosion.SetActive (true);

		if (mainAnimator != null)
			mainAnimator.enabled = true;
	}

    //Caution: works when SpriteRenderer set to disabled inside animation
	void OnBecameInvisible() {
		if(health != null)
			health.enabled = false;

		if(attack != null)
			attack.enabled = false;

		if (gun != null)
			gun.SetActive (false);

		if (weakPoint != null)
			weakPoint.SetActive (false);

		if (shadow != null)
			shadow.SetActive (false);

		if (closeExplosion != null)
			closeExplosion.SetActive (false);

		if (mainAnimator != null)
			mainAnimator.enabled = false;
	}
}
