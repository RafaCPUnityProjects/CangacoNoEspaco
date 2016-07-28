using UnityEngine;
using System.Collections;

public class RandomEnemyInstantiator : MonoBehaviour
{
	public GameObject enemyPrefab;

	void Start()
	{
		GameObject enemy = (GameObject)Instantiate(enemyPrefab);
		NPCController npcController = enemy.GetComponent<NPCController>();
		npcController.RandomizeMyClothing();
		Destroy(this.gameObject);
	}
}
