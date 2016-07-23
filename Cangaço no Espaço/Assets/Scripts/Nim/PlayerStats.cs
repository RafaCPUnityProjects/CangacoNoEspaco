using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

	public NimMove nimMove;
	public Slider sliderMove;

	public NimHealth nimHealth;
	public Slider sliderHealth;

	public NimAttack nimAttack;
	public Slider sliderAttack;

	public Slider sliderMelee;

	void Awake(){
		
	}

	void Start(){
		sliderMove.onValueChanged.AddListener (delegate {UpdateMove ();});
		sliderHealth.onValueChanged.AddListener (delegate {UpdateHealth ();});
		sliderAttack.onValueChanged.AddListener (delegate {UpdateAttack ();});
	}

	void UpdateMove(){
		nimMove.walkForce = sliderMove.value;
	}

	void UpdateHealth(){
		nimHealth.lives = (int)sliderHealth.value;
	}

	void UpdateAttack(){
		
	}

}
