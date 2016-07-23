using UnityEngine;
using System.Collections;

public class DropCoin : MonoBehaviour {

	public int coinsDropped = 1;
    public float timeToDie = 10f;

	//Blink
	public Animator anim;
	public bool blink;
	public float blinkTime = 2f;

	void Start(){

		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized, ForceMode2D.Impulse);

		if (timeToDie > 0f) {
			
			Invoke ("Blink", blinkTime);
			Invoke ("Killme", timeToDie);

		}
	}

    void Killme()
    {
        Destroy(this.gameObject);
    }

	void Blink(){
		if(anim != null)
			anim.SetTrigger ("blink");
	}
}
