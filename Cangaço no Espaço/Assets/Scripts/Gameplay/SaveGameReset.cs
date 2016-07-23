using UnityEngine;
using System.Collections;

public class SaveGameReset : MonoBehaviour
{
	public bool reset = true;

	void Awake()
	{
		if (reset)
		{
			FindObjectOfType<GameSave>().Reset();
		}
	}
}
