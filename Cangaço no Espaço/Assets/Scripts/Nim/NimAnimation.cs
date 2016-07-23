using UnityEngine;
using System.Collections;

public class NimAnimation : MonoBehaviour {

	//bool meleeAttack = false;
	
	public Animator nimAnimator;
	public Animator shadowAnimator;

	NimHealth health;
	bool diedAnimation = false;

	Vector2 input;
	Vector2 prevInput = Vector2.zero;

	void Awake(){
		health = GetComponent<NimHealth> ();
	}
	
	void Update () {

		if (GameController.instance.IsGamePaused())
			return;

		if (nimAnimator == null)
			return;

		//die animation
		if (health.lives <= 0 && !diedAnimation) {
			diedAnimation = true;
			nimAnimator.SetTrigger ("triggerDied");
		}

		if (health.Died ())
			return;
		

		if (NimMove.instance.paralized) {
			input = Vector3.zero;
		} else {
			input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
		}
		
		nimAnimator.SetFloat ("horizontal", input.x);
		nimAnimator.SetFloat ("vertical", input.y);

		//player idle looks at previous position
		nimAnimator.SetFloat ("prevHorizontal", prevInput.x);
		nimAnimator.SetFloat ("prevVertical", prevInput.y);
		
		if ((input.x != 0f || input.y != 0f) && !Input.GetButton ("Fire1")) {
			nimAnimator.SetBool ("isWalking", true);
			shadowAnimator.SetBool ("isWalking", true);
		} else {
			nimAnimator.SetBool ("isWalking", false);
			shadowAnimator.SetBool ("isWalking", false);
		}

		SavePreviousInput ();
	}

	void SavePreviousInput(){
		if (input.magnitude != 0) {
			prevInput = input;
		}
	}
}
