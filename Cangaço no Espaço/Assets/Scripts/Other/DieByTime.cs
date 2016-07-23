using UnityEngine;
using System.Collections;

public class DieByTime : MonoBehaviour {

	public float lifespan = 1f;
	public bool objectPooled;

	//Blink
	public Animator anim;
	public bool blink;
	public float blinkTime = 2f;

	void Update () {
		
		lifespan -= Time.deltaTime;

		if (blink && lifespan <= blinkTime) {
			blink = false;
			if(anim != null)
				anim.SetTrigger ("blink");
		}

		if (lifespan <= 0f) {
			
			if (objectPooled) {
				ObjectPool.Destroy (this);
			} else {
				Destroy (this.gameObject);
			}
		}
	}

}
