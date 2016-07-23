using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WakeEnemy : MonoBehaviour {

	public static WakeEnemy instance;

	GameObject[] enemies;
	public GameObject[] sceneEnemies = new GameObject[256];
	public int currentEnemies = 0;

	float nearDistance = 5f;
	float timeToCheck = 0f;
	private const float checkSeconds = 3f;

	void Awake(){

		if (instance == null)
			instance = this;
	}

	void Start(){

		enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		for(int i=0;i<enemies.Length;i++){
			sceneEnemies [i] = enemies [i];
			enemies [i].SetActive (false);
		}
		currentEnemies = enemies.Length;

	}

	void Update(){

		timeToCheck -= Time.deltaTime;
		if (timeToCheck <= 0f) {
			timeToCheck = checkSeconds;
			CheckEnemiesDistance ();
		}

	}

	void CheckEnemiesDistance(){
		//Sleep all far enemies!
		//for is faster than foreach, array faster than List

		for(int i=0;i<currentEnemies;i++){
			if (sceneEnemies[i] != null) {
				sceneEnemies[i].SetActive (Vector2.Distance (transform.position, sceneEnemies[i].transform.position) <= nearDistance);

				//Boss is active
				if (sceneEnemies[i].name == "Enemy Boss")
					sceneEnemies[i].SetActive (true);
			}
		}
	}

	public void AddEnemy(GameObject enemy){
		//currentEnemies++;
		//sceneEnemies [currentEnemies - 1] = enemy;
	}

	public void DestroyEnemy(GameObject enemy){
		//currentEnemies--;
		//sceneEnemies [currentEnemies - 1] = enemy;
	}

}
