using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public NimHealth playerHealth;
	public GameObject enemy;  
	public float spawnTime = 3f;
	public Transform[] spawnPoints;

	int maxEnemiesSpawned = 3; //enemies spawned at the same time
	int enemiesSpawned;

	void Start ()
	{
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}


	void Spawn ()
	{
		if(playerHealth.lives <= 0f)
			return;

		if(transform.GetComponentsInChildren<EnemyHealth> ().Length >= maxEnemiesSpawned)
			return;

		int spawnPointIndex = Random.Range (0, spawnPoints.Length);

		GameObject newEnemy = (GameObject)Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		newEnemy.transform.SetParent (transform);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			InvokeRepeating ("Spawn", spawnTime, spawnTime);
		}
	}
}
