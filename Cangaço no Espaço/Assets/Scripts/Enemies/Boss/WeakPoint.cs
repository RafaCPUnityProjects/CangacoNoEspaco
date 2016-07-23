using UnityEngine;
using System.Collections;

public class WeakPoint : MonoBehaviour {

	EnemyHealth enemyHealth;

	void Start(){
		enemyHealth = transform.parent.GetComponent<EnemyHealth> ();
	}

	void OnTriggerEnter2D(Collider2D other){

		if(enemyHealth != null && enemyHealth.invincible) {
			if (other.CompareTag ("PlayerMelee") || other.CompareTag ("Bullet") || other.CompareTag("Explosion")) {
				enemyHealth.enemyAnimator.SetTrigger ("blink");
			}
			return;
		}

		if (other.CompareTag ("PlayerMelee")) {
			enemyHealth.Damaged (NimAttack.instance.meleeDamage);
		}
        else if (other.CompareTag("Explosion"))
        {
            enemyHealth.Damaged(other.GetComponent<TriggerDamage>().damage);
        }

		if (other.CompareTag ("Bullet")) {
			other.gameObject.GetComponentInParent<Bullet> ().Impact ();
			enemyHealth.Damaged (other.GetComponentInParent<Bullet> ().damage);
		}
	}
}
