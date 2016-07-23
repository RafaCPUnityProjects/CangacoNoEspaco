using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnableGlitch : MonoBehaviour {

	Transform nim;
	TextGlitch textGlitch;
	Text mainText;
	public float glitchDistance = 4f;

	float timeToUpdate;

	void Awake(){
		GameplayEvents.nimOnStage.Subscribe(OnNimOnStage);
		textGlitch = GetComponentInChildren<TextGlitch> ();
		mainText = GetComponentInChildren<Text> ();

		timeToUpdate = Random.Range (1f,2f);
	}

	private void OnNimOnStage(NimHealth nimHealth)
	{
		nim = nimHealth.transform;
	}

	void Update(){
		if (nim == null)
			return;

		timeToUpdate -= Time.deltaTime;

		if (timeToUpdate > 0)
			return;
		else
			timeToUpdate = Random.Range (1f,2f);

		if (Vector3.Distance (nim.position, transform.position) < glitchDistance) {
			textGlitch.enabled = true;
			mainText.enabled = true;
		} else {
			textGlitch.enabled = false;
			mainText.enabled = false;
		}
	}
}
