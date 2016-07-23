using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextGlitch : MonoBehaviour {

	Text myText;
	string[] glitch = new string[4];

	public float glitchTime = 0.1f;

	float timeToUpdate = 0f;

	void Awake(){
		myText = GetComponent<Text> ();

		glitch[0] = "PLMN!4SVT#32?X3O&QIEN321GD089CMIU$S?87O&IJS&LKNPO92SPLMN!4SVT#32?X3O&QIEN321GD089CMIKNPO92S";
		glitch[1] = "KSJN;MSBYSUG9872NSHFUYPLMN4SVT32X3OQIEN32S90S1X3O&QIEN321GD089GD2NSHFUYPLMN4SVT32X3$S?87O";
		glitch[2] = "QLIJSIXN56SUNX3O&QIEN321GD089SI982KNSSKSJN;MSBYSU?G9&8#72NSHFUYSJN;MSBYSU?G9&8#72NSHF&IJS&";
		glitch[3] = "0&89CMIUS$7OI#JSLKNPO92SQLX3O&QIEN321GD089IJSIXN56SUNSI982KNSSNPO92SQLIJSIXN56SUNSI98UOL";
	}

	void Start(){
		//InvokeRepeating("ChangeGlitch", glitchTime, glitchTime);
	}

	void Update(){

		timeToUpdate -= Time.deltaTime;

		if (timeToUpdate <= 0f) {
			timeToUpdate = Random.Range (0.1f, 1f);

			ChangeGlitch ();
		}

	}

	void ChangeGlitch(){

		myText.text = glitch[Random.Range(0,3)];

	}
}
