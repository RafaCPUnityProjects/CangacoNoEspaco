using UnityEngine;
using System.Collections;

public class RandomLevel : MonoBehaviour {

	public GameObject[] levels;

	void Awake(){
		levels[Random.Range (0,levels.Length)].SetActive(true);
	}
}
