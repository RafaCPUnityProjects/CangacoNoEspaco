using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelLoadingDelay : MonoBehaviour {

	Image backgroundPanel;
	public float timeToHide = 3f;

	void Start () {
		backgroundPanel = GetComponent<Image> ();
		Invoke ("HidePanel", timeToHide);
		Invoke ("HideThis", timeToHide + 3f);
	}

	void HidePanel(){
		backgroundPanel.enabled = false;
	}

	void HideThis(){
		this.gameObject.SetActive (false);
	}

}
