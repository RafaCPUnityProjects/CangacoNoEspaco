using UnityEngine;
using System.Collections;
using System;	//events

public class EnemyAttack : MonoBehaviour {

	public int attackPower = 1;
	public float attackRepeatSeconds = 1f;

	GameObject player;
	NimHealth playerHealth;

    public Animator enemyAnimator;
    EnemyHealth health;

	void Awake(){
		GameplayEvents.nimDied.Subscribe (OnNimDied);
		GameplayEvents.nimOnStage.Subscribe (OnNimOnStage);
	}

    void Start()
    {
        health = GetComponent<EnemyHealth>();
    }

	void OnTriggerEnter2D (Collider2D other) {

        if (other.CompareTag ("PlayerHealth")) {
			InvokeRepeating ("Attack", 0f, attackRepeatSeconds);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		
		if (other.CompareTag ("PlayerHealth")) {
			CancelInvoke("Attack");
		}
	}

	void Attack(){

        if (health.died)
            return;

        if (playerHealth != null)
			playerHealth.Damage (attackPower);

		if(enemyAnimator != null)
			enemyAnimator.SetTrigger ("triggerAttack");
	}

	//Events Callbacks
	void OnNimDied(NimHealth nimHealth){
		//Debug.Log ("NIM DIED NOOOO!, STOP WALKING"+nimHealth.lives.ToString());
	}

	void OnNimOnStage(NimHealth nimHealth){
		playerHealth = nimHealth;
	}
}
