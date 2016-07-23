using UnityEngine;
using System.Collections;

public class RandomNimStart : MonoBehaviour {

	public GameObject Nim;

	void Start () {
		NimStartPoint[] starts = GameObject.FindObjectsOfType<NimStartPoint> ();
		Nim.transform.position = starts [Random.Range(0,starts.Length-1)].transform.position;
	}
}
