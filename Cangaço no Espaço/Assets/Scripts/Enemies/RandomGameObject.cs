using UnityEngine;
using System.Collections;

public class RandomGameObject : MonoBehaviour {

	public GameObject[] gameObjects;

	void Start ()
	{
		GameObject gameObject = (GameObject)Instantiate (gameObjects[Random.Range(0, gameObjects.Length-1)], transform.position, Quaternion.identity);
        gameObject.transform.SetParent(this.transform);
    }

}