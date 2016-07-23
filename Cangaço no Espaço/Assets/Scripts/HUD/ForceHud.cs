using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ForceHud : MonoBehaviour {

	public Sprite[] hudSprites;
	SpriteRenderer sr;

	int maxEnergy;
	public int energy;

	//private
	public bool pushing;
	public float timePushing;

	public float maxTime = 2.5f;

	EnergyHud eh;

	public float hudSpeed = 15f;

	void Start(){

		maxEnergy = hudSprites.Length;
		energy = maxEnergy;

		sr = GetComponent<SpriteRenderer> ();
		eh = transform.parent.GetComponent<EnergyHud>();
	}

	void Update(){

		if (!eh.Max ())
			return;

		if (pushing) {
			timePushing += Time.deltaTime * hudSpeed;
			UpdateSprite ((int)timePushing);
			//UpdateSprite (Mathf.RoundToInt (timePushing * 25f / maxTime));
		} else {
			Reset ();
		}
	}

	void UpdateSprite(int currentEnergy){
		int e = hudSprites.Length - currentEnergy;

		if (e <= hudSprites.Length - 1 && e >= 0) {
			sr.sprite = hudSprites [e];
		}
	}

	public void StartPush(){
		pushing = true;
		Reset ();
	}

	public void StopPush(){
		pushing = false;
		Reset ();
	}

	void Reset(){
		timePushing = 0f;
		energy = 0;
		sr.sprite = hudSprites [hudSprites.Length - 1];
	}

}
