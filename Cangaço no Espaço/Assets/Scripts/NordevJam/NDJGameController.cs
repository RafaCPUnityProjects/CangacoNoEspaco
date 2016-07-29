using UnityEngine;
using System.Collections;

public class NDJGameController : MonoBehaviour
{
	public float timeToShowBoss = 5f;
	public float timeToPlayer = 5f;

	private Transform player;
	private Transform boss;
	private Transform cameraTarget;

	private int deathCount;

	void Start()
	{

		StartCoroutine("FocusBoss");
	}

	IEnumerator FocusBoss()
	{
		yield return null;
		boss = GameObject.FindGameObjectWithTag("Boss").transform;
		boss.GetComponent<NPCController>().gameController = this;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		player.GetComponent<NPCController>().gameController = this;
		cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;
		yield return null;
		cameraTarget.position = boss.position;
		yield return new WaitForSeconds(timeToShowBoss);
		float elapsedTime = 0.0f;
		while (Vector3.Distance(cameraTarget.position, player.position) > 0.1f)
		{
			cameraTarget.position = Vector3.Lerp(boss.position, player.position, elapsedTime / timeToPlayer);
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		cameraTarget.position = player.position;
	}

	public void BossDefeated()
	{

	}

	public void PlayerDefeated()
	{

	}
}
