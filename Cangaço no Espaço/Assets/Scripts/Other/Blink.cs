using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

	SpriteRenderer sr;

	Material originalMaterial;
	Color originalColor;

	public Material additiveMaterial;

	int blinks;
	public int N_BLINKS = 3;

	float seconds;
	public float T_BLINK = 0.08f;

	bool visible;
	bool blinking = false;

	void Awake(){
		sr = GetComponent<SpriteRenderer> ();

		originalMaterial = sr.material;
		originalColor = sr.color;
	}

	void OnEnable(){
		StartBlink();
	}

	public void StartBlink(){

		seconds = T_BLINK;
		blinks = N_BLINKS;

		visible = true;
		blinking = true;

		Blinkme ();
	}

	void StopBlink(){

		blinking = false;

		sr.material = originalMaterial;
		sr.color = originalColor;

		//Disable script
		this.enabled = false;
	}

	void Update(){

		if (!blinking)
			return;

		seconds -= Time.deltaTime;
		if (seconds <= 0) {
			seconds = T_BLINK;
			Blinkme ();
		}
	}

	void Blinkme(){
		
		SetFx ();

		if(visible)
			blinks--;
		
		if (blinks < 0) {
			StopBlink ();
		}
	}

	void SetFx(){
		
		if (visible) {
			sr.material = additiveMaterial;
			sr.color = Color.white;
		} else {
			sr.material = originalMaterial;
			sr.color = new Color(0,0,0,0);
		}

		visible = !visible;
	}

}
