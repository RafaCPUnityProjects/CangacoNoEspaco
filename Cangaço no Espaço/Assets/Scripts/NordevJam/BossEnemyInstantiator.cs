using UnityEngine;
using System.Collections;

public class BossEnemyInstantiator : MonoBehaviour
{
	public GameObject enemyPrefab;

	void Start()
	{
		GameObject enemy = (GameObject)Instantiate(enemyPrefab, transform.position, Quaternion.identity);
		enemy.tag = "Boss";
		enemy.name += " boss";
		NPCController npcController = enemy.GetComponent<NPCController>();
		
		npcController.LoadBossInfo();
		Destroy(this.gameObject);
	}
}
