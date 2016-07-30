using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NDJGameController : MonoBehaviour
{
	public float timeToShowBoss = 5f;
	public float timeToPlayer = 5f;

	private Transform player;
	private Transform boss;
	private Transform cameraTarget;
	private NPCController playerController;
	private NPCController bossController;
	private int deathCount;

	void Start()
	{
		StartCoroutine(LookAtBoss());
	}

	IEnumerator LookAtBoss()
	{
		yield return null;
		boss = GameObject.FindGameObjectWithTag("Boss").transform;
		bossController = boss.GetComponent<NPCController>();
		bossController.gameController = this;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerController = player.GetComponent<NPCController>();
		playerController.gameController = this;
		playerController.canMove = false;
		cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;
		cameraTarget.position = boss.position;
		yield return null;
		//yield return new WaitForSeconds(timeToShowBoss);
	}

	IEnumerator LookAtPlayer()
	{
		
		float elapsedTime = 0.0f;
		while (Vector3.Distance(cameraTarget.position, player.position) > 0.1f)
		{
			cameraTarget.position = Vector3.Lerp(boss.position, player.position, elapsedTime / timeToPlayer);
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		cameraTarget.localPosition = Vector3.zero;
		playerController.canMove = true;
	}

	bool gameStarted = false;
	void Update()
	{
		if (gameStarted)
		{
			return;
		}

		if (Input.anyKey)
		{
			gameStarted = true;
			StartCoroutine(LookAtPlayer());
		}
	}

	public void BossDefeated()
	{
		deathCount = 0;
		playerController.myBodyInfo.SaveBody();
		Invoke("RestartScene", 5f);
	}

	public void PlayerDefeated()
	{
		deathCount++;
		Invoke("RestartScene", 5f);
	}

	void RestartScene()
	{
		SceneManager.LoadScene(1);
	}
}
