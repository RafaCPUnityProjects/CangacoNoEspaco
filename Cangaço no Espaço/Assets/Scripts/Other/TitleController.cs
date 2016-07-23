using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

	public GameObject panelLoading;
	public Animator loadingImageAnim;

	public void ExitGame(){
		Application.Quit ();
	}

	public void LoadGame(){
		panelLoading.SetActive (true);
		Invoke ("LoadStage", 3.5f);
	}

	void LoadStage(){
		loadingImageAnim.SetTrigger ("gameLoaded");
		//Application.LoadLevel (Application.loadedLevel + 1);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
