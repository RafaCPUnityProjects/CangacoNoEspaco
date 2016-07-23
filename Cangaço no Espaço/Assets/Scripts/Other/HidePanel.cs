using UnityEngine;
using System.Collections;

public class HidePanel : MonoBehaviour {

	public GameObject panel;

	public void HideBackgroundPanel(){
		panel.SetActive (false);
	}
}
