using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlowTyper : MonoBehaviour
{
	public string fullText;
	public float letterSpeed = 0.1f;
	public Text typedText;
	private string currentText;
	public int sceneToLoad = 1;

	void Start()
	{
		StartCoroutine(Type());
	}

	IEnumerator Type()
	{
		currentText = "";
		for (int i = 0; i < fullText.Length; i++)
		{
			//currentText += fullText[i];
			typedText.text += fullText[i];
			yield return new WaitForSeconds(letterSpeed);
		}
	}

	void Update()
	{
		if (Input.anyKey)
		{
			SceneManager.LoadScene(sceneToLoad);
		}
	}
}
