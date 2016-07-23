using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour {

	public Text sliderValue;
	Slider slider;
	
	void Start () {
		slider = GetComponent<Slider> ();
		slider.onValueChanged.AddListener (delegate {UpdateValueText ();});
	}

	void UpdateValueText(){
		sliderValue.text = slider.value.ToString();
	}
}
