using UnityEngine;
using System.Collections;

public class BossEnemyInstantiator : MonoBehaviour
{
	public GameObject enemyPrefab;

	void Start()
	{
		GameObject enemy = (GameObject)Instantiate(enemyPrefab);
		NPCController npcController = enemy.GetComponent<NPCController>();
		int body = PlayerPrefs.GetInt("BossHead", 0);
		int head = PlayerPrefs.GetInt("BossHead", -1);
		int face = PlayerPrefs.GetInt("BossHead", -1);
		int shirt = PlayerPrefs.GetInt("BossHead", -1);
		int pant = PlayerPrefs.GetInt("BossHead", 0);
		int feet = PlayerPrefs.GetInt("BossHead", -1);
		int knife = PlayerPrefs.GetInt("BossHead", 0);
		BodyInfo bossBodyInfo = new BodyInfo(body, head, face, shirt, pant, feet, knife);
		npcController.DressPlayer(bossBodyInfo);
		Destroy(this.gameObject);
	}
}
