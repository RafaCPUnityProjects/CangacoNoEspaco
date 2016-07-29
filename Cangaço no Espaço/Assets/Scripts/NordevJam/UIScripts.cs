using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIScripts : MonoBehaviour
{
	public int sceneToLoad = 1;
	public void LoadScene()
	{
		SceneManager.LoadScene(sceneToLoad);
	}

}
